using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PortfolioProject
{
    public class SavingNLoading : MonoBehaviour
    {
        public TextMeshProUGUI goldDisplay;

        public FishCarry fishCarry;
        public LineExtension lineExtension;
        public Speed speed;
        public SpawnRates spawnRates;
        private void Start()
        {
            FishCollection.goldAmount = PlayerPrefs.GetFloat("Money");
            goldDisplay.text = FishCollection.goldAmount.ToString("£0");

            fishCarry.multiplier = PlayerPrefs.GetFloat("fishCarryMultiplier");
            fishCarry.multiplierText.text = fishCarry.multiplier.ToString();
            fishCarry.cost = PlayerPrefs.GetFloat("fishCarryCost");
            fishCarry.costText.text = fishCarry.cost.ToString();

            lineExtension.multiplier = PlayerPrefs.GetFloat("lineExtensionMultiplier");
            lineExtension.multiplierText.text = lineExtension.multiplier.ToString();
            lineExtension.cost = PlayerPrefs.GetFloat("lineExtensionCost");
            lineExtension.costText.text = lineExtension.cost.ToString();

            speed.multiplier = PlayerPrefs.GetFloat("speedMultiplier");
            speed.multiplierText.text = speed.multiplier.ToString();
            speed.cost = PlayerPrefs.GetFloat("speedCost");
            speed.costText.text = speed.cost.ToString();

            spawnRates.multiplier = PlayerPrefs.GetFloat("spawnRatesMultiplier");
            spawnRates.multiplierText.text = spawnRates.multiplier.ToString();
            spawnRates.cost = PlayerPrefs.GetFloat("spawnRatesCost");
            spawnRates.costText.text = spawnRates.cost.ToString();
        }

        void OnApplicationQuit()
        {
            PlayerPrefs.SetFloat("Money", FishCollection.goldAmount);

            PlayerPrefs.SetFloat("fishCarryMultiplier", fishCarry.multiplier);
            PlayerPrefs.SetFloat("fishCarryCost", fishCarry.cost);

            PlayerPrefs.SetFloat("lineExtensionMultiplier", lineExtension.multiplier);
            PlayerPrefs.SetFloat("lineExtensionCost", lineExtension.cost);

            PlayerPrefs.SetFloat("speedMultiplier", speed.multiplier);
            PlayerPrefs.SetFloat("speedCost", speed.cost);

            PlayerPrefs.SetFloat("spawnRatesMultiplier", spawnRates.multiplier);
            PlayerPrefs.SetFloat("spawnRatesCost", spawnRates.cost);

            PlayerPrefs.Save();
            Debug.Log("Saving");
        }
    }
}
