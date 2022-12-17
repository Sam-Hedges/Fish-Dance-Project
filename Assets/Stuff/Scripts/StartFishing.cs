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
        public GameObject rope;
        public GameObject rodDisplay;
        GameObject cam;
        public GameObject fishCanvas;
        public GameObject mainCanvas;
        public GameObject minusGoldDisplay;

        public GameObject wall;
        GameObject newWall;

        Vector3 camMovePos;
        Vector3 rodMovePos;

        public static bool canFishSpawn = false;

        Vector3 wallSpawnPos;

        public List<GameObject> walls = new List<GameObject>();

        public static bool startedFishing;

        public static bool sellFish = false;

        public AudioSource Music;
        public AudioClip mainMusic;

        void Start()
        {
            cam = Camera.main.gameObject;

            walls.Add(wall);
            wallSpawnPos = new Vector3(wall.transform.position.x, wall.transform.position.y - 11.36f, wall.transform.position.z);
        }

        private void Update()
        {
            TestRodLocation();

            if(fishCollection.currentFishAmount == FishCollection.fishAmount || fishCollection.hitJellyFish)
            {
                rod.GetComponent<FollowMouse>().enabled = false;
                canFishSpawn = false;
                startedFishing = false;
                rodMovePos = new Vector3(rod.transform.position.x, 9.15f, rod.transform.position.z);
                camMovePos = new Vector3(cam.transform.position.x, 12.79f, cam.transform.position.z);
                if (!PauseMenu.GamePaused)
                {
                    RodUp();
                }
            }

            if(!canFishSpawn)
            {
                foreach (var item in fishSpawning.fishAmount)
                {
                    Destroy(item);
                }
            }
        }

        void TestRodLocation()
        {
            if (startedFishing && !canFishSpawn)
            {
                if (!PauseMenu.GamePaused)
                {
                    RodDown();
                } 

                if (rod.transform.position.y == (walls[walls.Count - 1].gameObject.transform.Find("RodBottomLoc").position.y))
                {
                    rod.GetComponent<FollowMouse>().enabled = true;
                    rod.GetComponent<FollowMouse>().camBottomPos = camMovePos.y; //at this stage the rod and the camera will be at the bottom of all the walls
                    canFishSpawn = true;
                }
            }
        }

        public void SpawnWalls()
        {
            walls.Clear();
            walls.Add(wall);
            fishCollection.currentFishAmount = 0;
            fishCollection.fishLeft = FishCollection.fishAmount;
            fishCollection.fishLeftDisplay.text = FishCollection.fishAmount.ToString();
            fishCanvas.SetActive(true);
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
                    foreach (Transform child in rod.transform) //sets each part of the rod to be visible
                    {
                        child.GetComponent<MeshRenderer>().enabled = true;
                    }
                    rod.transform.position = Vector3.MoveTowards(rod.transform.position, rodMovePos, 0.03f);
                    if (rod.transform.position.y <= 0.86f || (rod.transform.position.y < 12 && FishCollection.rodLength == 1)) //if the rod is in the middle of the screen then both the rod and camera move to the bottom
                    {
                        float lastWallPos = walls[walls.Count - 1].transform.position.y + 0.75f;
                        camMovePos = new Vector3(cam.transform.position.x, (lastWallPos), cam.transform.position.z); //camera will move down to the last wall
                        rodMovePos = new Vector3(rod.transform.position.x, (walls[walls.Count - 1].gameObject.transform.Find("RodBottomLoc").position.y), rod.transform.position.z); //gets the local position of that walls rod bottom location game object - 11.26 as that's the walls relative position * by thr amount -1 (so if there's two walls it will only move down by 11.36)
                        rod.transform.position = Vector3.MoveTowards(rod.transform.position, rodMovePos, 0.1f);
                        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camMovePos, 0.1f);
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
                    fishSpawning.fishAmount.Clear();
                    minusGoldDisplay.SetActive(false);
                    fishCanvas.SetActive(false);
                    Music.clip = mainMusic;
                    fishCollection.hitJellyFish = false;
                    foreach (Transform child in rod.transform) //sets each part of the rod to be invisible
                    {
                        child.GetComponent<MeshRenderer>().enabled = false;
                    }
                    rodDisplay.SetActive(true);
                }
            }
        }
    }
}
