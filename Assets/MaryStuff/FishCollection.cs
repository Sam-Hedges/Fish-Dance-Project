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

        float powerUpTimer = 10;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Fish")
            {
                fishSize = other.transform.localScale.z;
                CollectFish(other.gameObject);
            }
            if(other.tag == "Magnet")
            {
                StartCoroutine(MagnetPowerUp());
            }
        }

        //tempoary function to get the price of the fish (which will happen at the end of the dance battle)
        public void CollectFish(GameObject fish)
        {
            goldAmount += (10 * fishSize);
            goldDisplay.text = goldAmount.ToString("£0");
            FishSpawning.fishAmount.Remove(fish);
            Destroy(fish);
        }

        IEnumerator MagnetPowerUp()
        {
            for (int i = 0; i < powerUpTimer; i++)
            {
                foreach (var item in FishSpawning.fishAmount)
                {
                    item.GetComponent<FishMove>().reachedLocation = new Vector3(this.transform.position.x, this.transform.position.y, item.transform.position.z); ;
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}
