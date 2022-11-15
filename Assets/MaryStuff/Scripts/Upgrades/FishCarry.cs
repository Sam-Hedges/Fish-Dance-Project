using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class FishCarry : RodUpgrades
    {
        public override void Initialise()
        {
            this.upgradeName = "FishCarry";
            this.cost = 100;
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

            base.upgrades.Add(3, this);
        }

        public override void DoUpgrade()
        {
            Debug.Log("You bought the carry extension");
        }
    }
}
