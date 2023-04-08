using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public PlayerMovementTutorial playerMana;
    public Image fillImage;
    private Slider slider;
    private float fillValue;
    // Start is called before the first frame update
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Fixes Mana bar not completely dissapearing when at 0
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        fillValue = playerMana.currentMana / playerMana.maxMana;

        if (fillValue <= slider.maxValue / 3) // Changes Mana bar color to red when below 1/3 max Mana
        {
            fillImage.color = Color.red;
        }
        else if (fillValue > slider.maxValue / 3) // Changes Mana bar color to green when above 1/3 max Mana
        {
            fillImage.color = Color.green;
        }
        slider.value = fillValue;
    }
}
