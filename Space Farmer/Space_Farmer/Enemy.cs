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
    class Enemy
    {
        public static SoundEffect enemyExplosion;
        public static float arrivalBuffer = 100;
        public static List<Enemy> enemies = new List<Enemy>();
        public static Random random = new Random();

        public static Texture2D[] enemyTextures = new Texture2D[3];
        public static List<Vector2>[] enemyCollisionPoints = new List<Vector2>[3];

        public static int roundedEnemyShotAmount = 10;

        public static int defaultSize = 200;
        public static void spawnEnemies(int number)
        {
            Random random = new Random();

            if (Game1.turnNumber % 25 == 0)
            {
                Vector2 position = new Vector2(Game1.Width / 2, Game1.Height / 2);

                //Boss fight!
                if (random.Next(0, 2) == 0)
                {
                    //Y Axis
                    if (random.Next(0, 2) == 0)
                    {
                        //Up
                        position.X += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                        position.Y -= Game1.asteroidSize;
                        enemies.Add(new Enemy(position, .1F, (Game1.turnNumber / 25) * 25, 600, 400, 400, random.Next(900, 1100), (Game1.turnNumber / 25) * 1500, 2));
                    }
                    else
                    {
                        //Down
                        position.X += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                        position.Y += Game1.asteroidSize;
                        enemies.Add(new Enemy(position, .1F, (Game1.turnNumber / 25) * 25, 600, 400, 400, random.Next(900, 1100), (Game1.turnNumber / 25) * 1500, 2));
                    }
                }
                else
                {
                    //X Axis
                    if (random.Next(0, 2) == 0)
                    {
                        //Left
                        position.X -= Game1.asteroidSize;
                        position.Y += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                        enemies.Add(new Enemy(position, .1F, (Game1.turnNumber / 25) * 25, 600, 400, 400, random.Next(900, 1100), (Game1.turnNumber / 25) * 1500, 2));
                    }
                    else
                    {
                        //Right
                        position.X += Game1.asteroidSize;
                        position.Y += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                        enemies.Add(new Enemy(position, .1F, (Game1.turnNumber / 25) * 25, 600, 400, 400, random.Next(900, 1100), (Game1.turnNumber / 25) * 1500, 2));
                    }
                }
            }
            else
            {
                //Standard spawn
                for (int i = 0; i < number; i++)
                {
                    Vector2 position = new Vector2(Game1.Width / 2, Game1.Height / 2);

                    if (random.Next(0, 10) == 0)
                    {
                        //Rounded enemy
                        if (random.Next(0, 2) == 0)
                        {
                            //Y Axis
                            if (random.Next(0, 2) == 0)
                            {
                                //Up
                                position.X += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                                position.Y -= Game1.asteroidSize;
                                enemies.Add(new Enemy(position, .15F, (number / 2) + 1, 350, 150, 250, random.Next(1500, 2000), 250 + number, 1));
                            }
                            else
                            {
                                //Down
                                position.X += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                                position.Y += Game1.asteroidSize;
                                enemies.Add(new Enemy(position, .15F, (number / 2) + 1 + 1, 350, 150, 250, random.Next(1500, 2000), 250 + number, 1));
                            }
                        }
                        else
                        {
                            //X Axis
                            if (random.Next(0, 2) == 0)
                            {
                                //Left
                                position.X -= Game1.asteroidSize;
                                position.Y += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                                enemies.Add(new Enemy(position, .15F, (number / 2) + 1 + 1, 350, 150, 250, random.Next(1500, 2000), 250 + number, 1));
                            }
                            else
                            {
                                //Right
                                position.X += Game1.asteroidSize;
                                position.Y += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                                enemies.Add(new Enemy(position, .15F, (number / 2) + 1 + 1, 350, 150, 250, random.Next(1500, 2000), 250 + number, 1));
                            }
                        }
                    }
                    else
                    {
                        //Default enemy
                        if (random.Next(0, 2) == 0)
                        {
                            //Y Axis
                            if (random.Next(0, 2) == 0)
                            {
                                //Up
                                position.X += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                                position.Y -= Game1.asteroidSize;
                                enemies.Add(new Enemy(position, .2F, (number / 2) + 1, 350, 75, 200, random.Next(900, 1100), 100 + number, 0));
                            }
                            else
                            {
                                //Down
                                position.X += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                                position.Y += Game1.asteroidSize;
                                enemies.Add(new Enemy(position, .2F, (number / 2) + 1, 350, 75, 200, random.Next(900, 1100), 100 + number, 0));
                            }
                        }
                        else
                        {
                            //X Axis
                            if (random.Next(0, 2) == 0)
                            {
                                //Left
                                position.X -= Game1.asteroidSize;
                                position.Y += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                                enemies.Add(new Enemy(position, .2F, (number / 2) + 1, 350, 75, 200, random.Next(900, 1100), 100 + number, 0));
                            }
                            else
                            {
                                //Right
                                position.X += Game1.asteroidSize;
                                position.Y += random.Next(-Game1.asteroidSize, Game1.asteroidSize);
                                enemies.Add(new Enemy(position, .2F, (number / 2) + 1, 350, 75, 200, random.Next(900, 1100), 100 + number, 0));
                            }
                        }
                    }
                }
            }
        }

        public static void updateEnemies(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Count(); i++)
            {
                if (enemies[i].delete)
                {
                    ActiveKeyframeEffect.activeKeyframeEffects.Add(new ActiveKeyframeEffect(KeyframeEffect.keyframeEffects[0], new Rectangle((int)enemies[i].position.X - (enemies[i].size[1] / 2), (int)enemies[i].position.Y - (enemies[i].size[1] / 2), enemies[i].size[1], enemies[i].size[1]), Color.White));
                    enemies.Remove(enemies[i]);
                    enemyExplosion.Play();
                    i--;
                    if (i < 0)
                    {
                        return;
                    }
                }

                enemies[i].movementDetection(gameTime);
                enemies[i].initializeCollisionPoints();

                //Asks if out of bounds
                Vector2 center = new Vector2(Game1.Width / 2, Game1.Height / 2);
                if (Operation.distance(enemies[i].position, center) >= ((float)Game1.asteroidSize * 1.5F))
                {
                    enemies[i].delete = true;
                }

                if (gameTime.TotalGameTime.TotalMilliseconds - enemies[i].lastShotTime > enemies[i].shootSpeed || enemies[i].lastShotTime == -1)
                {
                    enemies[i].lastShotTime = (long)gameTime.TotalGameTime.TotalMilliseconds;
                    enemies[i].shoot();
                }
            }
        }

        public static void drawEnemies(SpriteBatch spriteBatch)
        {
            foreach (Enemy b in enemies)
            {
                spriteBatch.Draw(enemyTextures[b.imageID], new Rectangle((int)(b.position.X + Game1.offset.X), (int)(b.position.Y + Game1.offset.Y), b.size[0], b.size[1]), null, Color.White, b.currentAngle, new Vector2(enemyTextures[b.imageID].Width / 2, enemyTextures[b.imageID].Height / 2), SpriteEffects.None, 0F);

                //Test Collision Points
                //foreach (Vector2 j in b.collisionPoint)
                //{
                //    spriteBatch.Draw(Minimap.blipTexture, new Rectangle((int)(j.X - (Minimap.blipSize / 2)), (int)(j.Y - (Minimap.blipSize / 2)), Minimap.blipSize, Minimap.blipSize), Color.White);
                //}
            }
        }

        public static void setCollisionPoints()
        {
            enemyCollisionPoints[0] = new List<Vector2>();
            enemyCollisionPoints[0].Add(new Vector2(0, 138));
            enemyCollisionPoints[0].Add(new Vector2(-80, 80));
            enemyCollisionPoints[0].Add(new Vector2(0, -138));
            enemyCollisionPoints[0].Add(new Vector2(80, 80));
            enemyCollisionPoints[0].Add(new Vector2(0, 138));

            enemyCollisionPoints[1] = new List<Vector2>();
            enemyCollisionPoints[1].Add(new Vector2(-102, 184));
            enemyCollisionPoints[1].Add(new Vector2(-102, -69));
            enemyCollisionPoints[1].Add(new Vector2(0, -184));
            enemyCollisionPoints[1].Add(new Vector2(102, 184));
            enemyCollisionPoints[1].Add(new Vector2(102, -69));
            enemyCollisionPoints[1].Add(new Vector2(-102, -69));

            enemyCollisionPoints[2] = new List<Vector2>();
            enemyCollisionPoints[2].Add(new Vector2(-136, 85));
            enemyCollisionPoints[2].Add(new Vector2(-100, -46));
            enemyCollisionPoints[2].Add(new Vector2(-76, -100));
            enemyCollisionPoints[2].Add(new Vector2(0, -125));
            enemyCollisionPoints[2].Add(new Vector2(136, 85));
            enemyCollisionPoints[2].Add(new Vector2(100, -46));
            enemyCollisionPoints[2].Add(new Vector2(76, -100));
            enemyCollisionPoints[2].Add(new Vector2(0, 125));
            enemyCollisionPoints[2].Add(new Vector2(-136, 85));
        }

        //Object
        public bool delete = false;
        public int health;
        private int defaultHealth;
        public int imageID;
        public Vector2 position;
        public float[] velocity = {0, 0};
        public float acceleration;
        public float radius;
        public float damage;
        public float shootSpeed;
        public long lastShotTime = -1;
        public int[] size = new int[2];
        public Tile target;
        public Vector2[] collisionPoint = new Vector2[0];
        public float currentAngle = 0;
        public Tile tag = null;

        public Enemy(Vector2 positionInput, float accelerationInput, float damageInput, float radiusInput, int sizeX, int sizeY, float shootSpeedInput, int health, int imageID)
        {
            position = positionInput;
            acceleration = accelerationInput;
            radius = radiusInput;
            damage = damageInput;
            size[0] = sizeX;
            size[1] = sizeY;
            shootSpeed = shootSpeedInput;

            this.imageID = imageID;
            defaultHealth = health;
            this.health = defaultHealth;

            Array.Resize(ref collisionPoint, enemyCollisionPoints[imageID].Count());

            initializeCollisionPoints();
        }

        public void shoot()
        {
            if (imageID == 1)
            {
                Vector2 updatedPos = new Vector2(position.X + Game1.offset.X, position.Y + Game1.offset.Y);
                Vector2 targetCenter1 = new Vector2(target.tileRectangle.X + (target.tileRectangle.Width / 2), target.tileRectangle.Y + (target.tileRectangle.Height / 2));

                if (Operation.distance(updatedPos, targetCenter1) <= radius)
                {
                    if (random.Next(0, 6) == 5)
                    {
                        Projectile.projectiles.Add(new Projectile(position, Player.laserSpeed / 12, Color.White, true));
                    }
                    else
                    {
                        float increment = MathHelper.ToRadians(360) / roundedEnemyShotAmount;

                        for (int i = 0; i < roundedEnemyShotAmount; i++)
                        {
                            Projectile.projectiles.Add(new Projectile(position, (increment * i), Player.laserSpeed, damage, false, Color.Red, null, true));
                        }

                        //Play Sound
                        Random random = new Random();
                        Projectile.laserSound[random.Next(0, Projectile.laserSound.Count())].Play(.5F, 0, 0);
                    }
                }
            }
            else if (imageID == 2)
            {
                Vector2 updatedPos = new Vector2(position.X + Game1.offset.X, position.Y + Game1.offset.Y);
                Vector2 targetCenter1 = new Vector2(target.tileRectangle.X + (target.tileRectangle.Width / 2), target.tileRectangle.Y + (target.tileRectangle.Height / 2));

                if (Operation.distance(updatedPos, targetCenter1) <= radius)
                {
                    Projectile.projectiles.Add(new Projectile(position, Player.laserSpeed / 12, Color.White, true));
                    Projectile.projectiles.Add(new Projectile(position, currentAngle, Player.laserSpeed, damage, false, Color.Red, target, false));
                }
            }
            else
            {
                Vector2 updatedPos = new Vector2(position.X + Game1.offset.X, position.Y + Game1.offset.Y);
                Vector2 targetCenter1 = new Vector2(target.tileRectangle.X + (target.tileRectangle.Width / 2), target.tileRectangle.Y + (target.tileRectangle.Height / 2));

                if (Operation.distance(updatedPos, targetCenter1) <= radius)
                {
                    Projectile.projectiles.Add(new Projectile(position, currentAngle, Player.laserSpeed, damage, false, Color.Red, target, false));
                }
            }
        }

        public void movementDetection(GameTime gameTime)
        {
            Vector2 updatedPos = new Vector2(position.X + Game1.offset.X, position.Y + Game1.offset.Y);
            Tile oldTarget = target;

            //Finds closest tile
            float minDist = -1;
            foreach (Tile b in Tile.tiles)
            {
                if (b.built)
                {
                    Vector2 tileCenter = new Vector2(b.tileRectangle.X + (b.tileRectangle.Width / 2), b.tileRectangle.Y + (b.tileRectangle.Height / 2));
                    float dist1 = Operation.distance(updatedPos, tileCenter);

                    if (dist1 < minDist || minDist == -1)
                    {
                        minDist = dist1;
                        target = b;
                    }
                }
            }

            if (minDist == -1)
            {
                return;
            }

            //Makes a defense building the target if it was shot by it
            if (health <= defaultHealth / 2)
            {
                if (tag != null && !target.building.defense)
                {
                    if (tag.built)
                    {
                        target = tag;
                    }
                }
            }

            //Checks if target has changed
            if (oldTarget != target)
            {
                velocity[0] = 0;
                velocity[1] = 0;
            }

            Vector2 targetCenter1 = new Vector2(target.tileRectangle.X + (target.tileRectangle.Width / 2), target.tileRectangle.Y + (target.tileRectangle.Height / 2));

            float xDist = Math.Abs(targetCenter1.X - updatedPos.X);
            float yDist = Math.Abs(targetCenter1.Y - updatedPos.Y);

            float angle = (float)Math.Atan(xDist / yDist);
            float dist = (float)Math.Sqrt((xDist * xDist) + (yDist * yDist));

            float minHorizontalDist = (float)(Math.Sin(angle) * (dist - radius));
            float minVerticalDist = (float)(Math.Cos(angle) * (dist - radius));

            float horizontalDrag = (float)(Math.Sin(angle) * Player.drag);
            float verticalDrag = (float)(Math.Cos(angle) * Player.drag);
            float horizontalAccelerationCutoffDist = (velocity[0] / 2) * (-velocity[0] / -horizontalDrag);
            float verticalAccelerationCutoffDist = (velocity[1] / 2) * (-velocity[1] / -verticalDrag);

            //Creates sample angle
            //Sets complete angle
            float finalAngle = angle;
            if (targetCenter1.X < updatedPos.X)
            {
                //Left
                if (targetCenter1.Y < updatedPos.Y)
                {
                    //Up
                    finalAngle = (float)(360 * (Math.PI / 180)) - angle;
                }
                else
                {
                    //Down
                    finalAngle += (float)(180 * (Math.PI / 180));
                }
            }
            else
            {
                //Right
                if (targetCenter1.Y < updatedPos.Y)
                {
                    //Up
                }
                else
                {
                    //Down
                    finalAngle = (float)(180 * (Math.PI / 180)) - angle;
                }
            }

            if (currentAngle != finalAngle)
            {
                if (finalAngle < currentAngle)
                {
                    currentAngle -= (acceleration / 3.5F);                   
                }
                else
                {
                    currentAngle += (acceleration / 3.5F);
                }

                if (Math.Abs(currentAngle - finalAngle) <= (acceleration / 3.5F) * 2)
                {
                    currentAngle = finalAngle;
                }
                else
                {
                    return;
                }
            }

            if (horizontalAccelerationCutoffDist >= minHorizontalDist && verticalAccelerationCutoffDist >= minVerticalDist)
            {
                //No accelerate (Only drag)

                if (velocity[0] == 0 && velocity[1] == 0)
                {
                    return;
                }

                if (targetCenter1.X < updatedPos.X)
                {
                    //Left
                    if (targetCenter1.Y < updatedPos.Y)
                    {
                        //Up
                        float horizontalFinalVelocity = velocity[0] + horizontalDrag;
                        position.X += (velocity[0] + horizontalFinalVelocity) / 2;
                        velocity[0] = horizontalFinalVelocity;

                        float verticalFinalVelocity = velocity[1] + verticalDrag;
                        position.Y += (velocity[1] + verticalFinalVelocity) / 2;
                        velocity[1] = verticalFinalVelocity;
                    }
                    else
                    {
                        //Down
                        float horizontalFinalVelocity = velocity[0] + horizontalDrag;
                        position.X += (velocity[0] + horizontalFinalVelocity) / 2;
                        velocity[0] = horizontalFinalVelocity;

                        float verticalFinalVelocity = velocity[1] - verticalDrag;
                        position.Y += (velocity[1] + verticalFinalVelocity) / 2;
                        velocity[1] = verticalFinalVelocity;
                    }
                }
                else
                {
                    //Right
                    if (targetCenter1.Y < updatedPos.Y)
                    {
                        //Up
                        float horizontalFinalVelocity = velocity[0] - horizontalDrag;
                        position.X += (velocity[0] + horizontalFinalVelocity) / 2;
                        velocity[0] = horizontalFinalVelocity;

                        float verticalFinalVelocity = velocity[1] + verticalDrag;
                        position.Y += (velocity[1] + verticalFinalVelocity) / 2;
                        velocity[1] = verticalFinalVelocity;
                    }
                    else
                    {
                        //Down
                        float horizontalFinalVelocity = velocity[0] - horizontalDrag;
                        position.X += (velocity[0] + horizontalFinalVelocity) / 2;
                        velocity[0] = horizontalFinalVelocity;

                        float verticalFinalVelocity = velocity[1] - verticalDrag;
                        position.Y += (velocity[1] + verticalFinalVelocity) / 2;
                        velocity[1] = verticalFinalVelocity;
                    }
                }

                //Increase drag buffer multiplier if enemies randomly fly away
                if (Math.Abs(velocity[0]) <= horizontalDrag * 5 && Math.Abs(velocity[1]) <= verticalDrag * 5)
                {
                    velocity[0] = 0;
                    velocity[1] = 0;
                }
            }
            else
            {
                //Continue acceleration

                if (targetCenter1.X < updatedPos.X)
                {
                    //Left
                    if (targetCenter1.Y < updatedPos.Y)
                    {
                        //Up
                        float horizontalAcceleration = (float)(Math.Sin(angle) * acceleration);
                        float verticalAcceleration = (float)(Math.Cos(angle) * acceleration);

                        float horizontalFinalVelocity = velocity[0] - horizontalAcceleration + horizontalDrag;
                        position.X += (velocity[0] + horizontalFinalVelocity) / 2;
                        velocity[0] = horizontalFinalVelocity;

                        float verticalFinalVelocity = velocity[1] - verticalAcceleration + verticalDrag;
                        position.Y += (velocity[1] + verticalFinalVelocity) / 2;
                        velocity[1] = verticalFinalVelocity;
                    }
                    else
                    {
                        //Down
                        float horizontalAcceleration = (float)(Math.Sin(angle) * acceleration);
                        float verticalAcceleration = (float)(Math.Cos(angle) * acceleration);

                        float horizontalFinalVelocity = velocity[0] - horizontalAcceleration + horizontalDrag;
                        position.X += (velocity[0] + horizontalFinalVelocity) / 2;
                        velocity[0] = horizontalFinalVelocity;

                        float verticalFinalVelocity = velocity[1] + verticalAcceleration - verticalDrag;
                        position.Y += (velocity[1] + verticalFinalVelocity) / 2;
                        velocity[1] = verticalFinalVelocity;
                    }
                }
                else
                {
                    //Right
                    if (targetCenter1.Y < updatedPos.Y)
                    {
                        //Up
                        float horizontalAcceleration = (float)(Math.Sin(angle) * acceleration);
                        float verticalAcceleration = (float)(Math.Cos(angle) * acceleration);

                        float horizontalFinalVelocity = velocity[0] + horizontalAcceleration - horizontalDrag;
                        position.X += (velocity[0] + horizontalFinalVelocity) / 2;
                        velocity[0] = horizontalFinalVelocity;

                        float verticalFinalVelocity = velocity[1] - verticalAcceleration + verticalDrag;
                        position.Y += (velocity[1] + verticalFinalVelocity) / 2;
                        velocity[1] = verticalFinalVelocity;
                    }
                    else
                    {
                        //Down
                        float horizontalAcceleration = (float)(Math.Sin(angle) * acceleration);
                        float verticalAcceleration = (float)(Math.Cos(angle) * acceleration);

                        float horizontalFinalVelocity = velocity[0] + horizontalAcceleration - horizontalDrag;
                        position.X += (velocity[0] + horizontalFinalVelocity) / 2;
                        velocity[0] = horizontalFinalVelocity;

                        float verticalFinalVelocity = velocity[1] + verticalAcceleration - verticalDrag;
                        position.Y += (velocity[1] + verticalFinalVelocity) / 2;
                        velocity[1] = verticalFinalVelocity;
                    }
                }
            }

            if (targetCenter1.X < updatedPos.X)
            {
                //Left
                if (targetCenter1.Y < updatedPos.Y)
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
                if (targetCenter1.Y < updatedPos.Y)
                {
                    //Up
                }
                else
                {
                    //Down
                    angle = (float)(180 * (Math.PI / 180)) - angle;
                }
            }

            currentAngle = angle;
        }

        public void initializeCollisionPoints()
        {
            Vector2 updatedPos = new Vector2(position.X + Game1.offset.X, position.Y + Game1.offset.Y);
            float widthRatio = (float)size[0] / enemyTextures[imageID].Width;
            float heightRatio = (float)size[1] / enemyTextures[imageID].Height;

            for (int i = 0; i < enemyCollisionPoints[imageID].Count(); i++)
            {
                collisionPoint[i] = updatedPos;
                collisionPoint[i].X += (enemyCollisionPoints[imageID][i].X * widthRatio);
                collisionPoint[i].Y += (enemyCollisionPoints[imageID][i].Y * heightRatio);
            }

            for (int i = 0; i < collisionPoint.Count(); i++)
            {
                float xWithout = collisionPoint[i].X - updatedPos.X;
                float yWithout = collisionPoint[i].Y - updatedPos.Y;

                collisionPoint[i].X = (float)(updatedPos.X + ((xWithout * Math.Cos(currentAngle)) - (yWithout * Math.Sin(currentAngle))));
                collisionPoint[i].Y = (float)(updatedPos.Y + ((xWithout * Math.Sin(currentAngle)) + (yWithout * Math.Cos(currentAngle))));
            }
        }
    }
}
