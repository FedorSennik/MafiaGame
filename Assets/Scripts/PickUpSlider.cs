using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpSlider : MonoBehaviour
{
    public Slider pickUpSkider;
    public static PickUpSlider instance;

    private void Start()
    {
        pickUpSkider = gameObject.GetComponent<Slider>();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
