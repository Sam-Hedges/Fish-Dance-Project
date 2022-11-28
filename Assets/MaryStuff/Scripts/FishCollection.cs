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

        bool magnet;
        bool boost;

        private int fishAmount = 4;
        public int currentFishAmount;
        public float rodLength = 1;
        public List<GameObject> collectedFish = new List<GameObject>();

        public AudioSource collection;

        public List<GameObject> scrolls = new List<GameObject>();

        private void Update()
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
            else if (PowerUpSpawning.canSpawn)
            {
                foreach (var item in scrolls)
                {
                    item.GetComponent<WaterScrollEffect>().ySpeed = 0.2f;
                }
            }

            if (collectedFish != null)
            {
                foreach (var item in collectedFish)
                {
                    item.GetComponent<FishMove>().reachedLocation = new Vector3(this.transform.position.x, this.transform.position.y, item.transform.position.z); //so the fish follows the rod to the top
                }

                if (currentFishAmount == fishAmount)
                {
                    foreach (var item in collectedFish)
                    {
                        CollectFish(item);
                    }
                    collectedFish.Clear();
                    currentFishAmount = 0;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Fish")
            {
                fishSize = other.transform.localScale.z;
                currentFishAmount++;
                other.GetComponent<Collider2D>().enabled = false; //this is so the fish can only add 1 to the counter when collided with
                other.GetComponent<FishMove>().collected = true;
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
            PowerUpSpawning.canSpawn = true;

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
