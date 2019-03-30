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
    class Player
    {
        //Game stat variables
        public static float gold = 750;
        public static float goldPerTurn = 50;

        //Center of Original Rocket: 219,538
        public static float laserDamage = 50;
        public static int imageID = 0;
        public static float sizeRatioToOriginal;
        public static Texture2D[] shipTexture = new Texture2D[4];
        public static SoundEffect thrusterSound;
        public static float laserSpeed = 50;
        public static float[] velocity = new float[2];
        public static float rotationVelocity = 0;
        public static float rotationDrag = .005F;
        public static float rotation = 0;
        public static float accelerationSpeed = .2F;
        public static float drag = .08F;
        public static float rotationSpeed = .05F;
        public static int[] size = new int[2];
        public static ulong timeOfLastShot = 0;
        public static float timeBetweenShots = 250;
        public static float maxSpeed = 10;
        public static SoundEffectInstance thrusterSoundInstance;
        public static float flameAnimationSpeed = 100;
        public static Vector2[] collisionPoint = new Vector2[5];
        public static bool dead = false;

        private static bool accelerate = false;
        private static ulong timeOfLastAccelerate = 0;
        public static void controller(KeyboardState state, GameTime gameTime)
        {
            if (state.IsKeyDown(Keys.A))
            {
                //rotationVelocity -= rotationSpeed;
                //if (rotationVelocity < -rotationSpeed)
                //{
                //    rotationVelocity = -rotationSpeed;
                //}
                rotation -= rotationSpeed;
            }
            else if (state.IsKeyDown(Keys.D))
            {
                //rotationVelocity += rotationSpeed;
                //if (rotationVelocity > rotationSpeed)
                //{
                //    rotationVelocity = rotationSpeed;
                //}
                rotation += rotationSpeed;
            }
            //else
            //{
            //    if (rotationVelocity < 0)
            //    {
            //        rotationVelocity += rotationDrag;
            //        if (rotationVelocity > 0)
            //        {
            //            rotationVelocity = 0;
            //        }
            //    }
            //    else if (rotationVelocity > 0)
            //    {
            //        rotationVelocity -= rotationDrag;
            //        if (rotationVelocity < 0)
            //        {
            //            rotationVelocity = 0;
            //        }
            //    }
            //}
            //rotation += rotationVelocity;
            if (rotation > 360 * (Math.PI / 180))
            {
                rotation = (float)(rotation - (360 * (Math.PI / 180)));
            }
            else if (rotation < -360 * (Math.PI / 180))
            {
                rotation = (float)(rotation + (360 * (Math.PI / 180)));
            }
            //End of rotation

            if (state.IsKeyDown(Keys.Space))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - timeOfLastShot > timeBetweenShots || timeOfLastShot == 0)
                {
                    timeOfLastShot = (ulong)gameTime.TotalGameTime.TotalMilliseconds;
                    shootProjectile();
                }
            }
            if (state.IsKeyDown(Keys.W))
            {
                if (!accelerate)
                {
                    timeOfLastAccelerate = (ulong)gameTime.TotalGameTime.TotalMilliseconds;
                    imageID = 1;
                    accelerate = true;
                }

                if (gameTime.TotalGameTime.TotalMilliseconds - timeOfLastAccelerate >= flameAnimationSpeed)
                {
                    timeOfLastAccelerate = (ulong)gameTime.TotalGameTime.TotalMilliseconds;
                    imageID++;
                    if (imageID >= 4)
                    {
                        imageID = 1;
                    }
                }

                thrusterSoundInstance.Play();
                velocity[0] += (float)(Math.Sin(rotation) * accelerationSpeed);
                velocity[1] -= (float)(Math.Cos(rotation) * accelerationSpeed);
            }
            else
            {
                accelerate = false;
                timeOfLastAccelerate = 0;
                imageID = 0;
                thrusterSoundInstance.Pause();
            }                 
        }

        private static void shootProjectile()
        {
            Vector2 center = new Vector2((Game1.Width / 2) - Game1.offset.X, (Game1.Height / 2) - Game1.offset.Y);
            Vector2 origin = new Vector2(center.X, center.Y - ((538 * sizeRatioToOriginal) / 2));
            float xWithout = origin.X - center.X;
            float yWithout = origin.Y - center.Y;
            origin.X = (float)(center.X + ((xWithout * Math.Cos(rotation)) - (yWithout * Math.Sin(rotation))));
            origin.Y = (float)(center.Y + ((xWithout * Math.Sin(rotation)) + (yWithout * Math.Cos(rotation))));

            Projectile.projectiles.Add(new Projectile(origin, rotation, laserSpeed, laserDamage, true, Color.Blue, null, false));
        }

        public static void movement()
        {
            //Drag
            if (velocity[0] != 0 && velocity[1] != 0)
            {
                float angle = (float)Math.Atan(velocity[0] / velocity[1]);

                if (velocity[1] < 0)
                {
                    velocity[0] += (float)(Math.Sin(angle) * drag);
                    velocity[1] += (float)(Math.Cos(angle) * drag);
                }
                else
                {
                    velocity[0] -= (float)(Math.Sin(angle) * drag);
                    velocity[1] -= (float)(Math.Cos(angle) * drag);
                }

                if (Math.Abs(velocity[0]) <= drag)
                {
                    velocity[0] = 0;
                }
                if (Math.Abs(velocity[1]) <= drag)
                {
                    velocity[1] = 0;
                }
            }
            else if (velocity[0] != 0)
            {
                if (velocity[0] < 0)
                {
                    velocity[0] += drag;
                    if (velocity[0] > 0)
                    {
                        velocity[0] = 0;
                    }
                }
                else if (velocity[0] > 0)
                {
                    velocity[0] -= drag;
                    if (velocity[0] < 0)
                    {
                        velocity[0] = 0;
                    }
                }
            }
            else if (velocity[1] != 0)
            {
                if (velocity[1] < 0)
                {
                    velocity[1] += drag;
                    if (velocity[1] > 0)
                    {
                        velocity[1] = 0;
                    }
                }
                else if (velocity[1] > 0)
                {
                    velocity[1] -= drag;
                    if (velocity[1] < 0)
                    {
                        velocity[1] = 0;
                    }
                }
            }

            //Speed Limit
            for (int i = 0; i < velocity.Count(); i++)
            {
                velocity[i] = Operation.clamp(velocity[i], -maxSpeed, maxSpeed);
            }

            Game1.offset.X -= velocity[0];
            Game1.offset.Y -= velocity[1];

            //Boundaries
            float xClamp = Operation.clamp(Game1.offset.X, -Game1.backgroundPadding * (Game1.backgroundPadding / 10), Game1.backgroundPadding * (Game1.backgroundPadding / 10));
            float yClamp = Operation.clamp(Game1.offset.Y, -Game1.backgroundPadding * (Game1.backgroundPadding / 10), Game1.backgroundPadding * (Game1.backgroundPadding / 10));
            
            if (xClamp != Game1.offset.X)
            {
                Game1.offset.X = xClamp;
                velocity[0] = 0;
            }
            if (yClamp != Game1.offset.Y)
            {
                Game1.offset.Y = yClamp;
                velocity[1] = 0;
            }
        }

        public static void mouseController(MouseState state)
        {
            if (state.LeftButton == ButtonState.Pressed || state.LeftButton == ButtonState.Released)
            {
                //Checks for GUI in range (First priority)
                if (GUI.checkButtonClick(state)) return;

                //Checks for building button in range
                if (GUI.checkBuildingButtonClick(state)) return;

                //Checks for tile in range
                Tile.selectGrid(state);

            }
        }

        public static void initializeCollisionPoints()
        {
            Vector2 position = new Vector2(Game1.Width / 2, Game1.Height / 2);

            collisionPoint[0].X = position.X;
            collisionPoint[0].Y = position.Y - (269 * sizeRatioToOriginal);

            collisionPoint[1].X = position.X + (110 * sizeRatioToOriginal);
            collisionPoint[1].Y = position.Y + (150 * sizeRatioToOriginal);

            collisionPoint[2].X = position.X;
            collisionPoint[2].Y = position.Y + (269 * sizeRatioToOriginal);

            collisionPoint[3].X = position.X - (110 * sizeRatioToOriginal);
            collisionPoint[3].Y = position.Y + (150 * sizeRatioToOriginal);

            collisionPoint[4].X = position.X;
            collisionPoint[4].Y = position.Y - (269 * sizeRatioToOriginal);

            for (int i = 0; i < collisionPoint.Count(); i++)
            {
                float xWithout = collisionPoint[i].X - position.X;
                float yWithout = collisionPoint[i].Y - position.Y;

                collisionPoint[i].X = (float)(position.X + ((xWithout * Math.Cos(rotation)) - (yWithout * Math.Sin(rotation))));
                collisionPoint[i].Y = (float)(position.Y + ((xWithout * Math.Sin(rotation)) + (yWithout * Math.Cos(rotation))));
            }
        }

    }
}
