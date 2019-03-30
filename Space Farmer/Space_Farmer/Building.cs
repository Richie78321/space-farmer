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
    class Building
    {
        public static List<BuildingType> buildingTypes = new List<BuildingType>();
    }

    class BuildingType
    {
        public string name;
        public int goldCost;
        public int goldPerTurn;
        public int health;

        public bool defense = false;
        public int damage = 0;
        public float shotTime = 0;
        public int radius = 0;
        public Color bulletColor;

        public bool buff = false;
        public bool[] modifier = new bool[4];
        public float[] percentIncrease = { 0, 0, 0, 0 };


        public Texture2D texture;
        
        //Cast for standard building
        public BuildingType(int goldCostInput, int goldPerTurnInput, string nameInput, int healthInput, Texture2D textureInput)
        {
            goldCost = goldCostInput;
            goldPerTurn = goldPerTurnInput;
            name = nameInput;
            texture = textureInput;
            health = healthInput;
        }

        //Cast for defense building
        public BuildingType(int goldCostInput, int goldPerTurnInput, int radiusInput, int damageInput, float shotTimeInput, string nameInput, int healthInput, Color bulletColorInput, Texture2D textureInput)
        {
            defense = true;
            goldCost = goldCostInput;
            goldPerTurn = goldPerTurnInput;
            damage = damageInput;
            shotTime = shotTimeInput;
            radius = radiusInput;
            name = nameInput;
            texture = textureInput;
            health = healthInput;
            bulletColor = bulletColorInput;
        }

        //Cast for buff building
        public BuildingType(int goldCostInput, int goldPerTurnInput, string nameInput, int healthInput, float[] percentIncreaseInput, Texture2D textureInput)
        {
            this.goldCost = goldCostInput;
            goldPerTurn = goldPerTurnInput;
            name = nameInput;
            texture = textureInput;
            health = healthInput;

            for (int i = 0; i < percentIncreaseInput.Count(); i++)
            {
                if (percentIncreaseInput[i] != 0)
                {
                    modifier[i] = true;
                }
            }

            percentIncrease = percentIncreaseInput;
            buff = true;
        }
    }
}
