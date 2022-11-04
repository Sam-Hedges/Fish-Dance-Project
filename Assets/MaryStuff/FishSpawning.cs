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

    public float waitTime;

    public int direction;

    public Vector3 spawnLocation;

    float bottomOfScreen;
    float topOfScreen;
    float rightOfScreen;
    float leftOfScreen;

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
        waitTime = Random.Range(0.5f, 3);
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
                this.direction = 0;
                this.xSpawn = leftOfScreen;
                break;
            case 1:
                this.direction = 180;
                this.xSpawn = rightOfScreen;
                break;
        }

        this.spawnLocation = new Vector3(this.xSpawn, this.ySpawn, this.zSpawn);
        Quaternion facing = new Quaternion(0, direction, 0, 0);

        this.scale = Random.Range(0.4f, 1.5f);
        newFish = Instantiate(fish, spawnLocation, facing);
        newFish.transform.localScale = Vector3.one * scale;
        newFish.GetComponent<SpriteRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}
