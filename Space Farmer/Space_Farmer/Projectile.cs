using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Space_Farmer
{
    class Projectile
    {
        public static List<Projectile> projectiles = new List<Projectile>();
        public static Texture2D laserBeam;
        public static Texture2D rocketTexture;
        public static SoundEffect[] laserSound = new SoundEffect[2];
        public static int[] laserSize = { 4, 32 };
        public static int[] rocketSize = { 50, 180 };

        public static void drawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile b in projectiles)
            {
                if (!b.rocket)
                {
                    spriteBatch.Draw(laserBeam, new Rectangle((int)(b.position.X + Game1.offset.X), (int)(b.position.Y + Game1.offset.Y), laserSize[0], laserSize[1]), null, b.color, b.angle, new Vector2(laserBeam.Width / 2, laserBeam.Height / 2), SpriteEffects.None, 0F);
                }
                else
                {
                    spriteBatch.Draw(rocketTexture, new Rectangle((int)(b.position.X + Game1.offset.X), (int)(b.position.Y + Game1.offset.Y), rocketSize[0], rocketSize[1]), null, Color.White, b.angle, new Vector2(rocketTexture.Width / 2, rocketTexture.Height / 2), SpriteEffects.None, 0F);
                }
            }
        }

        public static int explosionSize = 100;
        public static void movement()
        {
            for (int i = 0; i < projectiles.Count(); i++)
            {
                if (!projectiles[i].rocket)
                {
                    projectiles[i].projectileMovement();
                }
                else
                {
                    //Rocket movement
                    projectiles[i].rocketMovement();
                }

                if (projectiles[i].delete)
                {
                    ActiveKeyframeEffect.activeKeyframeEffects.Add(new ActiveKeyframeEffect(KeyframeEffect.keyframeEffects[1], new Rectangle((int)projectiles[i].position.X - (explosionSize / 2), (int)projectiles[i].position.Y - (explosionSize / 2), explosionSize, explosionSize), projectiles[i].color));
                    projectiles.Remove(projectiles[i]);

                    i--;
                    if (i < 0)
                    {
                        return;
                    }
                }
            }
        }

        //Object
        public bool delete = false;
        public Vector2 position;
        public Vector2 pastPos;
        float angle;
        float damage;
        float[] velocity = new float[2];
        bool friendly = false;
        public Color color;
        public Tile target = null;
        public Tile tileFrom = null;
        public bool rocket = false;
        public float projectileSpeed;

        public void checkCollision()
        {
            if (!rocket)
            {
                //Tile Check
                if (!friendly)
                {
                    if (target != null)
                    {
                        if (target.built)
                        {
                            if (target.tileRectangle.Contains((int)(position.X + Game1.offset.X), (int)(position.Y + Game1.offset.Y)))
                            {
                                //Has collided
                                target.health -= (int)damage;

                                if (target.health <= 0)
                                {
                                    target.breakBuilding();
                                }

                                delete = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        //Can affect all tiles
                        foreach (Tile b in Tile.tiles)
                        {
                            if (b.built)
                            {
                                if (b.tileRectangle.Contains((int)(position.X + Game1.offset.X), (int)(position.Y + Game1.offset.Y)))
                                {
                                    //Has collided
                                    b.health -= (int)damage;

                                    if (b.health <= 0)
                                    {
                                        b.breakBuilding();
                                    }

                                    delete = true;
                                    return;
                                }
                            }
                        }
                    }
                }

                //Enemy Check
                if (friendly)
                {
                    Vector2 updatedPos = new Vector2(position.X + Game1.offset.X, position.Y + Game1.offset.Y);
                    Vector2 updatedPastPos = new Vector2(pastPos.X + Game1.offset.X, pastPos.Y + Game1.offset.Y);

                    foreach (Enemy b in Enemy.enemies)
                    {
                        Vector2 midpoint = new Vector2((position.X + pastPos.X) / 2, (position.Y + pastPos.Y) / 2);
                        Vector2 collisionPoint = b.position;
                        float xDist = Math.Abs(position.X - pastPos.X);
                        float yDist = Math.Abs(position.Y - pastPos.Y);

                        collisionPoint.X = Operation.clamp(collisionPoint.X, midpoint.X - (xDist / 2), midpoint.X + (xDist / 2));
                        collisionPoint.Y = Operation.clamp(collisionPoint.Y, midpoint.Y - (yDist / 2), midpoint.Y + (yDist / 2));

                        if (Operation.distance(collisionPoint, b.position) <= b.size[1])
                        {
                            //In Range
                            float projectileSlope = (updatedPastPos.Y - updatedPos.Y) / (updatedPastPos.X - updatedPos.X);
                            float projectileYIntercept = updatedPos.Y - (projectileSlope * updatedPos.X);
                            float projMinX = -1, projMaxX = -1, projMinY = -1, projMaxY = -1;
                            projMinX = updatedPos.X;
                            projMaxX = updatedPos.X;
                            projMinY = updatedPos.Y;
                            projMaxY = updatedPos.Y;
                            if (updatedPastPos.X < projMinX)
                            {
                                projMinX = updatedPastPos.X;
                            }
                            if (updatedPastPos.X > projMaxX)
                            {
                                projMaxX = updatedPastPos.X;
                            }
                            if (updatedPastPos.Y < projMinY)
                            {
                                projMinY = updatedPastPos.Y;
                            }
                            if (updatedPastPos.Y > projMaxY)
                            {
                                projMaxY = updatedPastPos.Y;
                            }

                            for (int i = 0; i < b.collisionPoint.Count() - 1; i++)
                            {
                                float enemyMinX = -1, enemyMaxX = -1, enemyMinY = -1, enemyMaxY = -1;
                                foreach (Vector2 p in b.collisionPoint)
                                {
                                    if (p.X < enemyMinX || enemyMinX == -1)
                                    {
                                        enemyMinX = p.X;
                                    }
                                    if (p.X > enemyMaxX || enemyMaxX == -1)
                                    {
                                        enemyMaxX = p.X;
                                    }
                                    if (p.Y < enemyMinY || enemyMinY == -1)
                                    {
                                        enemyMinY = p.Y;
                                    }
                                    if (p.Y > enemyMaxY || enemyMaxY == -1)
                                    {
                                        enemyMaxY = p.Y;
                                    }
                                }

                                float enemySlope = (b.collisionPoint[i + 1].Y - b.collisionPoint[i].Y) / (b.collisionPoint[i + 1].X - b.collisionPoint[i].X);
                                float enemyYIntercept = b.collisionPoint[i].Y - (enemySlope * b.collisionPoint[i].X);

                                Vector2 intersection = new Vector2();
                                intersection.X = ((enemyYIntercept - projectileYIntercept) / (projectileSlope - enemySlope));
                                intersection.Y = (projectileSlope * intersection.X) + projectileYIntercept;

                                if ((intersection.X >= enemyMinX && intersection.X <= enemyMaxX && intersection.Y >= enemyMinY && intersection.Y <= enemyMaxY) && (intersection.X >= projMinX && intersection.X <= projMaxX && intersection.Y >= projMinY && intersection.Y <= projMaxY))
                                {
                                    //Collided
                                    b.health -= (int)(damage + (damage * Buffs.buffs[1]));

                                    //Adds tag
                                    if (b.tag == null && tileFrom != null)
                                    {
                                        b.tag = tileFrom;
                                    }

                                    if (b.health <= 0)
                                    {
                                        b.delete = true;
                                        //Adds gold for kill
                                        Player.gold += (int)(10 + (10 * Buffs.buffs[3]));
                                    }

                                    delete = true;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Vector2 updatedPos = new Vector2(position.X + Game1.offset.X, position.Y + Game1.offset.Y);
                Vector2 collisionPoint = new Vector2(updatedPos.X, updatedPos.Y - (rocketSize[1] / 2));
                Vector2 playerCenter = new Vector2(Game1.Width / 2, Game1.Height / 2);
                float xWithout = collisionPoint.X - updatedPos.X;
                float yWithout = collisionPoint.Y - updatedPos.Y;
                collisionPoint.X = (float)(updatedPos.X + ((xWithout * Math.Cos(angle)) - (yWithout * Math.Sin(angle))));
                collisionPoint.Y = (float)(updatedPos.Y + ((xWithout * Math.Sin(angle)) + (yWithout * Math.Cos(angle))));

                for (int i = 0; i < Player.collisionPoint.Count() - 1; i++)
                {
                    float slope = (Player.collisionPoint[i + 1].Y - Player.collisionPoint[i].Y) / (Player.collisionPoint[i + 1].X - Player.collisionPoint[i].X);
                    float yIntercept = Player.collisionPoint[i].Y - (slope * Player.collisionPoint[i].X);

                    if (playerCenter.Y > (slope * playerCenter.X) + yIntercept)
                    {
                        //Y is greater
                        if (!(collisionPoint.Y > (slope * collisionPoint.X) + yIntercept))
                        {
                            return;
                        }
                    }
                    else
                    {
                        //Y is less than
                        if (!(collisionPoint.Y < (slope * collisionPoint.X) + yIntercept))
                        {
                            return;
                        }
                    }
                }

                //Reached collision
                Player.dead = true;
            }
        }

        public Projectile(Vector2 positionInput, float angleInput, float projectileSpeed, float damageInput, Color colorInput, Tile targetInput)
        {
            position = positionInput;
            pastPos = position;

            angle = angleInput;

            damage = damageInput;

            this.projectileSpeed = projectileSpeed;
            velocity[0] = (float)(Math.Sin(angle) * projectileSpeed);
            velocity[1] = (float)(Math.Cos(angle) * projectileSpeed);

            color = colorInput;

            target = targetInput;

            //Play Sound
            Random random = new Random();
            laserSound[random.Next(0, laserSound.Count())].Play(.5F, 0, 0);
        }

        public Projectile(Vector2 positionInput, float projectileSpeed, Color colorInput, bool rocketInput)
        {
            rocket = rocketInput;
            position = positionInput;
            pastPos = position;

            this.projectileSpeed = projectileSpeed;
            velocity[0] = (float)(Math.Sin(angle) * projectileSpeed);
            velocity[1] = (float)(Math.Cos(angle) * projectileSpeed);

            color = colorInput;

            //Play Sound
            Random random = new Random();
            laserSound[random.Next(0, laserSound.Count())].Play(.5F, 0, 0);
        }

        public Projectile(Vector2 positionInput, float angleInput, float projectileSpeed, float damageInput, bool friendlyInput, Color colorInput, Tile tileFromInput, bool muted)
        {
            position = positionInput;
            pastPos = position;

            angle = angleInput;
            friendly = friendlyInput;
            damage = damageInput;

            this.projectileSpeed = projectileSpeed;
            velocity[0] = (float)(Math.Sin(angle) * projectileSpeed);
            velocity[1] = (float)(Math.Cos(angle) * projectileSpeed);

            color = colorInput;

            tileFrom = tileFromInput;

            if (!muted)
            {
                //Play Sound
                Random random = new Random();
                laserSound[random.Next(0, laserSound.Count())].Play(.5F, 0, 0);
            }
        }

        public void rocketMovement()
        {
            Vector2 targetPosition = new Vector2(Game1.Width / 2, Game1.Height / 2);
            Vector2 updatedPos = new Vector2((position.X + Game1.offset.X), (position.Y + Game1.offset.Y));
            float xDist = Math.Abs(targetPosition.X - updatedPos.X);
            float yDist = Math.Abs(targetPosition.Y - updatedPos.Y);
            float angle = (float)Math.Atan(xDist / yDist);
            float horizontalTravelDistance = (float)(Math.Sin(angle) * projectileSpeed);
            float verticalTravelDistance = (float)(Math.Cos(angle) * projectileSpeed);

            if (targetPosition.X < updatedPos.X)
            {
                //Left
                if (targetPosition.Y < updatedPos.Y)
                {
                    //Up
                    angle = (float)(360 * (Math.PI / 180)) - angle;
                }
                else
                {
                    //Down
                    angle += (float)(180 * (Math.PI / 180));
                }
            }
            else
            {
                //Right
                if (targetPosition.Y < updatedPos.Y)
                {
                    //Up
                }
                else
                {
                    //Down
                    angle = (float)(180 * (Math.PI / 180)) - angle;
                }
            }
            this.angle = angle;

            if (targetPosition.X > updatedPos.X)
            {
                //Right
                if (targetPosition.Y > updatedPos.Y)
                {
                    //Down
                    position.X += horizontalTravelDistance;
                    position.Y += verticalTravelDistance;
                }
                else
                {
                    //Up
                    position.X += horizontalTravelDistance;
                    position.Y -= verticalTravelDistance;
                }
            }
            else
            {
                //Left
                if (targetPosition.Y > updatedPos.Y)
                {
                    //Down
                    position.X -= horizontalTravelDistance;
                    position.Y += verticalTravelDistance;
                }
                else
                {
                    //Up
                    position.X -= horizontalTravelDistance;
                    position.Y -= verticalTravelDistance;
                }
            }

            checkCollision();
        }

        public void projectileMovement()
        {
            pastPos = position;

            position.X += velocity[0];
            position.Y -= velocity[1];

            //Bounds Check
            if (position.X > (Game1.asteroidSize) || position.X < -(Game1.asteroidSize) || position.Y > (Game1.asteroidSize) || position.Y < -(Game1.asteroidSize)) 
            {
                delete = true;
            }

            checkCollision();
        }
    }
}
