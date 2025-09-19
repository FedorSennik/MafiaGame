using UnityEngine;
using UnityEngine.UI;

public class PickUpSlider : MonoBehaviour
{
    public Slider pickUpSkider;
    public static PickUpSlider instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        pickUpSkider = GetComponent<Slider>();
    }
} 