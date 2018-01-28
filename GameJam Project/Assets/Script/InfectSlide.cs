using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectSlide : MonoBehaviour
{

    public Slider mainSlider;

    float sliderIncrease = .001f;

    void Start()
    {
        mainSlider.value = .01f;
        mainSlider.maxValue = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        mainSlider.value += sliderIncrease;
    }
}