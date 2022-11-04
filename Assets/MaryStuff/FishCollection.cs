using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PortfolioProject
{
    public class FishCollection : MonoBehaviour
    {
        public TextMeshProUGUI goldDisplay;
        public float goldAmount;
        public float fishSize;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Fish")
            {
                fishSize = other.transform.localScale.z; //all of the scale is the same so it doesn't matter what value
                CollectFish(other.gameObject);
            }
        }

        //tempoary function to get the price of the fish (which will happen at the end of the dance battle)
        public void CollectFish(GameObject fish)
        {
            goldAmount += (10 * fishSize);
            goldDisplay.text = goldAmount.ToString("£0");
            Destroy(fish);
        }
    }
}
