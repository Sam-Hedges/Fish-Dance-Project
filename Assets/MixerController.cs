using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PortfolioProject
{
    public class MixerController : MonoBehaviour
    {
        // mixer references
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private AudioMixer sfxMixer;
        [SerializeField] private AudioMixer musicMixer;
        // slider references
        public Slider sfxSlider;
        public Slider musicSlider;
        // bool for sticking the sliders together
        private bool stick = false;

        public void SetVolumeMaster(float sliderValue)
        {
            // sets the volume to the sliders value
            masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
            // These if statements check if the other sliders are equal to or above master to prevent a lowered slider from snapping back up to the masters volume, the stick bool checks makes the other sliders stay with the master slider when they collide
            if (sfxSlider.value >= sliderValue || stick == true)
            {
                sfxSlider.value = sliderValue;
                stick = true;
            }
            if (musicSlider.value >= sliderValue || stick == true)
            {
                musicSlider.value = sliderValue;
                stick = true;
            }
        }

        public void SetVolumeSFX(float sliderValue)
        {
            // resets the stick variable
            stick = false;
            // sets the volume to the sliders value
            sfxMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        }

        public void SetVolumeMusic(float sliderValue)
        {
            //resets the stick variable
            stick = false;
            // sets the volume to the sliders value
            musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        }
    }
}
