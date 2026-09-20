using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    public Slider healthBar;
    public PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.maxValue = player.maxHP;
        healthBar.value = player.currentHP;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = player.currentHP;
    }
}
