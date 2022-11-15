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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Fish")
            {
                fishSize = other.transform.localScale.z;
                CollectFish(other.gameObject);
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

        //tempoary function to get the price of the fish (which will happen at the end of the dance battle)
        public void CollectFish(GameObject fish)
        {
            goldAmount += (10 * fishSize);
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

            Debug.Log(this.magnet);
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
