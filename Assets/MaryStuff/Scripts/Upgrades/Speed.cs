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
            this.multiplierText.text = multiplier.ToString();
            this.costText.text = cost.ToString();

            base.upgrades.Add(2, this);
        }

        public override void DoUpgrade()
        {
            Debug.Log("You bought the speed extension");
        }
    }
}
