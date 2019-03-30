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
    class Minimap
    {
        public static Vector2 position;
        public static int size;
        public static Texture2D blipTexture;
        public static Texture2D buildingBlipTexture;
        public static int buildingBlipSize;
        public static int blipSize = 10;
        public static List<Tile> destroyedBuildings = new List<Tile>();

        public static void drawMinimap(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.asteroidTexture, new Rectangle((int)position.X, (int)position.Y, size, size), Color.Wheat);
            spriteBatch.Draw(Tile.selectionOverlay, new Rectangle((int)position.X, (int)position.Y, size, size), Color.Black);
            float ratio = (float)size / Game1.asteroidSize;

            Vector2 minimapZeroZero = new Vector2(position.X + ((size - (Game1.Width * ratio)) / 2), position.Y + ((size - (Game1.Height * ratio)) / 2));
            foreach (Tile b in Tile.tiles)
            {
                if (b.built)
                {
                    if (b.building.defense)
                    {
                        spriteBatch.Draw(buildingBlipTexture, new Rectangle((int)(minimapZeroZero.X + (b.position.X * ratio)), (int)(minimapZeroZero.Y + (b.position.Y * ratio)), buildingBlipSize, buildingBlipSize), Color.Magenta);
                    }
                    else
                    {
                        spriteBatch.Draw(buildingBlipTexture, new Rectangle((int)(minimapZeroZero.X + (b.position.X * ratio)), (int)(minimapZeroZero.Y + (b.position.Y * ratio)), buildingBlipSize, buildingBlipSize), Color.Blue);
                    }
                }
            }

            if (Game1.offset.X * ratio > -(size / 2) && Game1.offset.X * ratio < (size / 2) && Game1.offset.Y * ratio > -(size / 2) && Game1.offset.Y * ratio < (size / 2))
            {
                Vector2 origin = position;
                origin.X += (size / 2);
                origin.Y += (size / 2);

                spriteBatch.Draw(blipTexture, new Rectangle((int)(origin.X - (Game1.offset.X * ratio)) - (blipSize / 2), (int)(origin.Y - (Game1.offset.Y * ratio)) - (blipSize / 2), blipSize, blipSize), Color.White);
            }

            Vector2 centerMap = new Vector2(Game1.Width / 2, Game1.Height / 2);
            foreach (Enemy b in Enemy.enemies)
            {
                if (Operation.distance(b.position, centerMap) <= (Game1.asteroidSize / 2))
                {
                    Vector2 origin = position;
                    origin.X += ((Game1.asteroidSize - Game1.Width) / 2) * ratio;
                    origin.Y += ((Game1.asteroidSize - Game1.Height) / 2) * ratio;

                    spriteBatch.Draw(blipTexture, new Rectangle((int)(origin.X + (b.position.X * ratio)) - (blipSize / 2), (int)(origin.Y + (b.position.Y * ratio)) - (blipSize / 2), blipSize, blipSize), Color.Red);
                }
            }
        }
    }
}
