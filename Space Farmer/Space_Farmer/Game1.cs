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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Player Offset
        public static Vector2 offset;
        public static int Width, Height;

        //Screen
        public static Rectangle screenBounds;

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Common Fonts
        SpriteFont displayFont;
        public static SpriteFont buildingButtonFont;

        //Common Textures
        public static Texture2D asteroidTexture;
        Texture2D galaxyBackground;
        Texture2D galaxyStars;
        Texture2D galaxyDebris;
        Texture2D repairCursor;

        //Turn Data
        public static int turnNumber = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Initialize Screen Preferences
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        //Non-Graphical
        public static float screenSizeRatio;
        public static int screenBoundPadding = 1000;
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here   
            IsMouseVisible = true;
            offset = new Vector2(0, 0);
            Width = GraphicsDevice.Viewport.Width;
            Height = GraphicsDevice.Viewport.Height;
            screenBounds = new Rectangle(-screenBoundPadding, -screenBoundPadding, GraphicsDevice.Viewport.Width + (screenBoundPadding * 2), GraphicsDevice.Viewport.Height + (screenBoundPadding * 2));

            //Creates building button size
            screenSizeRatio = ((float)GraphicsDevice.Viewport.Height / 1080);
            GUI.maxBuildButtonDimensions[0] = (int)(GUI.maxBuildButtonDimensions[0] * screenSizeRatio);
            GUI.maxBuildButtonDimensions[1] = (int)(GUI.maxBuildButtonDimensions[1] * screenSizeRatio);

            int buttonPadding = 10;
            //Adds Minimap
            int minimapSizes = 200;
            Minimap.position = new Vector2(10, GraphicsDevice.Viewport.Height - minimapSizes);
            Minimap.size = minimapSizes;

            int[] pauseMenuButtonSize = { 200, 100 };
            //Adds GUI Buttons
            GUI.GUIButtons.Add(new GUIButton(true, new Rectangle(GraphicsDevice.Viewport.Width - GUI.buttonSize - buttonPadding, GraphicsDevice.Viewport.Height - GUI.buttonSize - buttonPadding, GUI.buttonSize, GUI.buttonSize), 0));
            GUI.GUIButtons.Add(new GUIButton(false, new Rectangle(GraphicsDevice.Viewport.Width - GUI.buttonSize - buttonPadding, GraphicsDevice.Viewport.Height - GUI.buttonSize - buttonPadding, GUI.buttonSize, GUI.buttonSize), 1));
            GUI.GUIButtons.Add(new GUIButton(false, new Rectangle(buttonPadding, GraphicsDevice.Viewport.Height - Minimap.size - buttonPadding - (Minimap.size / 4), Minimap.size, Minimap.size / 4), 2));
            GUI.GUIButtons.Add(new GUIButton(true, new Rectangle(GraphicsDevice.Viewport.Width - GUI.buttonSize - buttonPadding, buttonPadding, GUI.buttonSize, GUI.buttonSize), 3));
            GUI.GUIButtons.Add(new GUIButton(false, new Rectangle((GraphicsDevice.Viewport.Width - pauseMenuButtonSize[0]) / 2, ((GraphicsDevice.Viewport.Height - (pauseMenuButtonSize[1] * 4) - (buttonPadding * 3)) / 2), pauseMenuButtonSize[0], pauseMenuButtonSize[1]), 7));
            GUI.GUIButtons.Add(new GUIButton(false, new Rectangle((GraphicsDevice.Viewport.Width - pauseMenuButtonSize[0]) / 2, ((GraphicsDevice.Viewport.Height - (pauseMenuButtonSize[1] * 4) - (buttonPadding * 3)) / 2) + (pauseMenuButtonSize[1] * 1) + (buttonPadding * 1), pauseMenuButtonSize[0], pauseMenuButtonSize[1]), 5));
            GUI.GUIButtons.Add(new GUIButton(false, new Rectangle((GraphicsDevice.Viewport.Width - pauseMenuButtonSize[0]) / 2, ((GraphicsDevice.Viewport.Height - (pauseMenuButtonSize[1] * 4) - (buttonPadding * 3)) / 2) + (pauseMenuButtonSize[1] * 2) + (buttonPadding * 2), pauseMenuButtonSize[0], pauseMenuButtonSize[1]), 4));
            GUI.GUIButtons.Add(new GUIButton(false, new Rectangle((GraphicsDevice.Viewport.Width - pauseMenuButtonSize[0]) / 2, ((GraphicsDevice.Viewport.Height - (pauseMenuButtonSize[1] * 4) - (buttonPadding * 3)) / 2) + (pauseMenuButtonSize[1] * 3) + (buttonPadding * 3), pauseMenuButtonSize[0], pauseMenuButtonSize[1]), 6));
            GUI.GUIButtons.Add(new GUIButton(true, new Rectangle(GraphicsDevice.Viewport.Width - ((GUI.buttonSize + buttonPadding) * 2), buttonPadding, GUI.buttonSize, GUI.buttonSize), 8));

            base.Initialize();
        }

        public static int playerInitSize = 200;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Load Player Textures
            Player.shipTexture[0] = Content.Load<Texture2D>("Player/playerShip0");
            Player.shipTexture[1] = Content.Load<Texture2D>("Player/playerShip1");
            Player.shipTexture[2] = Content.Load<Texture2D>("Player/playerShip2");
            Player.shipTexture[3] = Content.Load<Texture2D>("Player/playerShip3");
            //Load Player Size
            float ratio = (float)Player.shipTexture[0].Width / Player.shipTexture[0].Height;
            Player.size[1] = playerInitSize;
            Player.size[0] = (int)(Player.size[1] * ratio);
            Player.sizeRatioToOriginal = (float)playerInitSize / Player.shipTexture[0].Height;

            //TEST ENEMY
            //Enemy.enemies.Add(new Enemy(new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), .2F, 2, 350, (int)(playerInitSize * ratio), playerInitSize, 1000));
            //Enemy.enemies.Add(new Enemy(new Vector2(20, 0), .2F, 2, 400, (int)(playerInitSize * ratio), playerInitSize, 1000));

            GUI.goldCoin = Content.Load<Texture2D>("UI Textures/goldCoin");
            galaxyDebris = Content.Load<Texture2D>("General Textures/galaxyDebris");
            asteroidTexture = Content.Load<Texture2D>("General Textures/asteroidTexture");
            galaxyBackground = Content.Load<Texture2D>("General Textures/galaxyBackground");
            galaxyStars = Content.Load<Texture2D>("General Textures/galaxyStars");
            repairCursor = Content.Load<Texture2D>("General Textures/repairCursor");
            Projectile.laserBeam = Content.Load<Texture2D>("General Textures/laserBeam");
            Projectile.rocketTexture = Content.Load<Texture2D>("General Textures/missileProjectile");
            Projectile.laserSound[0] = Content.Load<SoundEffect>("General Sounds/Laser/laserShot0");
            Projectile.laserSound[1] = Content.Load<SoundEffect>("General Sounds/Laser/laserShot1");
            Player.thrusterSound = Content.Load<SoundEffect>("General Sounds/thrusterSound");
            Player.thrusterSoundInstance = Player.thrusterSound.CreateInstance();
            Player.thrusterSoundInstance.IsLooped = true;

            //Load keyframeEffects
            KeyframeEffect.keyframeEffects.Add(new KeyframeEffect(89, "Keyframe Effects/Building Explosion/explosion"));
            KeyframeEffect.keyframeEffects.Add(new KeyframeEffect(21, "Keyframe Effects/Laser Spark/explosion"));
            KeyframeEffect.loadKeyframes(Content);

            //Load Enemy Sounds
            Enemy.enemyExplosion = Content.Load<SoundEffect>("General Sounds/Enemy/enemyExplosion");

            //Load Minimap textures
            Minimap.blipTexture = Content.Load<Texture2D>("Minimap Textures/minimapBlip");
            Minimap.buildingBlipTexture = Content.Load<Texture2D>("Minimap Textures/minimapBuilding");

            //Load Enemy Textures and Collision Points
            Enemy.enemyTextures[0] = Content.Load<Texture2D>("Enemy Textures/enemyShip");
            Enemy.enemyTextures[1] = Content.Load<Texture2D>("Enemy Textures/roundEnemy");
            Enemy.enemyTextures[2] = Content.Load<Texture2D>("Enemy Textures/bossEnemy");
            Enemy.setCollisionPoints();

            //Load UI textures
            GUI.buildingButtonTextures[0] = Content.Load<Texture2D>("UI Textures/buildingButton");
            GUI.buildingButtonTextures[1] = Content.Load<Texture2D>("UI Textures/buildingButtonSelected");
            GUI.textures[0] = Content.Load<Texture2D>("UI Textures/buildButton");
            GUI.textures[1] = Content.Load<Texture2D>("UI Textures/exitBuildButton");
            GUI.textures[2] = Content.Load<Texture2D>("UI Textures/nextTurnButton");
            GUI.textures[3] = Content.Load<Texture2D>("UI Textures/pauseButton");
            GUI.textures[4] = Content.Load<Texture2D>("UI Textures/saveButton");
            GUI.textures[5] = Content.Load<Texture2D>("UI Textures/loadButton");
            GUI.textures[6] = Content.Load<Texture2D>("UI Textures/exitButton");
            GUI.textures[7] = Content.Load<Texture2D>("UI Textures/resumeButton");
            GUI.textures[8] = Content.Load<Texture2D>("UI Textures/repairButton");
            GUI.pausedOverlay = Content.Load<Texture2D>("UI Textures/pausedOverlay");

            //Load grid textures
            Tile.selectionOverlay = Content.Load<Texture2D>("Tile Textures/gridSelectionOverlay");
            Tile.healthRectangle = Content.Load<Texture2D>("Tile Textures/healthRectangle");

            //Buildings (Cannot have same name)
            Building.buildingTypes.Add(new BuildingType(250, 50, "Basic Mine", 25, Content.Load<Texture2D>("Building Textures/basicMine")));
            Building.buildingTypes.Add(new BuildingType(1000, 150, "Advanced Mine", 100, Content.Load<Texture2D>("Building Textures/advancedMine")));
            Building.buildingTypes.Add(new BuildingType(500, -25, 500, 5, 500, "Zapper", 50, Color.GhostWhite, Content.Load<Texture2D>("Building Textures/zapperBuilding")));
            Building.buildingTypes.Add(new BuildingType(1500, -100, 600, 15, 1000, "Basic Turret", 100, Color.Green, Content.Load<Texture2D>("Building Textures/basicTurret")));
            Building.buildingTypes.Add(new BuildingType(3000, -200, 700, 35, 800, "Adv. Turret", 150, Color.Cyan, Content.Load<Texture2D>("Building Textures/advancedTurret")));

            float buffStrength = .05F;
            Building.buildingTypes.Add(new BuildingType(2500, 0, "Factory GPT", 50, new[] { buffStrength, 0, 0, 0}, Content.Load<Texture2D>("Building Textures/factoryBuilding")));
            Building.buildingTypes.Add(new BuildingType(2500, -100, "Factory Damage", 50, new[] { 0, buffStrength, 0, 0 }, Content.Load<Texture2D>("Building Textures/factoryBuilding")));
            Building.buildingTypes.Add(new BuildingType(2500, -100, "Factory GoldC", 50, new[] { 0, 0, buffStrength, 0 }, Content.Load<Texture2D>("Building Textures/factoryBuilding")));
            Building.buildingTypes.Add(new BuildingType(2500, -100, "Factory EKR", 50, new[] { 0, 0, 0, buffStrength }, Content.Load<Texture2D>("Building Textures/factoryBuilding")));
            Building.buildingTypes.Add(new BuildingType(1000000, 0, "Warp Gate", 1, Content.Load<Texture2D>("Building Textures/blackHole")));

            //Adds Building Buttons
            GUI.initializeBuildingButtons();

            //Init Grid
            Tile.initializeGrid(asteroidTexture.Width);

            //Loading fonts
            displayFont = Content.Load<SpriteFont>("displayFont");
            buildingButtonFont = Content.Load<SpriteFont>("buildingButtonFont");

            //Load Player Collision Points
            Player.initializeCollisionPoints();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //Play audio in update
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (!GUI.paused)
            {
                handleRepairMode();

                //Update Player
                Player.controller(Keyboard.GetState(), gameTime);
                Player.movement();
                Player.initializeCollisionPoints();

                //Update Mouse Input
                Player.mouseController(Mouse.GetState());

                //Update Tile Rectangles
                Tile.updateTileRectangles();

                //Update TEST ENEMY
                Enemy.updateEnemies(gameTime);

                //Update Defense Tiles
                Tile.updateDefenseTiles(gameTime);

                //Update Triggered Buttons
                GUI.updateButtons();

                //Update Projectiles
                Projectile.movement();

                //Checks if player is dead
                if (Player.dead)
                {
                    handlePlayerDeath();
                }

                //Update Keyframes
                ActiveKeyframeEffect.updateActiveKeyframeEffects();

                //Update Turn Check
                checkTurn();
            }
            else
            {
                //Only checks mouse clicks and updates buttons

                //Update Mouse Input
                Player.mouseController(Mouse.GetState());

                //Update Triggered Buttons
                GUI.updateButtons();

                //Asks if exit button has been pressed
                if (GUI.GUIButtons[7].triggered)
                {
                    this.Exit();
                }
            }

            base.Update(gameTime);
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public static int asteroidSize = 4000;
        public static int backgroundPadding = 200;
        public static int repairCursorSize = 35;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //Depth Level 0:
            spriteBatch.Draw(galaxyBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            //Depth Level 1:
            spriteBatch.Draw(galaxyStars, new Rectangle(-backgroundPadding + (int)(offset.X / (backgroundPadding / 10)), -backgroundPadding + (int)(offset.Y / (backgroundPadding / 10)), GraphicsDevice.Viewport.Width + (backgroundPadding * 2), GraphicsDevice.Viewport.Height + (backgroundPadding * 2)), Color.White);

            //Depth Level 2:
            spriteBatch.Draw(asteroidTexture, new Rectangle((GraphicsDevice.Viewport.Width / 2) - (asteroidSize / 2) + (int)offset.X, (GraphicsDevice.Viewport.Height / 2) - (asteroidSize / 2) + (int)offset.Y, asteroidSize, asteroidSize), Color.White);
            Tile.drawTiles(spriteBatch);
            Tile.setTileOverlay(Mouse.GetState(), spriteBatch);

            spriteBatch.Draw(Player.shipTexture[Player.imageID], new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, Player.size[0], Player.size[1]), null, Color.White, Player.rotation, new Vector2(110, 269), SpriteEffects.None, 0F);

            Projectile.drawProjectiles(spriteBatch);
            Enemy.drawEnemies(spriteBatch);

            ActiveKeyframeEffect.drawKeyframeEffects(spriteBatch);

            GUI.drawGold(spriteBatch, displayFont);
            GUI.drawButtons(spriteBatch);
            Minimap.drawMinimap(spriteBatch);

            if (GUI.paused)
            {
                spriteBatch.Draw(GUI.pausedOverlay, screenBounds, Color.White);
            }
            handleRepairModeDraw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static bool duringTurn = false;

        public static int repairCostMultiplier = 8;
        private void handleRepairModeDraw(SpriteBatch spriteBatch)
        {
            if (GUI.repairMode)
            {
                MouseState state = Mouse.GetState();
                spriteBatch.Draw(repairCursor, new Rectangle(state.X - (repairCursorSize / 2), state.Y - (repairCursorSize / 2), repairCursorSize, repairCursorSize), Color.White);
                foreach (Tile b in Tile.tiles)
                {
                    if (b.tileRectangle.Contains(state.X, state.Y))
                    {
                        if (b.built)
                        {
                            spriteBatch.DrawString(displayFont, ((b.building.health - b.health) * repairCostMultiplier).ToString(), new Vector2(state.X + repairCursorSize, state.Y - (repairCursorSize / 2)), Color.Black);
                        }
                        return;
                    }
                }
            } 
        }

        private void handlePlayerDeath()
        {
            Enemy.enemyExplosion.Play(1F, -.5F, 0);
            ActiveKeyframeEffect.activeKeyframeEffects.Add(new ActiveKeyframeEffect(KeyframeEffect.keyframeEffects[0], new Rectangle((int)((Width / 2) - Game1.offset.X - (Player.size[1] / 2)), (int)((Height / 2) - Game1.offset.Y - (Player.size[1] / 2)), Player.size[1], Player.size[1]), Color.White));
            GUI.GUIButtons[0].visible = true;
            GUI.GUIButtons[2].visible = true;
            GUI.GUIButtons[3].visible = true;
            GUI.GUIButtons[8].visible = true;
            IsMouseVisible = true;

            turnNumber--;
            duringTurn = false;
            Enemy.enemies.Clear();
            Projectile.projectiles.Clear();
            Player.dead = false;
        }

        private void handleRepairMode()
        {
            if (GUI.repairMode)
            {
                IsMouseVisible = false;
            }
            else
            {
                IsMouseVisible = true;
            }
        }

        private void checkTurn()
        {
            if (duringTurn && Enemy.enemies.Count() == 0)
            {
                GUI.GUIButtons[0].visible = true;
                GUI.GUIButtons[2].visible = true;
                GUI.GUIButtons[3].visible = true;
                GUI.GUIButtons[8].visible = true;

                IsMouseVisible = true;
                Projectile.projectiles.Clear();

                if (Player.goldPerTurn < 0)
                {
                    Player.gold += Player.goldPerTurn;
                }
                else
                {
                    Player.gold += (int)(Player.goldPerTurn + (Player.goldPerTurn * Buffs.buffs[0]));
                }

                //Rewards for boss turn
                if (turnNumber % 25 == 0)
                {
                    Player.gold += (2000 * (turnNumber / 25)) + ((1000 * (turnNumber / 25)) * Buffs.buffs[3]);
                }

                duringTurn = false;
            }
            else if (duringTurn)
            {
                IsMouseVisible = false;

                //Returns if at least one tile is still built after turn
                foreach (Tile b in Tile.tiles)
                {
                    if (b.built)
                    {
                        return;
                    }
                }

                //Exits if no tiles are still built
                this.Exit();
            }
        }

        public static void nextTurn()
        {
            if (!duringTurn)
            {
                turnNumber++;

                //Makes GUI innaccessible until all enemies are killed
                GUI.buildMenuOpen = false;
                GUI.GUIButtons[0].visible = false;
                GUI.GUIButtons[1].visible = false;
                GUI.GUIButtons[2].visible = false;
                GUI.GUIButtons[3].visible = false;
                GUI.GUIButtons[8].visible = false;

                foreach (BuildingButton b in GUI.buildingButtons)
                {
                    b.visible = false;
                }

                duringTurn = true;

                Enemy.spawnEnemies(1 + (turnNumber / 5));        
            }
        }
    }
}
