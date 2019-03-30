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
    class KeyframeEffect
    {
        public static List<KeyframeEffect> keyframeEffects = new List<KeyframeEffect>();
        public static void loadKeyframes(ContentManager Content)
        {
            foreach (KeyframeEffect b in keyframeEffects)
            {
                for (int i = 1; i < b.totalKeyframes; i++)
                {
                    if (i < 10)
                    {
                        b.frames[i] = Content.Load<Texture2D>(b.imagesLocation + "000" + i.ToString());
                    }
                    else if (i < 100)
                    {
                        b.frames[i] = Content.Load<Texture2D>(b.imagesLocation + "00" + i.ToString());
                    }
                    else if (i < 1000)
                    {
                        b.frames[i] = Content.Load<Texture2D>(b.imagesLocation + "0" + i.ToString());
                    }
                    else
                    {
                        b.frames[i] = Content.Load<Texture2D>(b.imagesLocation + i.ToString());
                    }
                }
            }
        }

        //Object
        public int totalKeyframes;
        private string imagesLocation;
        public Texture2D[] frames = new Texture2D[0];

        public KeyframeEffect(int totalKeyframesInput, string imagesLocationInput)
        {
            totalKeyframes = totalKeyframesInput;
            imagesLocation = imagesLocationInput;
            Array.Resize(ref frames, totalKeyframes);
        }
    }

    class ActiveKeyframeEffect
    {
        public static List<ActiveKeyframeEffect> activeKeyframeEffects = new List<ActiveKeyframeEffect>();

        public static void updateActiveKeyframeEffects()
        {
            for (int i = 0; i < activeKeyframeEffects.Count(); i++)
            {
                activeKeyframeEffects[i].currentKeyframe++;

                if (activeKeyframeEffects[i].currentKeyframe >= activeKeyframeEffects[i].effect.totalKeyframes)
                {
                    //Deletes finished effect
                    activeKeyframeEffects.Remove(activeKeyframeEffects[i]);
                    i--;
                    if (i < 0)
                    {
                        return;
                    }
                }
            }
        }

        public static void drawKeyframeEffects(SpriteBatch spriteBatch)
        {
            foreach (ActiveKeyframeEffect b in activeKeyframeEffects)
            {
                Rectangle currentRec = new Rectangle(b.rectangle.X + (int)Game1.offset.X, b.rectangle.Y + (int)Game1.offset.Y, b.rectangle.Width, b.rectangle.Height);

                spriteBatch.Draw(b.effect.frames[b.currentKeyframe], currentRec, b.color);
            }
        }

        //Object
        public int currentKeyframe = 1;
        public Rectangle rectangle;
        public KeyframeEffect effect;
        public Color color;

        public ActiveKeyframeEffect(KeyframeEffect effect, Rectangle positionRectangle, Color colorInput)
        {
            this.effect = effect;
            rectangle = positionRectangle;
            color = colorInput;
        }
    }
}
