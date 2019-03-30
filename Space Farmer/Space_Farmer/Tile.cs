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
    class Tile
    {
        public static int gridSize = 1200;
        public static int gridPadding = 10;
        public static int gridNumber = 20;
        public static Tile[,] tiles;

        public static Texture2D selectionOverlay;
        public static Texture2D healthRectangle;

        public static void updateDefenseTiles(GameTime gameTime)
        {
            foreach (Tile b in tiles)
            {
                if (b.built)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds - b.lastShotTime > b.building.shotTime || b.lastShotTime == -1)
                    {
                        b.lastShotTime = (long)gameTime.TotalGameTime.TotalMilliseconds;
                        b.shoot();
                    }
                }
            }
        }
               
        public static void drawTiles(SpriteBatch spriteBatch)
        {
            foreach (Tile b in Tile.tiles)
            {
                if (b.built)
                {
                    spriteBatch.Draw(b.building.texture, b.tileRectangle, Color.White);
                    
                    if (b.health != b.building.health)
                    {
                        spriteBatch.Draw(healthRectangle, new Rectangle(b.tileRectangle.X + (b.tileRectangle.Width / 2) - (b.health / 2), b.tileRectangle.Y + ((b.tileRectangle.Height / 4) * 3), b.health, 10), Color.White);
                    }
                }
            }
        }

        public static void setTileOverlay(MouseState state, SpriteBatch spriteBatch)
        {
            if (GUI.buildMenuOpen)
            {
                foreach (Tile b in tiles)
                {
                    if (b.tileRectangle.Contains(state.X, state.Y))
                    {
                        spriteBatch.Draw(selectionOverlay, b.tileRectangle, Color.White);
                    }
                }
            }
        }

        public static void selectGrid(MouseState state)
        {
            if (state.LeftButton == ButtonState.Pressed)
            {
                if (GUI.buildMenuOpen)
                {
                    if (GUI.buildingButtonSelected != null)
                    {
                        //Building tile
                        foreach (Tile b in Tile.tiles)
                        {
                            if (b.tileRectangle.Contains(state.X, state.Y))
                            {
                                if (!b.built)
                                {
                                    if (Player.gold >= GUI.buildingButtonSelected.building.goldCost - (GUI.buildingButtonSelected.building.goldCost * Buffs.buffs[2]))
                                    {
                                        Player.gold -= GUI.buildingButtonSelected.building.goldCost - (GUI.buildingButtonSelected.building.goldCost * Buffs.buffs[2]);
                                        Player.goldPerTurn += GUI.buildingButtonSelected.building.goldPerTurn;
                                        b.built = true;
                                        //Temp Test
                                        b.building = GUI.buildingButtonSelected.building;
                                        b.health = GUI.buildingButtonSelected.building.health;
                                        GUI.GUIButtons[2].visible = true;

                                        //Checks if black hole
                                        if (GUI.buildingButtonSelected.building.name == "Black Hole")
                                        {
                                            SavegameManager.saveGame(0);
                                            Environment.Exit(0);
                                        }

                                        if (GUI.buildingButtonSelected.building.buff)
                                        {
                                            for (int i = 0; i < GUI.buildingButtonSelected.building.modifier.Count(); i++)
                                            {
                                                if (GUI.buildingButtonSelected.building.modifier[i])
                                                {
                                                    Buffs.buffs[i] += GUI.buildingButtonSelected.building.percentIncrease[i];
                                                }

                                                //Clamps GoldCost
                                                if (i == 2)
                                                {
                                                    Buffs.buffs[i] = Operation.clamp(Buffs.buffs[i], 0F, .9F);
                                                }
                                            }
                                        }

                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Check if repair
                    if (GUI.repairMode)
                    {
                        foreach (Tile b in tiles)
                        {
                            if (b.tileRectangle.Contains(state.X, state.Y))
                            {
                                if (b.built)
                                {
                                    int goldCostAmount = (b.building.health - b.health) * Game1.repairCostMultiplier;
                                    if (Player.gold >= goldCostAmount)
                                    {
                                        //Repair
                                        Player.gold -= goldCostAmount;
                                        b.health = b.building.health;
                                    }

                                    return;
                                }
                            }
                        } 
                    }
                }
            }
        }

        public static void initializeGrid(int originalWidth)
        {
            tiles = new Tile[gridNumber, gridNumber];

            float ratio = Game1.asteroidSize / originalWidth;
            int size = (int)((gridSize * ratio) - (gridPadding * gridNumber)) / gridNumber;
            Minimap.buildingBlipSize = (int)(size * ((float)Minimap.size / Game1.asteroidSize));
            Vector2 origin = new Vector2((Game1.Width / 2) - (size * (gridNumber / 2)) - (gridPadding * (gridNumber / 2)), (Game1.Height / 2) - (size * (gridNumber / 2)) - (gridPadding * (gridNumber / 2)));

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    Vector2 position = origin;
                    position.X += (size * i) + (gridPadding * i);
                    position.Y += (size * j) + (gridPadding * j);

                    tiles[i, j] = new Tile(position, size);
                }
            }
        }

        public static void updateTileRectangles()
        {
            foreach (Tile b in tiles)
            {
                b.updateRectangle();
            }
        }

        //Object
        public Vector2 position;
        public Rectangle tileRectangle;
        public bool built = false;
        public BuildingType building;
        public int health;
        public long lastShotTime = -1;

        public Tile(Vector2 positionInput, int size)
        {
            position = positionInput;
            tileRectangle = new Rectangle((int)positionInput.X, (int)positionInput.Y, size, size);
        }

        public void updateRectangle()
        {
            tileRectangle.X = (int)(position.X + Game1.offset.X);
            tileRectangle.Y = (int)(position.Y + Game1.offset.Y);
        }

        public void breakBuilding()
        {
            if (building.buff)
            {
                for (int i = 0; i < building.modifier.Count(); i++)
                {
                    if (building.modifier[i])
                    {
                        Buffs.buffs[i] -= building.percentIncrease[i];
                        
                        //Clamps GoldCost
                        if (i == 2)
                        {
                            Buffs.buffs[i] = Operation.clamp(Buffs.buffs[i], 0F, .9F);
                        }
                    }
                }
            }

            Player.goldPerTurn += -building.goldPerTurn;
            built = false;
            building = null;

            Enemy.enemyExplosion.Play(1F, -.5F, 0);

            Rectangle trueTileRec = new Rectangle(tileRectangle.X - (int)Game1.offset.X, tileRectangle.Y - (int)Game1.offset.Y, tileRectangle.Width, tileRectangle.Height);
            ActiveKeyframeEffect.activeKeyframeEffects.Add(new ActiveKeyframeEffect(KeyframeEffect.keyframeEffects[0], trueTileRec, Color.White));
        }

        public void shoot()
        {
            Vector2 tileCenter1 = new Vector2(tileRectangle.X + (tileRectangle.Width / 2), tileRectangle.Y + (tileRectangle.Height / 2));
            Enemy target = null;
            int minDist = -1;
            foreach (Enemy b in Enemy.enemies)
            {
                Vector2 updatedPos = new Vector2(b.position.X + Game1.offset.X, b.position.Y + Game1.offset.Y);

                int dist = (int)Operation.distance(updatedPos, tileCenter1);
                if (dist <= building.radius)
                {
                    if (dist <= minDist || minDist == -1)
                    {
                        target = b;
                    }
                }
            }
       

            //Asks if no enemies to target
            if (target == null)
            {
                return;
            }

            Vector2 updatedPos1 = new Vector2(target.position.X + Game1.offset.X, target.position.Y + Game1.offset.Y);
            float xDist = Math.Abs(updatedPos1.X - tileCenter1.X);
            float yDist = Math.Abs(updatedPos1.Y - tileCenter1.Y);
            float angle = (float)Math.Atan(xDist / yDist);

            //Sets complete angle
            if (updatedPos1.X < tileCenter1.X)
            {
                //Left
                if (updatedPos1.Y < tileCenter1.Y)
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
                if (updatedPos1.Y < tileCenter1.Y)
                {
                    //Up
                }
                else
                {
                    //Down
                    angle = (float)(180 * (Math.PI / 180)) - angle;
                }
            }


            Projectile.projectiles.Add(new Projectile(new Vector2(tileCenter1.X - Game1.offset.X, tileCenter1.Y - Game1.offset.Y), angle, Player.laserSpeed, building.damage, true, building.bulletColor, this, false));
        }
    }

    class Buffs
    {
        //GPT, Damage, GoldCost, EnemyDeathReward
        public static float[] buffs = { 0, 0, 0, 0};
    }
}
