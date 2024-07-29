using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    public static EnemyHealthBar instance;
    Slider slider;
    public TextMeshProUGUI healthNumber;
    [SerializeField] Image fillSlider;
    [SerializeField] Gradient healthColors;
    public bool healthBarIsActive = false;
    public Enemy enemyHPToShow;
    


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        gameObject.SetActive(false);
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (healthBarIsActive)
        {
            UpdateHealthBar();
        }
    }

    public void ShowHealthBar(Enemy enemyPressed)
    {
        healthBarIsActive = true;
        gameObject.SetActive(true);
        enemyHPToShow = enemyPressed;
    }

    public void DeactivateHealthBar()
    {
        healthBarIsActive = false;
        gameObject.SetActive(false);
    }

    //this should only be called when it takes damage, and should take a param but this is just testing
    public void UpdateHealthBar()
    {
        slider.value = enemyHPToShow.currentHP / enemyHPToShow.enemySO.MaxHP;
        healthNumber.text = enemyHPToShow.currentHP + " " + "/" + " " + enemyHPToShow.enemySO.MaxHP;
        fillSlider.color = healthColors.Evaluate(slider.value);
    }
}
