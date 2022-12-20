using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace PortfolioProject
{
    [System.Serializable]
    public class PlayerData
    {
        public float gold;
        public float[] RodSpeed = new float[2];
        public float[] RodLineExtension = new float[2];
        public float[] RodSpawnRates = new float[2];
        public float[] RodCarry = new float[2];
        public float[] mixerValues = new float[3];

        public string currentTime;

        public PlayerData(FishCollection fishCollection, Speed speed, LineExtension lineExtension, SpawnRates spawnRates, FishCarry fishCarry, MixerController mixer)
        {
            currentTime = DateTime.Now.ToString();
            gold = fishCollection.goldAmount;

            RodSpeed[0] = speed.multiplier;
            RodSpeed[1] = speed.cost;

            RodLineExtension[0] = lineExtension.multiplier;
            RodLineExtension[1] = lineExtension.cost;

            RodSpawnRates[0] = spawnRates.multiplier;
            RodSpawnRates[1] = spawnRates.cost;

            RodCarry[0] = fishCarry.multiplier;
            RodCarry[1] = fishCarry.cost;

            mixerValues[0] = mixer.GetComponent<Slider>().value;
            mixerValues[1] = mixer.sfxSlider.value;
            mixerValues[2] = mixer.musicSlider.value;

        }
    }
}
