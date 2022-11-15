using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class WaterScrollEffect : MonoBehaviour
    {
        public float ySpeed = 0.2f;

        // Update is called once per frame
        void Update()
        {
            float yOffset = Time.time * ySpeed;

            this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, yOffset);
        }
    }
}
