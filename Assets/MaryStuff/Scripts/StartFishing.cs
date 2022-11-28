using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PortfolioProject
{
    public class StartFishing : MonoBehaviour
    {
        public FishSpawning fishSpawning;
        public FishCollection fishCollection;

        public GameObject rod;
        public GameObject wall;
        GameObject newWall;

        Vector3 screenBottom;

        public bool canFishSpawn = false;
        public bool spawnNewWalls;

        Vector3 wallSpawnPos;

        public List<GameObject> walls = new List<GameObject>();

        private void Update()
        {
            TestRodLocation();
        }

        void TestRodLocation()
        {
            if (!canFishSpawn)
            {
                RodDown();

                if (rod.transform.position == screenBottom)
                {
                    spawnNewWalls = true;
                }
            }

            if (spawnNewWalls)
            {
                //will only instantiate once (loop)
                if (newWall == null)
                {
                    for (int i = 0; i < fishCollection.rodLength - 1; i++)
                    {
                        newWall = Instantiate(wall, wallSpawnPos, Quaternion.identity);
                        walls.Add(newWall);
                        wallSpawnPos = new Vector3(newWall.transform.position.x, newWall.transform.position.y - 11.36f, newWall.transform.position.z); //spawns it below the last wall
                    }
                }

                Debug.Log(walls[walls.Count - 1].transform.position.y);
                if (walls[walls.Count - 1].transform.position.y < 6.5f) //if the wall at the bottom isn't at the centre then keep moving every object in the array up
                {
                    foreach (var item in walls)
                    {      
                        Vector3 wallMovePos = new Vector3(item.transform.position.x, item.transform.position.y + (11.36f * (walls.Count - 1)), item.transform.position.z);
                        item.transform.position = Vector3.MoveTowards(item.transform.position, wallMovePos, 0.03f);
                    }
                }
                if (walls[walls.Count - 1].transform.position.y >= 6.5f) //if the wall at the bottom reaches the main camera area (you reached as far as you can go)
                {
                    screenBottom = new Vector3(-0.13f, 5.72f, -3.44f);

                    if (rod.transform.position == screenBottom /*&& bottom of the walls is at the top*/)
                    {
                        spawnNewWalls = false;
                        fishSpawning.StartCoroutine(fishSpawning.WaitForFish());
                        canFishSpawn = true;
                    }
                }
            }
        }

        void Start()
        {
            screenBottom = new Vector3(-0.13f, 9.83f, -3.44f);
            walls.Add(wall);
            fishCollection.rodLength = 2;
            wallSpawnPos = new Vector3(wall.transform.position.x, wall.transform.position.y - 11.36f, wall.transform.position.z);
            RodDown();
        }

        void RodDown()
        {
            rod.transform.position = Vector3.MoveTowards(rod.transform.position, screenBottom, 0.03f);
        }

        void RodUp()
        {

        }

        void RodFollowMouse()
        {

        }
    }
}
