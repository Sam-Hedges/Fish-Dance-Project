using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class Speed : RodUpgrades
    {
        public override void Initialise(float multiplier, float cost)
        {
            this.upgradeName = "Speed";
            this.multiplierText.text = multiplier.ToString("x0.00");
            this.costText.text = cost.ToString("£0.00");
            this.multiplierIncrease = 0.2f;
            this.mStart = "x0.00";

            base.upgrades.Add(2, this);
        }

        public override void DoUpgrade()
        {
            FishSpawning.lowerSpeed = FishSpawning.lowerSpeed * 0.75f;
            FishSpawning.higherSpeed = FishSpawning.higherSpeed * 0.75f;
            Debug.Log("You bought the speed extension");
        }
    }
}
