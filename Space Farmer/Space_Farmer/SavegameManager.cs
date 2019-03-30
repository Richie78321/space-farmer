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
using System.IO;

namespace Space_Farmer
{
    class SavegameManager
    {

        public static void saveGame(int saveSlot)
        {
            //Asks if directory for saves exists
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Saves"))
            {
                //Does not exist, will create
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Saves");
            }

            StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "/Saves/save" + saveSlot.ToString() + ".txt");

            //Saves tiles
            for (int i = 0; i < Tile.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tile.tiles.GetLength(1); j++)
                {
                    //Asks if built
                    if (Tile.tiles[i, j].built)
                    {
                        //Writes built tiles to save file
                        writer.WriteLine("Tile," + i + "," + j + "," + Tile.tiles[i, j].building.name + "," + Tile.tiles[i, j].health);
                    }
                }
            }

            //Save turn number
            writer.WriteLine("Turn," + Game1.turnNumber);

            //Save gold (No need to save GPT, automatically added
            writer.WriteLine("Gold," + Player.gold);

            writer.Close();
        }

        public static void loadGame(int saveSlot)
        {
            //Checks that the directory exists
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Saves"))
            {
                return;
            }
            else
            {
                //Resets stats
                foreach (Tile b in Tile.tiles)
                {
                    if (b.built)
                    {
                        b.built = false;
                        b.building = null;
                    }
                }

                Player.goldPerTurn = 50;
                Enemy.enemies.Clear();
                Projectile.projectiles.Clear();
                Player.gold = 750;
                Game1.turnNumber = 0;
                for (int i = 0; i < Buffs.buffs.Count(); i++)
                {
                    Buffs.buffs[i] = 0;
                }
            }

            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/Saves/save" + saveSlot.ToString() + ".txt");

            while (!reader.EndOfStream)
            {
                string[] line = reader.ReadLine().Split(',');

                //Loads tiles
                if (line[0] == "Tile")
                {
                    foreach (BuildingType b in Building.buildingTypes)
                    {
                        if (b.name == line[3])
                        {
                            GUI.GUIButtons[2].visible = true;
                            Player.goldPerTurn += b.goldPerTurn;
                            Tile.tiles[int.Parse(line[1]), int.Parse(line[2])].built = true;
                            Tile.tiles[int.Parse(line[1]), int.Parse(line[2])].building = b;
                            Tile.tiles[int.Parse(line[1]), int.Parse(line[2])].health = int.Parse(line[4]);

                            for (int i = 0; i < b.modifier.Count(); i++)
                            {
                                if (b.modifier[i])
                                {
                                    Buffs.buffs[i] += b.percentIncrease[i];
                                }

                                //Clamps GoldCost
                                if (i == 2)
                                {
                                    Buffs.buffs[i] = Operation.clamp(Buffs.buffs[i], 0F, .9F);
                                }
                            }
                        }
                    }
                }

                //Loads turn number
                if (line[0] == "Turn")
                {
                    Game1.turnNumber = int.Parse(line[1]);
                }

                //Loads gold number
                if (line[0] == "Gold")
                {
                    Player.gold = float.Parse(line[1]);
                }
            }

            reader.Close();
        }
    }
}
