using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionSlider : MonoBehaviour {

    public Slider MainSlider;

    float slideIncrease = .001f;

	void Start () {
        MainSlider.value = .05f;
        MainSlider.maxValue = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        MainSlider.value = MainSlider.value + slideIncrease;
	}
}
