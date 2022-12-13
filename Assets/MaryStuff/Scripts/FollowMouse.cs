using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioProject
{
    public class FollowMouse : MonoBehaviour
    {
        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                //Vector3 is equal to the mousePosition, even though it's a vector3 it only get's the X and Y values.
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);
            }
        }
    }
}
