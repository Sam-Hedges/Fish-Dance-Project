using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class LineExtension : RodUpgrades
    {
        public override void Initialise(float multiplier, float cost)
        {
            this.upgradeName = "LineExtension";
            this.multiplierText.text = multiplier.ToString("0.00m");
            this.costText.text = cost.ToString("£0.00");
            this.multiplierIncrease = 1;
            this.mStart = "0.00m";

            base.upgrades.Add(1, this);
        }

        public override void DoUpgrade()
        {
            Debug.Log("You bought the line extension");
            FishCollection.rodLength += 1;
        }
    }
}
