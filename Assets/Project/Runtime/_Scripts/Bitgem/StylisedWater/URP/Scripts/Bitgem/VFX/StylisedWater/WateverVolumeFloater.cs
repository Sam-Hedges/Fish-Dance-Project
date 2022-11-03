#region Using statements

using System.Collections;
using System.Collections.Generic;
using PortfolioProject.Bitgem.StylisedWater.URP.Scripts.Bitgem.VFX.StylisedWater;
using UnityEngine;

#endregion


    public class WateverVolumeFloater : MonoBehaviour
    {
        #region Public fields

        public WaterVolumeHelper WaterVolumeHelper = null;

        #endregion

        #region MonoBehaviour events

        void Update()
        {
            var instance = WaterVolumeHelper ? WaterVolumeHelper : WaterVolumeHelper.Instance;
            if (!instance)
            {
                return;
            }

            transform.position = new Vector3(transform.position.x, instance.GetHeight(transform.position) ?? transform.position.y, transform.position.z);
        }

        #endregion
    }
