using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class PowerUpSpawning : MonoBehaviour
    {
        public float waitTime;

        private void Start()
        {
            StartCoroutine(WaitForSpawn());
        }

        IEnumerator WaitForSpawn()
        {
            waitTime = Random.Range(10f, 24);
            yield return new WaitForSeconds(waitTime);
            SpawnPowerUp();
            StartCoroutine(WaitForSpawn());
        }

        void SpawnPowerUp()
        {
            
        }
    }
}
