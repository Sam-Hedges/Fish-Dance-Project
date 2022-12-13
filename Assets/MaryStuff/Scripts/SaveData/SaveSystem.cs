using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using System.IO;

namespace PortfolioProject
{
    public class SaveSystem : MonoBehaviour
    {
        public Speed speed;
        public LineExtension lineExtension;
        public SpawnRates spawnRates;
        public FishCarry fishCarry;
        public FishCollection fishCollection;

        public TextMeshProUGUI goldDisplay;

        DateTime currentTime;

        public GameObject idleCanvas, mainCanvas, goldCanvas;
        public TextMeshProUGUI idleTimeDisplay, goldEarntDisplay;

        private void Start()
        {
            PlayerData data = SavingNLoading.Load();

            if (data != null)
            {
                currentTime = DateTime.Parse(data.currentTime);
                TimeSpan idleTime = DateTime.Now - currentTime; //comparing the time saved (when you quit) to the time now to work out how much money you would have earnt
                float moneyEarnt = (idleTime.Minutes * data.RodSpawnRates[0] * data.RodCarry[0]);
                Debug.Log(moneyEarnt);

                if (idleTime.Minutes > 0)
                {
                    idleCanvas.SetActive(true);
                    mainCanvas.SetActive(false);
                    goldCanvas.SetActive(false);
                    idleTimeDisplay.text = idleTime.Minutes.ToString("0 Minute(s)");
                    goldEarntDisplay.text = moneyEarnt.ToString("£0");
                    data.gold += moneyEarnt;
                }
                fishCollection.goldAmount = data.gold;

                speed.multiplier = data.RodSpeed[0];
                speed.cost = data.RodSpeed[1];

                lineExtension.multiplier = data.RodLineExtension[0];
                lineExtension.cost = data.RodLineExtension[1];

                spawnRates.multiplier = data.RodSpawnRates[0];
                spawnRates.cost = data.RodSpawnRates[1];

                fishCarry.multiplier = data.RodCarry[0];
                fishCarry.cost = data.RodCarry[1];
            }
            else
            {
                fishCollection.goldAmount = 0;

                speed.multiplier = 1.25f;
                speed.cost = 100;

                lineExtension.multiplier = 2f;
                lineExtension.cost = 100;

                spawnRates.multiplier = 1.25f;
                spawnRates.cost = 100;

                fishCarry.multiplier = 5f;
                fishCarry.cost = 100;
            }
            FishCollection.fishAmount = (int)fishCarry.multiplier - 1;
            FishCollection.rodLength = lineExtension.multiplier - 1;
            speed.Initialise(speed.multiplier, speed.cost);
            lineExtension.Initialise(lineExtension.multiplier, lineExtension.cost);
            spawnRates.Initialise(spawnRates.multiplier, spawnRates.cost);
            fishCarry.Initialise(fishCarry.multiplier, fishCarry.cost);

            goldDisplay.text = fishCollection.goldAmount.ToString("£0");

            Debug.Log("loading");
        }

        void OnApplicationQuit()
        {
            SavingNLoading.Save(fishCollection, speed, lineExtension, spawnRates, fishCarry);
            Debug.Log("saving");
        }

        [ContextMenu("DeleteSaveData")]
        public void DeleteSaveData()
        {
            string path = Application.persistentDataPath + "/data.savedata";
            File.Delete(path);
        }
    }
}
