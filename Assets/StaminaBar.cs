using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    private void Start()
    {
        //slider = GetComponent<Slider>();
    }
    public void SetMaxStamina(int maxStamina)
    {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
    }
    public void SetCurrentHealth(int currStamina)
    {
        slider.value = currStamina;
    }
}
