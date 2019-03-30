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
    class GUI
    {
        public static List<GUIButton> GUIButtons = new List<GUIButton>();
        public static List<BuildingButton> buildingButtons = new List<BuildingButton>();
        public static BuildingButton buildingButtonSelected = null;

        public static Texture2D pausedOverlay;
        public static Texture2D[] textures = new Texture2D[9];
        public static Texture2D[] buildingButtonTextures = new Texture2D[2];
        public static Texture2D goldCoin;

        public static int buttonSize = 100;
        public static int[] maxBuildButtonDimensions = { 300, 100 };
        public static int buildingButtonPadding = 10;

        public static bool buildMenuOpen = false;
        public static bool paused = false;
        public static bool repairMode = false;

        public static void drawButtons(SpriteBatch spriteBatch)
        {
            foreach (GUIButton b in GUIButtons)
            {
                if (b.visible)
                {
                    spriteBatch.Draw(textures[b.imageID], b.buttonRectangle, Color.White);
                }
            }

            foreach (BuildingButton b in buildingButtons)
            {
                if (b.visible)
                {
                    if (b.selected)
                    {
                        spriteBatch.Draw(buildingButtonTextures[1], b.buttonRectangle, Color.White);
                        spriteBatch.Draw(b.building.texture, new Rectangle(b.buttonRectangle.X + (int)(b.buttonRectangle.Width * (2F/3)), b.buttonRectangle.Y, b.buttonRectangle.Height, b.buttonRectangle.Height), Color.White);
                        b.drawInfo(spriteBatch);

                    }
                    else
                    {
                        spriteBatch.Draw(buildingButtonTextures[0], b.buttonRectangle, Color.White);
                        spriteBatch.Draw(b.building.texture, new Rectangle(b.buttonRectangle.X + (int)(b.buttonRectangle.Width * (2F / 3)), b.buttonRectangle.Y, b.buttonRectangle.Height, b.buttonRectangle.Height), Color.White);
                        b.drawInfo(spriteBatch);
                    }
                }
            }
        }

        public static void initializeBuildingButtons()
        {
            int overflow = 0;
            for (int i = 0; i < Building.buildingTypes.Count(); i++)
            {
                if (i >= 8 + (overflow * 8))
                {
                    overflow++;
                }

                buildingButtons.Add(new BuildingButton(false, new Rectangle(Game1.Width - (buildingButtonPadding * (overflow + 1)) - (maxBuildButtonDimensions[0] * (overflow + 1)), ((i + 1 - (overflow * 8)) * buildingButtonPadding) + (maxBuildButtonDimensions[1] * (i - (overflow * 8))), maxBuildButtonDimensions[0], maxBuildButtonDimensions[1]), Building.buildingTypes[i]));
            }
        }

        public static bool checkButtonClick(MouseState state)
        {
            foreach (GUIButton b in GUIButtons)
            {
                if (b.visible)
                {
                    if (!repairMode || b.imageID == 8)
                    {
                        if (b.buttonRectangle.Contains(state.X, state.Y))
                        {
                            if (state.LeftButton == ButtonState.Pressed && !b.pressed)
                            {
                                //To reduce mouse flicker
                                b.pressed = true;
                                return true;
                            }
                            else if (state.LeftButton == ButtonState.Released && b.pressed)
                            {
                                b.pressed = false;
                                b.triggered = true;
                                return true;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            b.pressed = false;
                        }
                    }
                }
            }

            return false;
        }

        public static bool checkBuildingButtonClick(MouseState state)
        {
            foreach (BuildingButton b in buildingButtons)
            {
                if (b.visible)
                {
                    if (b.buttonRectangle.Contains(state.X, state.Y))
                    {
                        if (state.LeftButton == ButtonState.Pressed && !b.pressed)
                        {
                            //To reduce mouse flicker
                            b.pressed = true;
                            b.selected = !b.selected;
                            if (b.selected)
                            {
                                buildingButtonSelected = b;
                            }
                            else
                            {
                                buildingButtonSelected = null;
                            }
                            foreach (BuildingButton i in buildingButtons)
                            {
                                if (i != b)
                                {
                                    i.selected = false;
                                }
                            }
                            return true;
                        }
                        else if (state.LeftButton == ButtonState.Released && b.pressed)
                        {
                            b.pressed = false;
                            b.triggered = true;
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        b.pressed = false;
                    }
                }
            }

            return false;
        }

        public static void drawGold(SpriteBatch spriteBatch, SpriteFont displayFont)
        {
            int goldSymbolSize = 18;
            spriteBatch.Draw(goldCoin, new Rectangle(5, 11, goldSymbolSize, goldSymbolSize), Color.White);

            if (Player.goldPerTurn < 0)
            {
                spriteBatch.DrawString(displayFont, Math.Round(Player.gold).ToString() + " " + Player.goldPerTurn + " | Turn " + Game1.turnNumber, new Vector2(goldSymbolSize + 7, 0), Color.White);
            }
            else
            {
                spriteBatch.DrawString(displayFont, Math.Round(Player.gold).ToString() + " +" + (Player.goldPerTurn + (Player.goldPerTurn * Buffs.buffs[0])) + " | Turn " + Game1.turnNumber, new Vector2(goldSymbolSize + 7, 0), Color.White);
            }
        }

        public static void updateButtons()
        {
            if (GUIButtons[0].triggered)
            {
                //Buildings Menu Button
                //Resets button
                GUIButtons[0].triggered = false;
                //Makes button invisible
                GUIButtons[0].visible = false;
                //Build menu open bool updated
                buildMenuOpen = true;
                //Makes exit button visible
                GUIButtons[1].visible = true;
                //Makes pause button invisible
                GUIButtons[3].visible = false;
                //Makes repair button invisible
                GUIButtons[8].visible = false;
                //Makes all building buttons visible
                foreach (BuildingButton b in buildingButtons)
                {
                    b.visible = true;
                }
            }

            if (GUIButtons[1].triggered)
            {
                //Buildings Menu EXIT Button
                //Resets button
                GUIButtons[1].triggered = false;
                //Makes button invisible
                GUIButtons[1].visible = false;
                //Build menu open bool updated
                buildMenuOpen = false;
                //Makes menu button visible
                GUIButtons[0].visible = true;
                //Makes pause button visible
                GUIButtons[3].visible = true;
                //Makes repair button visible
                GUIButtons[8].visible = true;
                //Makes all building buttons invisible
                foreach (BuildingButton b in buildingButtons)
                {
                    b.visible = false;
                }
            }

            if (GUIButtons[2].triggered)
            {
                //Next turn button
                Game1.nextTurn();
                GUIButtons[2].triggered = false;
            }

            if (GUIButtons[3].triggered)
            {
                //Makes paused
                paused = true;

                //Makes all menu buttons visible and makes all other buttons invisible
                GUIButtons[0].visible = false;
                GUIButtons[2].visible = false;
                GUIButtons[3].visible = false;
                GUIButtons[4].visible = true;
                GUIButtons[5].visible = true;
                GUIButtons[6].visible = true;
                GUIButtons[7].visible = true;

                GUIButtons[3].triggered = false;
            }

            if (GUIButtons[5].triggered)
            {
                //Plays load function
                SavegameManager.loadGame(0);

                GUIButtons[4].triggered = true;
                GUIButtons[5].triggered = false;
            }

            if (GUIButtons[6].triggered)
            {
                //Plays load function
                SavegameManager.saveGame(0);

                GUIButtons[4].triggered = true;
                GUIButtons[6].triggered = false;
            }

            if (GUIButtons[4].triggered)
            {
                //Unpauses
                paused = false;

                //Makes all menu buttons invisible and makes all other buttons visible
                GUIButtons[0].visible = true;
                GUIButtons[3].visible = true;
                GUIButtons[4].visible = false;
                GUIButtons[5].visible = false;
                GUIButtons[6].visible = false;
                GUIButtons[7].visible = false;
                foreach (Tile b in Tile.tiles)
                {
                    if (b.built)
                    {
                        GUIButtons[2].visible = true;
                    }
                }

                GUIButtons[4].triggered = false;
            }

            if (GUIButtons[8].triggered)
            {
                //Presses or unpresses
                GUIButtons[8].toggled = !GUIButtons[8].toggled;

                repairMode = GUIButtons[8].toggled;

                GUIButtons[8].triggered = false;
            }
        }
    }

    class GUIButton
    {
        public Rectangle buttonRectangle;
        public bool visible = false;
        public bool triggered = false;
        public bool pressed = false;
        public int imageID;
        public bool toggled = false;

        public GUIButton(bool visibleInput, Rectangle rectangleInput, int imageIDInput)
        {
            visible = visibleInput;
            buttonRectangle = rectangleInput;
            imageID = imageIDInput;
        }
    }

    class BuildingButton
    {
        public Rectangle buttonRectangle;
        public BuildingType building;
        public bool visible = false;
        public bool triggered = false;
        public bool selected = false;
        public bool pressed = false;

        public BuildingButton(bool visibleInput, Rectangle rectangleInput, BuildingType buildingTypeInput)
        {
            visible = visibleInput;
            buttonRectangle = rectangleInput;
            building = buildingTypeInput;
        }

        public void drawInfo(SpriteBatch spriteBatch)
        {
            if (building.buff)
            {
                if (building.modifier[0])
                {
                    spriteBatch.DrawString(Game1.buildingButtonFont, " " + building.name + "\n " + Math.Round(building.goldCost - (building.goldCost * Buffs.buffs[2])) + "G " + building.goldPerTurn + "GPT \n +" + (building.percentIncrease[0] * 100) + "% GPT", new Vector2(buttonRectangle.X, buttonRectangle.Y), Color.White, 0F, new Vector2(0, 0), Game1.screenSizeRatio, SpriteEffects.None, 0F);
                }
                else if (building.modifier[1])
                {
                    spriteBatch.DrawString(Game1.buildingButtonFont, " " + building.name + "\n " + Math.Round(building.goldCost - (building.goldCost * Buffs.buffs[2])) + "G " + building.goldPerTurn + "GPT \n +" + (building.percentIncrease[1] * 100) + "% Damage", new Vector2(buttonRectangle.X, buttonRectangle.Y), Color.White, 0F, new Vector2(0, 0), Game1.screenSizeRatio, SpriteEffects.None, 0F);
                }
                else if (building.modifier[2])
                {
                    spriteBatch.DrawString(Game1.buildingButtonFont, " " + building.name + "\n " + Math.Round(building.goldCost - (building.goldCost * Buffs.buffs[2])) + "G " + building.goldPerTurn + "GPT \n -" + (building.percentIncrease[2] * 100) + "% GoldCost", new Vector2(buttonRectangle.X, buttonRectangle.Y), Color.White, 0F, new Vector2(0, 0), Game1.screenSizeRatio, SpriteEffects.None, 0F);
                }
                else if (building.modifier[3])
                {
                    spriteBatch.DrawString(Game1.buildingButtonFont, " " + building.name + "\n " + Math.Round(building.goldCost - (building.goldCost * Buffs.buffs[2])) + "G " + building.goldPerTurn + "GPT \n +" + (building.percentIncrease[3] * 100) + "% EnemyKR", new Vector2(buttonRectangle.X, buttonRectangle.Y), Color.White, 0F, new Vector2(0, 0), Game1.screenSizeRatio, SpriteEffects.None, 0F);
                }
            }
            else
            {
                spriteBatch.DrawString(Game1.buildingButtonFont, " " + building.name + "\n " + Math.Round(building.goldCost - (building.goldCost * Buffs.buffs[2])) + " Gold \n " + building.goldPerTurn + " GPT", new Vector2(buttonRectangle.X, buttonRectangle.Y), Color.White, 0F, new Vector2(0, 0), Game1.screenSizeRatio, SpriteEffects.None, 0F);
            }
        }
    }
}
