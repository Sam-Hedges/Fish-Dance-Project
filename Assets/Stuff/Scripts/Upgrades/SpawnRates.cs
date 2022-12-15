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
            this.multiplierText.text = multiplier.ToString("x0.00");
            this.costText.text = cost.ToString("£0.00");
            this.multiplierIncrease = 0.25f;
            this.mStart = "x0.00";

            base.upgrades.Add(4, this);
        }

        public override void DoUpgrade()
        {
            FishSpawning.lowerWait = (2) - (this.multiplier * (FishCollection.rodLength * 1.5f));
            FishSpawning.higherWait = (5) - (this.multiplier * (FishCollection.rodLength * 1.5f));
            Debug.Log("You bought the spawn extension");
        }
    }
}
