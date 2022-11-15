using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class SpawnRates : RodUpgrades
    {
        public override void Initialise()
        {
            this.upgradeName = "SpawnRates";
            this.cost = 1;
            this.multiplier = 1.25f;

            foreach (Transform text in transform)
            {
                if (text.name == "Amount")
                {
                    this.multiplierText = text.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                    this.multiplierText.text = this.multiplier.ToString();
                }
                if (text.name == "Cost")
                {
                    this.costText = text.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                    this.costText.text = this.cost.ToString();
                }
            }

            base.upgrades.Add(4, this);
        }

        public override void DoUpgrade()
        {
            FishSpawning.lowerWait = FishSpawning.lowerWait / this.multiplier;
            FishSpawning.higherWait = FishSpawning.higherWait / this.multiplier;
            Debug.Log("You bought the spawn extension");
        }
    }
}
