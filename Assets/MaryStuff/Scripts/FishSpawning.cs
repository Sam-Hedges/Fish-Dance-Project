using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawning : MonoBehaviour
{
    public GameObject fish;
    GameObject newFish;

    public int side; //0 = left, 1 = right
    public float ySpawn;
    public float xSpawn; 
    public float zSpawn = -3.44f;

    public float scale;

    public static float waitTime;
    public static float lowerWait = 0.5f;
    public static float higherWait = 3f;

    public Vector3 spawnLocation;

    float bottomOfScreen;
    float topOfScreen;
    float rightOfScreen;
    float leftOfScreen;

    public static List<GameObject> fishAmount = new List<GameObject>();

    void Start()
    {
        var dist = (transform.position - Camera.main.transform.position).z;
        leftOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        rightOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        bottomOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        topOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;


        StartCoroutine(WaitForFish());
    }

    IEnumerator WaitForFish()
    {
        waitTime = Random.Range(lowerWait, higherWait);
        yield return new WaitForSeconds(waitTime);
        Spawn();
        StartCoroutine(WaitForFish());
    }

    public virtual void Spawn()
    {
        this.side = Random.Range(0, 2);

        this.ySpawn = Random.Range(bottomOfScreen, topOfScreen);

        switch (this.side)
        {
            case 0:
                this.xSpawn = leftOfScreen;
                break;
            case 1:
                this.xSpawn = rightOfScreen;
                break;
        }

        this.spawnLocation = new Vector3(this.xSpawn, this.ySpawn, this.zSpawn);

        this.scale = Random.Range(0.4f, 1.5f);
        newFish = Instantiate(fish, spawnLocation, Quaternion.identity);

        //random size for fish
        newFish.transform.localScale = Vector3.one * scale;

        //random colour for fish
        newFish.GetComponent<SpriteRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        FishSpawning.fishAmount.Add(newFish);
    }
}
