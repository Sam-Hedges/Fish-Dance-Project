using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class FishCarry : RodUpgrades
    {
        public override void Initialise(float multiplier, float cost)
        {
            this.upgradeName = "FishCarry";
            this.multiplierText.text = multiplier.ToString("0.00 Fish");
            this.costText.text = cost.ToString("£0.00");
            this.multiplierIncrease = 1;
            this.mStart = ("0.00 Fish");

            base.upgrades.Add(3, this);
        }

        public override void DoUpgrade()
        {         
            FishCollection.fishAmount += 1;
            Debug.Log("You bought the carry extension");
        }
    }
}
