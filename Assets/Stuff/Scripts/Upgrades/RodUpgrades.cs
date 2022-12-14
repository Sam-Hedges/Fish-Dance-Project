using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PortfolioProject
{
    public class RodUpgrades : MonoBehaviour
    {
        public GameObject money;

        public Dictionary<int, RodUpgrades> upgrades = new Dictionary<int, RodUpgrades>();

        protected string upgradeName;
        public float cost { get; set; }
        public float multiplier { get; set; }

        public TextMeshProUGUI multiplierText, costText;

        //this is to set the values on the children scripts for each upgrade

        private void Update()
        {
            foreach (var item in upgrades)
            {
                ChangeColour(upgrades[item.Key].name);
            }
        }
        public virtual void Initialise(float multiplier, float cost)    {   }

        public virtual void DoUpgrade()    {       }

        public virtual void ChangeColour(string upgradeType)
        {
            foreach (var item in upgrades)
            {
                if (upgrades[item.Key].upgradeName == upgradeType)
                {
                    if (money.GetComponent<FishCollection>().goldAmount < upgrades[item.Key].cost)
                    {
                        upgrades[item.Key].GetComponent<Image>().color = new Color32(63, 63, 63, 255);
                    }
                    else if (money.GetComponent<FishCollection>().goldAmount >= upgrades[item.Key].cost)
                    {
                        upgrades[item.Key].GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                    }
                }
            }
        }

        //this is called first to see if you have enough money
        public void CheckMoney(string upgradeType)
        {

            foreach (var item in upgrades)
            {
                if (upgrades[item.Key].upgradeName == upgradeType)
                {
                    if (money.GetComponent<FishCollection>().goldAmount >= upgrades[item.Key].cost)
                    {
                        DoUpgrade();
                        ChangeValues(upgrades[item.Key]);
                        ChangeDisplay(upgrades[item.Key]);
                    }
                }
            }
        }

        void ChangeValues(RodUpgrades rodUpgrades)
        {
            money.GetComponent<FishCollection>().goldAmount -= rodUpgrades.cost;
            rodUpgrades.cost += (1.25f * cost);
            rodUpgrades.multiplier++;
        }

        void ChangeDisplay(RodUpgrades rodUpgrades)
        {
            money.GetComponent<FishCollection>().goldDisplay.text = money.GetComponent<FishCollection>().goldAmount.ToString("£0");
            costText.text = rodUpgrades.cost.ToString();
            multiplierText.text = rodUpgrades.multiplier.ToString("0.00");
        }
    }
}
