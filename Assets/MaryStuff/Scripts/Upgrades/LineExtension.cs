using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class LineExtension : RodUpgrades
    {
        FishCollection fishCollection;
        public override void Initialise()
        {
            base.Initialise();
            this.upgradeName = "LineExtension";
            this.cost = 10;
            this.multiplier = 1.25f;
            fishCollection.rodLength = 5f;

            //cycling through all the text gui in this gameobject (children)
            foreach (Transform text in transform)
            {
                if(text.name == "Amount")
                {
                    this.multiplierText = text.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                    this.multiplierText.text = this.multiplier.ToString();
                }
                if(text.name == "Cost")
                {
                    this.costText = text.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                    this.costText.text = this.cost.ToString();
                }
            }

            base.upgrades.Add(1, this);
        }

        public override void DoUpgrade()
        {
            Debug.Log("You bought the line extension");
        }
    }
}
