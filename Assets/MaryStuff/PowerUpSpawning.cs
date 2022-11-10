using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class PowerUpSpawning : MonoBehaviour
    {
        public float waitTime;

        public GameObject Magnet;
        public GameObject SpeedBoost;

        GameObject newPowerUp;

        public GameObject topLeft;
        public GameObject bottomRight;

        public static bool canSpawn = true; // so another power-up doesn't spawn whilst still using one currently

        private void Start()
        {
            StartCoroutine(WaitForSpawn());
        }

        IEnumerator WaitForSpawn()
        {
            waitTime = Random.Range(15f, 34);
            yield return new WaitForSeconds(waitTime);
            if (canSpawn)
            {
                SpawnPowerUp();
                canSpawn = false;
            }
            StartCoroutine(WaitForSpawn());
        }

        void SpawnPowerUp()
        {
            int chosenPowerUp = Random.Range(1, 3);

            Vector3 minPos = topLeft.transform.position;
            Vector3 maxPos = bottomRight.transform.position;

            Vector3 randomPosition = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), Random.Range(minPos.z, maxPos.z));

            switch (chosenPowerUp)
            {
                case 1: //if it is magnet
                    newPowerUp = Instantiate(Magnet, randomPosition, Quaternion.identity);
                    break;
                case 2: //if it is speed
                    newPowerUp = Instantiate(SpeedBoost, randomPosition, Quaternion.identity);
                    break;
            }
        }
    }
}
