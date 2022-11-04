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

    public float waitTime;

    public int direction;

    public Vector3 spawnLocation;

    void Start()
    {     
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

        this.ySpawn = Random.Range(-3.42f, 6.24f);

        switch (this.side)
        {
            case 0:
                this.direction = 0;
                this.xSpawn = Random.Range(-11.66f, -7.37f);
                break;
            case 1:
                this.direction = 180;
                this.xSpawn = Random.Range(6.25f, 9.6f);
                break;
        }

        this.spawnLocation = new Vector3(this.xSpawn, this.ySpawn, this.zSpawn);
        Quaternion facing = new Quaternion(0, direction, 0, 0);
        newFish = Instantiate(fish, spawnLocation, facing);
    }
}
