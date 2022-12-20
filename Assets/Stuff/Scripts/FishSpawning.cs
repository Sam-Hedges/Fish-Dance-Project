using PortfolioProject;
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
    public static float lowerWait = 2f;
    public static float higherWait = 5f;
    public static float lowerSpeed = 0.08f;
    public static float higherSpeed = 0.12f;

    public Vector3 spawnLocation;

    public float bottomOfScreen;
    public float topOfScreen;
    float rightOfScreen;
    float leftOfScreen;
    float dist;

    public int randomFish;

    public List<GameObject> fishAmount = new List<GameObject>();

    public List<Material> fishTypes = new List<Material>();

    void Start()
    {
        dist = (transform.position - Camera.main.transform.position).z;
        leftOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        rightOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        StartCoroutine(WaitForFish());
    }

    private void Update()
    {
        topOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;
        bottomOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
    }

    public IEnumerator WaitForFish()
    {
        waitTime = Random.Range(lowerWait, higherWait);
        yield return new WaitForSeconds(waitTime);
        Spawn();
        StartCoroutine(WaitForFish());
    }


    public void Spawn()
    {
        //this is so the spawning doesn't break
        if(lowerWait <= 0.1)
        {
            lowerWait = 0.1f;
        }
        if (higherWait <= 0.2)
        {
            higherWait = 0.2f;
        }
        int spawnRotation = 0;
        if (StartFishing.canFishSpawn)
        {
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

            this.type = Random.Range(1, 11);

            switch (type)
            {
                default:
                    newFish = Instantiate(fish, spawnLocation, Quaternion.Euler(0, spawnRotation, 0));

                    //random size for fish
                    newFish.transform.localScale = Vector3.one * scale;
                    FishCheck();
                    //random colour for fish
                    Material currentFishType = fishTypes[randomFish];
                    newFish.GetComponentInChildren<SkinnedMeshRenderer>().material = currentFishType;
                    newFish.GetComponent<FishMain>().type = randomFish + 1;
                    newFish.GetComponent<FishMain>().size = this.scale;
                    newFish.GetComponent<FishMove>().speed = Random.Range(lowerSpeed, higherSpeed);

                    fishAmount.Add(newFish);
                    break;
                case 2:
                    newJellyFish = Instantiate(jellyfish, spawnLocation, Quaternion.identity);
                    newJellyFish.GetComponent<FishMove>().speed = Random.Range(lowerSpeed, higherSpeed);
                    fishAmount.Add(newJellyFish);
                    break;
                case 3:
                    int randomPowerUp = Random.Range(0, 2);
                    switch (randomPowerUp)
                    {
                        case 0:
                            newBubble = Instantiate(bubbles[0], spawnLocation, Quaternion.identity);
                            newBubble.GetComponent<FishMove>().speed = Random.Range(lowerSpeed, higherSpeed);
                            fishAmount.Add(newBubble);
                            break;
                        case 1:
                            newBubble = Instantiate(bubbles[1], spawnLocation, Quaternion.identity);
                            newBubble.GetComponent<FishMove>().speed = Random.Range(lowerSpeed, higherSpeed);
                            fishAmount.Add(newBubble);
                            break;
                    }
                    break;
            }
        }
    }

    public void FishCheck()
    {
        int randomChance = Random.Range(1, 101);
        //gets ySpawn - the lower it is the higher chance of rarer fish

        if (randomChance<= 25) // 25%
        {
            randomFish = 0;
        }
        else if (randomChance > 25 && randomChance <= 45) //20%
        {
            randomFish = 5;
        }
        else if (randomChance > 45 && randomChance <= 63) //18%
        {
            randomFish = 4;
        }
        else if (randomChance > 63 && randomChance <= 78) //15%
        {
            randomFish = 3;
        }
        else if (randomChance > 78 && randomChance <= 90) //12%
        {
            randomFish = 2;
        }
        else if (randomChance > 90 && randomChance <= 98) //8%
        {
            randomFish = 1;
        }
        else if (randomChance > 98) //2%
        {
            randomFish = 6;
        }
    }
}
