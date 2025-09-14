using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlicerStatic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        OBJECTPICKUP.pickupSlider = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
