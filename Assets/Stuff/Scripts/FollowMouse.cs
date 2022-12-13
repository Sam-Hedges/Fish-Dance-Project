using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class FollowMouse : MonoBehaviour
    {
        float dist;
        float bottomOfScreen; 
        float topOfScreen;
        float topQuater;
        float bottomQuater;
        public float camBottomPos;  //bottom of screen is set in the StartFishing script

        float leftOfScreen; //these are set to just be the water area so if you click ouside the rod won't move to your position
        float rightOfScreen;

        GameObject cam;

        private void Start()
        {
            dist = (transform.position - Camera.main.transform.position).z;
            cam = Camera.main.gameObject;

            leftOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0.25f, 0, dist)).x;
            rightOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0.75f, 0, dist)).x;
        }
        void Update()
        {
            RodFollowMouse();
            CameraWithRod();
        }

        void RodFollowMouse()
        {
            if (Input.GetMouseButton(0))
            {
                //Vector3 is equal to the mousePosition, even though it's a vector3 it only get's the X and Y values.
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mousePosition.x > leftOfScreen && mousePosition.x < rightOfScreen && !PauseMenu.GamePaused)
                {
                    transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);
                }
            }
        }

        void CameraWithRod()
        {
            topOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;
            topQuater = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.75f, dist)).y;
            bottomOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
            bottomQuater = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.25f, dist)).y;

            //if the rod is within the top quater of the screen then move the camera up towards the overall top
            if (FishCollection.rodLength > 1)
            {
                if (this.gameObject.transform.position.y >= topQuater && this.gameObject.transform.position.y <= topOfScreen)
                {
                    Vector3 camMovePos = new Vector3(cam.transform.position.x, 1, cam.transform.position.z);
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, camMovePos, 0.07f);
                }

                if (this.gameObject.transform.position.y >= bottomOfScreen && this.gameObject.transform.position.y <= bottomQuater)
                {
                    Vector3 camMovePos = new Vector3(cam.transform.position.x, camBottomPos, cam.transform.position.z);
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, camMovePos, 0.07f);
                }
            }

            if(this.transform.position.y <= bottomQuater)
            {
                Vector3 newPos = new Vector3(this.transform.position.x, bottomQuater - 0.1f, this.transform.position.z);
                this.transform.position = newPos;
            }
            if (this.transform.position.y >= topQuater)
            {
                Vector3 newPos = new Vector3(this.transform.position.x, topQuater + 0.1f, this.transform.position.z);
                this.transform.position = newPos;
            }
        }
    }
}
