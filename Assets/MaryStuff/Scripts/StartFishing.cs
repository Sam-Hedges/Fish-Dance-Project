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
        public GameObject fishCanvas;
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
            FishCollection.rodLength = 1f;
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
                rod.GetComponent<FollowMouse>().enabled = false;
                canFishSpawn = false;
                startedFishing = false;
                rodMovePos = new Vector3(rod.transform.position.x, 9.15f, rod.transform.position.z);
                camMovePos = new Vector3(cam.transform.position.x, 12.79f, cam.transform.position.z);
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

                if (rod.transform.position.y == (walls[walls.Count - 1].gameObject.transform.Find("RodBottomLoc").position.y))
                {
                    rod.GetComponent<FollowMouse>().enabled = true;
                    StartCoroutine(fishSpawning.WaitForFish());
                    canFishSpawn = true;
                }
            }

            if(canFishSpawn)
            {
                //foreach (var item in walls)
                //{
                //    if (rod.transform.position.y >= item.transform.position.y + 12.42648f && rod.transform.position.y <= item.transform.position.y + 16.08f)
                //    {
                //        Debug.Log("Move rod up");
                //        camMovePos.y = 0.75f;
                //        RodUp();
                //    }

                //    if (rod.transform.position.y >= item.transform.position.y + 7.38f && rod.transform.position.y <= item.transform.position.y + 9.7f)
                //    {
                //        Debug.Log("Move Rod down");
                //        camMovePos.y = walls[walls.Count - 1].transform.position.y + 0.75f;
                //        RodDown();
                //    }
                //}
            }
        }

        public void SpawnWalls()
        {
            Debug.Log(FishCollection.rodLength);
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
                fishSpawning.bottomOfScreen = walls[walls.Count - 1].transform.position.y; //the bottom of the last wall in the array

                camMovePos = new Vector3(cam.transform.position.x, 1, cam.transform.position.z); //moves the camera down to the centre of the first wall
                rodMovePos = new Vector3(rod.transform.position.x, 0.86f, rod.transform.position.z); //moves rod to centre of screen
                startedFishing = true;
            }
        }

        //will be called at the start to send your rod to the bottom (depending on line length)
        void RodDown()
        {
            if (!canFishSpawn)
            {
                if (cam.transform.position.y <= 1)
                {
                    rod.transform.position = Vector3.MoveTowards(rod.transform.position, rodMovePos, 0.03f);
                    if (rod.transform.position.y <= 0.86f || (rod.transform.position.y < 12 && FishCollection.rodLength == 1)) //if the rod is in the middle of the screen then both the rod and camera move to the bottom
                    {
                        float lastWallPos = walls[walls.Count - 1].transform.position.y + 0.75f;
                        camMovePos = new Vector3(cam.transform.position.x, (lastWallPos), cam.transform.position.z); //camera will move down to the last wall
                        rodMovePos = new Vector3(rod.transform.position.x, (walls[walls.Count - 1].gameObject.transform.Find("RodBottomLoc").position.y), rod.transform.position.z); //gets the local position of that walls rod bottom location game object - 11.26 as that's the walls relative position * by thr amount -1 (so if there's two walls it will only move down by 11.36)
                        rod.transform.position = Vector3.MoveTowards(rod.transform.position, rodMovePos, 0.03f);
                        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camMovePos, 0.03f);
                    }
                }
            }
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camMovePos, 0.05f);
        }

        //will be called when you have collected the desired amount of fish
        void RodUp()
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camMovePos, 0.07f);
            rod.transform.position = Vector3.MoveTowards(rod.transform.position, rodMovePos, 0.07f);

            if (!canFishSpawn)
            {
                //if the camera and rod are at the top of the screen
                if (cam.transform.position == camMovePos && rod.transform.position == rodMovePos)
                {
                    Cursor.visible = true;
                    foreach (var item in walls)
                    {
                        if (item != walls[0])
                        {
                            Destroy(item);
                        }
                    }
                    wallSpawnPos = new Vector3(wall.transform.position.x, wall.transform.position.y - 11.36f, wall.transform.position.z);
                    mainCanvas.SetActive(true);
                    sellFish = true;
                    fishCanvas.SetActive(false);
                }
            }
        }
    }
}
