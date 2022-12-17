using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class WallsMain : MonoBehaviour
    {
        public float[] fishSpawnChances = new float[7];

        public float topOfArea;
        public float bottomOfArea;

        public float sizeScale = 1;

        public void Start()
        {
            fishSpawnChances[0] = 0f; //bluefish
            fishSpawnChances[1] = 15f; //goldfish
            fishSpawnChances[2] = 30f; //GoldFish
            fishSpawnChances[3] = 50f; //grammafish
            fishSpawnChances[4] = 70f; //greenfish
            fishSpawnChances[5] = 77f; //pinkfish
            fishSpawnChances[6] = 95f; //tuna
        }
    }
}
