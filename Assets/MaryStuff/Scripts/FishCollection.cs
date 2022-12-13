using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PortfolioProject
{
    public class FishCollection : MonoBehaviour
    {
        public TextMeshProUGUI goldDisplay;
        public TextMeshProUGUI minusGoldDisplay;
        public TextMeshProUGUI fishLeftDisplay;
        int fishLeft = 4;
        public float goldAmount;
        public float fishSize;

        float powerUpTimer = 10;

        bool magnet;
        bool boost;
        public bool hitJellyFish = false;

        public static int fishAmount;
        public int currentFishAmount;
        public static float rodLength;
        public static List<GameObject> collectedFish = new List<GameObject>();

        public AudioSource collection;

        public List<GameObject> scrolls = new List<GameObject>();


        private void Update()
        {
            if (currentFishAmount != fishAmount && StartFishing.canFishSpawn && collectedFish != null)
            {
                if (magnet)
                {
                    foreach (var item in FishSpawning.fishAmount)
                    {
                        item.GetComponent<FishMove>().reachedLocation = new Vector3(this.transform.position.x, this.transform.position.y, item.transform.position.z);
                    }
                }
                if (boost)
                {
                    foreach (var item in FishSpawning.fishAmount)
                    {
                        item.GetComponent<FishMove>().speed = item.GetComponent<FishMove>().speed * 2.5f;
                    }
                    foreach (var item in scrolls)
                    {
                        item.GetComponent<WaterScrollEffect>().ySpeed = 0.6f;
                    }
                    boost = false;
                }
            }

            Fish();
        }

        void Fish()
        {

            foreach (var item in collectedFish)
            {
                item.GetComponent<FishMove>().reachedLocation = new Vector3(this.transform.position.x, this.transform.position.y, item.transform.position.z); //so the fish follows the rod to the top
            }

            if (currentFishAmount == fishAmount && StartFishing.sellFish || hitJellyFish && !StartFishing.canFishSpawn)
            {
                fishLeft = fishAmount;
                foreach (var item in collectedFish)
                {
                    CollectFish(item);
                }
                collectedFish.Clear();
                currentFishAmount = 0;
                magnet = false;
                boost = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Fish")
            {
                fishLeft--;
                fishLeftDisplay.text = fishLeft.ToString();
                fishSize = other.transform.localScale.z;
                currentFishAmount++;
                other.GetComponent<Collider>().enabled = false; //this is so the fish can only add 1 to the counter when collided with
                other.GetComponent<FishMove>().collected = true;
                FishSpawning.fishAmount.Remove(other.gameObject);
                collectedFish.Add(other.gameObject);
            }
            if(other.tag == "Magnet")
            {
                StartCoroutine(PowerUp("magnet"));
                collection.Play();
                Destroy(other.gameObject);
            }
            if (other.tag == "Boost")
            {
                StartCoroutine(PowerUp("boost"));
                FishSpawning.waitTime = FishSpawning.waitTime / 2.5f;
                collection.Play();
                Destroy(other.gameObject);
            }
            if(other.tag == "JellyFish")
            {
                float minusMoney = Random.Range((fishAmount * 3), (fishAmount * 5));
                goldAmount -= minusMoney;
                if(goldAmount < 0)
                {
                    goldAmount = 0;
                }
                minusGoldDisplay.gameObject.SetActive(true);
                minusGoldDisplay.text = minusMoney.ToString("-0");
                goldDisplay.text = goldAmount.ToString("£0");

                //magnet = false;
                //boost = false;
                //foreach (var item in collectedFish)
                //{
                //    Destroy(item.gameObject);
                //}
                //collectedFish.Clear();
                //currentFishAmount = 0;
                //Destroy(other.gameObject);
                hitJellyFish = true;
            }
        }

        //gets the price of the fish and sells it
        public void CollectFish(GameObject fish)
        {
            goldAmount += ((10 * fishSize) * rodLength);
            goldDisplay.text = goldAmount.ToString("£0");
            FishSpawning.fishAmount.Remove(fish);
            Destroy(fish);
        }

        IEnumerator PowerUp(string powerUpType)
        {
            switch (powerUpType)
            {
                case "magnet":
                    this.magnet = true;
                    break;
                case "boost":
                    this.boost = true;
                    break;
            }

            for (int i = 0; i < powerUpTimer; i++)
            {
                yield return new WaitForSeconds(1);
            }

            switch (powerUpType)
            {
                case "magnet":
                    this.magnet = false;
                    break;
                case "boost":
                    this.boost = false;
                    break;
            }
        }
    }
}
