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

        float rightOfScreen;
        float leftOfScreen;

        private void Awake()
        {

            var dist = (transform.position - Camera.main.transform.position).z;
            leftOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
            rightOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;


            if (this.transform.position.x == leftOfScreen)
            {
                moveTowards = rightOfScreen;
            }
            else if (this.transform.position.x == rightOfScreen)
            {
                moveTowards = leftOfScreen;
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
