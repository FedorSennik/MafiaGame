using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float Health = 100;
    private float HealthC = 0;
    public void HealthChange(float _healthchange)
    {
        HealthC = _healthchange + Health;
        if(HealthC > 100)
        {
            Health = 100;
        }
        else if (HealthC <= 100)
        {
            Health = HealthC;
        }
        else if(HealthC <= 0) 
        {
            Health = 0;
        }
        else
        {
            Health = 0;
            Debug.Log("Health error. Health = 0");
        }

    }
}
