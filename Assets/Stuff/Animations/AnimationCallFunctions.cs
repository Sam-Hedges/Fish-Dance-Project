using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class AnimationCallFunctions : MonoBehaviour
    {
        public StartFishing startFishing;
        Vector3 rodMovePos;
        public GameObject rod;
        public void StartFishing() //called on the animation frame when the rod is back in the water
        {
            rod.SetActive(false);
            startFishing.SpawnWalls();
        }

        private void Start()
        {
            rodMovePos = new Vector3(-6.47f, rod.transform.localPosition.y, rod.transform.localPosition.z);
        }
        private void Update()
        {
            rod.transform.localPosition = Vector3.MoveTowards(rod.transform.localPosition, rodMovePos, 0.01f);
        }
        public void RodMoveUp() //these are also called within the rod cast animations
        {
            rodMovePos = new Vector3(-0.47f, rod.transform.localPosition.y, rod.transform.localPosition.z);
        }

        public void RodMoveDown()
        {
            rodMovePos = new Vector3(-6.47f, rod.transform.localPosition.y, rod.transform.localPosition.z);
        }
    }
}
