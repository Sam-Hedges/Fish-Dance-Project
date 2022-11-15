using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PortfolioProject
{
    public class RodUpgrades : MonoBehaviour
    {
        public GameObject money;
        float currentMoney;

        public Dictionary<int, RodUpgrades> upgrades = new Dictionary<int, RodUpgrades>();

        protected string upgradeName;
        protected float cost { get; set; }
        protected float multiplier { get; set; }

        public TextMeshProUGUI multiplierText, costText;

        private void Start()
        {
            money = GameObject.Find("hook");
            Initialise();
        }

        //this is to set the values on the children scripts for each upgrade
        public virtual void Initialise()    {   }

        public virtual void DoUpgrade()  { }

        //this is called first to see if you have enough money
        public void CheckMoney(string upgradeType)
        {
            currentMoney = money.GetComponent<FishCollection>().goldAmount;

            foreach (var item in upgrades)
            {
                if (upgrades[item.Key].upgradeName == upgradeType)
                {
                    if (currentMoney >= upgrades[item.Key].cost)
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
            currentMoney -= rodUpgrades.cost;
            rodUpgrades.cost += (2 * cost);
            rodUpgrades.multiplier += (int)(0.5f + multiplier);
        }

        void ChangeDisplay(RodUpgrades rodUpgrades)
        {
            money.GetComponent<FishCollection>().goldDisplay.text = currentMoney.ToString("£0");
            money.GetComponent<FishCollection>().goldAmount = currentMoney;
            costText.text = rodUpgrades.cost.ToString();
            multiplierText.text = rodUpgrades.multiplier.ToString();
        }
    }
}
