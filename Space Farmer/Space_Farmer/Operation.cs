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
    class Operation
    {
        public static float clamp(float value, float min, float max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        public static float distance(Vector2 point1, Vector2 point2)
        {
            float xDist = Math.Abs(point1.X - point2.X);
            float yDist = Math.Abs(point1.Y - point2.Y);

            return (float)Math.Sqrt((xDist * xDist) + (yDist * yDist));
        }
    }
}
