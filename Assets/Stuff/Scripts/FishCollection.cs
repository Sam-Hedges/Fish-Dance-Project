using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PortfolioProject
{
    public class FishCollection : MonoBehaviour
    {
        public FishSpawning fishSpawning;

        public TextMeshProUGUI goldDisplay;
        public TextMeshProUGUI GoldDisplay;
        public TextMeshProUGUI fishLeftDisplay;
        public int fishLeft = 4;
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
        public AudioClip powerup, jellyfish, fish;

        public List<GameObject> scrolls = new List<GameObject>();

        public float totalMoneyEarnt;

        private void Start()
        {
            StartCoroutine(IdleMoneyGain());
        }

        private void Update()
        {
            if (currentFishAmount != fishAmount && StartFishing.canFishSpawn && collectedFish != null)
            {
                if (magnet)
                {
                    foreach (var item in fishSpawning.fishAmount)
                    {
                        if (item.gameObject.tag == "Fish")
                        {
                            item.GetComponent<FishMove>().reachedLocation = new Vector3(this.transform.position.x, this.transform.position.y, item.transform.position.z);
                        }
                    }
                }
                if (boost)
                {
                    foreach (var item in fishSpawning.fishAmount)
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

        IEnumerator IdleMoneyGain()
        {
            AddIdleMoney();
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(IdleMoneyGain());
        }

        void AddIdleMoney()
        {
            if (!StartFishing.startedFishing)
            {
                goldAmount += (1 * fishAmount) / 2;
                goldDisplay.text = goldAmount.ToString("£0");
            }
        }

        void Fish()
        {
            if (currentFishAmount == fishAmount && StartFishing.sellFish)
            {
                foreach (var item in collectedFish)
                {
                    float moneyEarnt = ((10 * fishSize) * rodLength);
                    CollectFish(item, moneyEarnt);
                }
                collectedFish.Clear();
                currentFishAmount = 0;
                magnet = false;
                boost = false;
                StartCoroutine(FishCollected());
            }
        }

        public IEnumerator FishCollected()
        {
            GoldDisplay.gameObject.SetActive(true);
            GoldDisplay.color = Color.yellow;
            GoldDisplay.text = totalMoneyEarnt.ToString("+0");
            goldAmount += totalMoneyEarnt;
            goldDisplay.text = goldAmount.ToString("£0");
            totalMoneyEarnt = 0;
            yield return new WaitForSeconds(1.5f);
            GoldDisplay.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Fish")
            {
                fishLeft -= 1;
                fishLeftDisplay.text = fishLeft.ToString();
                fishSize = other.transform.localScale.z;
                currentFishAmount++;
                other.GetComponent<Collider>().enabled = false; //this is so the fish can only add 1 to the counter when collided with
                other.GetComponent<FishMove>().enabled = false; //so the fish stops moving
                fishSpawning.fishAmount.Remove(other.gameObject);
                collectedFish.Add(other.gameObject);

                other.transform.localPosition = new Vector3(0f, (this.transform.position.y - 1f), other.transform.localPosition.z);
                other.transform.eulerAngles = new Vector3(-90f, other.transform.rotation.y, -45);
                other.transform.SetParent(this.gameObject.transform);
            }
            if(other.tag == "Magnet")
            {
                StartCoroutine(PowerUp("magnet"));
                collection.clip = powerup;
                collection.Play();
                fishSpawning.fishAmount.Remove(other.gameObject);
                Destroy(other.gameObject);
            }
            if (other.tag == "Boost")
            {
                StartCoroutine(PowerUp("boost"));
                FishSpawning.waitTime = FishSpawning.waitTime / 2.5f;
                collection.clip = powerup;
                collection.Play();
                fishSpawning.fishAmount.Remove(other.gameObject);
                Destroy(other.gameObject);
            }
            if(other.tag == "JellyFish")
            {
                collection.clip = jellyfish;
                collection.Play();
                float minusMoney = Random.Range((fishAmount * 3), (fishAmount * 5));
                goldAmount -= minusMoney;
                if(goldAmount < 0)
                {
                    goldAmount = 0;
                }
                GoldDisplay.gameObject.SetActive(true);
                GoldDisplay.color = Color.red;
                GoldDisplay.text = minusMoney.ToString("-0");
                goldDisplay.text = goldAmount.ToString("£0");
                hitJellyFish = true;
                fishSpawning.fishAmount.Remove(other.gameObject);

                foreach (var item in collectedFish)
                {
                    Destroy(item.gameObject);
                }
                collectedFish.Clear();
            }
        }

        //gets the price of the fish and sells it
        public void CollectFish(GameObject fish, float moneyEarnt)
        {
            totalMoneyEarnt += moneyEarnt;
            fishSpawning.fishAmount.Remove(fish);
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
