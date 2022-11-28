using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class StartFishing : MonoBehaviour
    {
        public FishSpawning fishSpawning;
        public GameObject rod;

        Vector3 screenBottom;

        public bool canFishSpawn = false;

        private void Update()
        {
            if(!canFishSpawn)
            {
                RodDown();

                if(rod.transform.position == screenBottom)
                {
                    fishSpawning.StartCoroutine(fishSpawning.WaitForFish());
                    canFishSpawn = true;
                }
            }
        }
        void Start()
        {
            RodDown();
        }

        void RodDown()
        {
            screenBottom = new Vector3(-0.13f, 5.72f, -3.44f);
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
