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
        GameObject cam;
        public GameObject mainCanvas;

        public GameObject wall;
        GameObject newWall;

        Vector3 camMovePos;
        Vector3 rodMovePos;

        public bool canFishSpawn = false;

        Vector3 wallSpawnPos;

        public List<GameObject> walls = new List<GameObject>();

        bool startedFishing;

        public static bool sellFish = false;

        void Start()
        {
            cam = Camera.main.gameObject;
            rod.GetComponentInChildren<SpriteRenderer>().enabled = false;
            FishCollection.rodLength = 2f;
            fishSpawning.topOfScreen = 5.46f;
            fishSpawning.bottomOfScreen = -9.97f;

            walls.Add(wall);
            wallSpawnPos = new Vector3(wall.transform.position.x, wall.transform.position.y - 11.36f, wall.transform.position.z);
        }

        private void Update()
        {
            TestRodLocation();

            if(fishCollection.currentFishAmount == FishCollection.fishAmount)
            {
                canFishSpawn = false;
                startedFishing = false;
                rodMovePos = new Vector3(rod.transform.position.x, 16.5f, rod.transform.position.z);
                camMovePos = new Vector3(cam.transform.position.x, 11.14f, cam.transform.position.z);
                RodUp();
            }

            if(!canFishSpawn)
            {
                foreach (var item in FishSpawning.fishAmount)
                {
                    Destroy(item);
                }
                StopCoroutine(fishSpawning.WaitForFish());
            }
        }

        void TestRodLocation()
        {
            if(startedFishing && !canFishSpawn)
            {
                RodDown();

                if (rod.transform.position.y == (walls[walls.Count - 1].transform.position.y + 1.5f))
                {
                    StartCoroutine(fishSpawning.WaitForFish());
                    canFishSpawn = true;
                }
            }
        }

        public void SpawnWalls()
        {
            if (!startedFishing) //can't spawn walls multiple times when not fishing
            {
                sellFish = false;
                //repeats spawning the walls below the other depepnding on the length - 1 (because a wall will always start there)
                for (int i = 0; i < FishCollection.rodLength - 1; i++)
                {
                    newWall = Instantiate(wall, wallSpawnPos, Quaternion.identity);
                    walls.Add(newWall);
                    wallSpawnPos = new Vector3(newWall.transform.position.x, newWall.transform.position.y - 11.36f, newWall.transform.position.z); //spawns it below the last wall
                }
                fishSpawning.topOfScreen = walls[0].transform.position.y; //the very top of all the walls together
                fishSpawning.bottomOfScreen = walls[walls.Count - 1].transform.position.y - 11.36f; //the bottom of the last wall in the array

                camMovePos = new Vector3(cam.transform.position.x, 1, cam.transform.position.z); //moves the camera down to the centre of the first wall
                rodMovePos = new Vector3(rod.transform.position.x, 11.82f, rod.transform.position.z); //moves rod to centre of screen
                startedFishing = true;
            }
        }
        //will be called at the start to send your rod to the bottom (depending on line length)
        void RodDown()
        {
            if(cam.transform.position.y <= 1)
            {
                rod.GetComponentInChildren<SpriteRenderer>().enabled = true;
                rod.transform.position = Vector3.MoveTowards(rod.transform.position, rodMovePos, 0.03f);
                if (rod.transform.position.y <= 11.82f) //if the rod is in the middle of the screen then both the rod and camera move to the bottom
                {
                    camMovePos = new Vector3(cam.transform.position.x, walls[walls.Count - 1].transform.position.y - 5.5f, cam.transform.position.z); //camera will move down to the last wall
                    rodMovePos = new Vector3(rod.transform.position.x, walls[walls.Count - 1].transform.position.y + 1.5f, rod.transform.position.z);
                    rod.transform.position = Vector3.MoveTowards(rod.transform.position, rodMovePos, 0.03f);
                }
            }
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camMovePos, 0.05f);
        }

        //will be called when you have collected the desired amount of fish
        void RodUp()
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camMovePos, 0.05f);
            rod.transform.position = Vector3.MoveTowards(rod.transform.position, rodMovePos, 0.06f);

            if(cam.transform.position == camMovePos && rod.transform.position == rodMovePos)
            {
                rod.GetComponentInChildren<SpriteRenderer>().enabled = false;
                mainCanvas.SetActive(true);
                sellFish = true;
            }
        }

        void RodFollowMouse()
        {

        }
    }
}
