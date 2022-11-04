using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class FishMove : MonoBehaviour
    {
        public Vector3 reachedLocation;

        float moveTowards;

        float speed;

        private void Awake()
        {
            if(this.transform.position.x < -7.37f && this.transform.position.x > -11.66f)
            {
                moveTowards = 9.6f;
            }

            else if (this.transform.position.x < 9.6f && this.transform.position.x > 6.25f)
            {
                moveTowards = -11.66f;
            }
            reachedLocation = new Vector3(moveTowards, this.transform.position.y, this.transform.position.z);
            speed = Random.Range(0.02f, 0.08f);
        }

        private void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.reachedLocation, this.speed);

            if(this.transform.position == reachedLocation)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
