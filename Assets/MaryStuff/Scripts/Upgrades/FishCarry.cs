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
            this.multiplierText.text = multiplier.ToString();
            this.costText.text = cost.ToString();

            base.upgrades.Add(3, this);
        }

        public override void DoUpgrade()
        {         
            FishCollection.fishAmount += Mathf.RoundToInt(this.multiplier);
            Debug.Log("You bought the carry extension");
        }
    }
}
