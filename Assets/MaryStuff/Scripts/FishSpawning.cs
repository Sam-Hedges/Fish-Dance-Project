using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawning : MonoBehaviour
{
    public GameObject fish;
    GameObject newFish;

    public GameObject[] bubbles;
    GameObject newBubble;

    public GameObject jellyfish;
    GameObject newJellyFish;

    public int type;
    public int side; //0 = left, 1 = right
    public float ySpawn;
    public float xSpawn; 
    public float zSpawn = -3.44f;

    public float scale;

    public static float waitTime;
    public static float lowerWait = 0.5f;
    public static float higherWait = 3f;

    public Vector3 spawnLocation;

    public float bottomOfScreen;
    public float topOfScreen;
    float rightOfScreen;
    float leftOfScreen;

    public static List<GameObject> fishAmount = new List<GameObject>();

    public List<Material> fishTypes = new List<Material>();

    void Start()
    {
        var dist = (transform.position - Camera.main.transform.position).z;
        leftOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        rightOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        topOfScreen = 6.64f;
    }

    public IEnumerator WaitForFish()
    {
            waitTime = Random.Range(lowerWait, higherWait);
            yield return new WaitForSeconds(waitTime);
            Spawn();
            StartCoroutine(WaitForFish());
    }


    public virtual void Spawn()
    {
        int spawnRotation = 0;
        this.side = Random.Range(0, 2);

        this.ySpawn = Random.Range(bottomOfScreen, topOfScreen);

        switch (this.side)
        {
            case 0:
                this.xSpawn = leftOfScreen;
                spawnRotation = 90;
                break;
            case 1:
                this.xSpawn = rightOfScreen;
                spawnRotation = -90;
                break;
        }

        this.spawnLocation = new Vector3(this.xSpawn, this.ySpawn, this.zSpawn);

        this.scale = Random.Range(0.4f, 1.5f);

        this.type = Random.Range(1,11);

        switch(type)
        {
            default:
                newFish = Instantiate(fish, spawnLocation, Quaternion.Euler(0, spawnRotation, 0));

                //random size for fish
                newFish.transform.localScale = Vector3.one * scale;

                //random colour for fish
                int randomFish = Random.Range(0, fishTypes.Count);
                Material currentFishType = fishTypes[randomFish];
                newFish.GetComponentInChildren<SkinnedMeshRenderer>().material = currentFishType;

                FishSpawning.fishAmount.Add(newFish);
                break;
            case 2:
                newJellyFish = Instantiate(jellyfish, spawnLocation, Quaternion.identity);
                break;
            case 3:
                int randomPowerUp = Random.Range(0, 2);
                switch(randomPowerUp)
                {
                    case 0:
                        newBubble = Instantiate(bubbles[0], spawnLocation, Quaternion.identity);
                        break;
                    case 1:
                        newBubble = Instantiate(bubbles[1], spawnLocation, Quaternion.identity);
                        break;
                }
                break;
        }

    }
}
