using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class SpawnRates : RodUpgrades
    {
        public override void Initialise(float multiplier, float cost)
        {
            this.upgradeName = "SpawnRates";
            this.multiplierText.text = multiplier.ToString();
            this.costText.text = cost.ToString();

            base.upgrades.Add(4, this);
        }

        public override void DoUpgrade()
        {
            FishSpawning.lowerWait = FishSpawning.lowerWait - this.multiplier;
            FishSpawning.higherWait = FishSpawning.higherWait - this.multiplier;
            Debug.Log("You bought the spawn extension");
        }
    }
}
