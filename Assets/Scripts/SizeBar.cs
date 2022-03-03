using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeBar : MonoBehaviour
{
    private Slider slider;
    
    private void Start() {
        slider = GetComponent<Slider>();
    }

    public void SetMaxSize(float max){
        slider.maxValue = max;
        slider.value = max;
    }
    public void SetSize(float size){
        slider.value = size;
    }
}
