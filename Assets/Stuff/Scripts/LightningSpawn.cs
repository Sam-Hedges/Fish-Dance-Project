using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class LightningSpawn : MonoBehaviour
    {
        public GameObject lightning, Light;
        public AudioSource lightningSound;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitForLightning());
        }

        IEnumerator WaitForLightning()
        {
            int waitTime = Random.Range(30, 61);
            yield return new WaitForSeconds(waitTime);
            float spawnPos = Random.Range(-8.72f, 9.64f);
            Vector3 spawnPosV3 = new Vector3(spawnPos, 6.16f, 8.68f);
            GameObject newLightning = Instantiate(lightning, spawnPosV3, Quaternion.identity);
            Light.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Light.SetActive(false);
            lightningSound.Play();
            yield return new WaitForSeconds(2f);
            Destroy(newLightning);
            StartCoroutine(WaitForLightning());
        }
    }
}
