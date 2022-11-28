using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class WaterScrollEffect : MonoBehaviour
    {
        public float ySpeed;
        public StartFishing startFishing;

        // Update is called once per frame
        void Update()
        {
            ySpeed = 0.11f;
            float yOffset = Time.time * ySpeed;
            this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, yOffset);
        }
    }
}
