using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//GameUI used in the UI canvas in unity
public class GameUI : MonoBehaviour{
    public TMP_Text textHp;
    public TMP_Text textScore;
    public RawImage key1;
    public RawImage key2;
    public Slider healthSlider;
    public Slider staminaSlider;

    public int currHealth;
    public int stamina;
    public int score;

    // Update is called once per frame gets the players current stamina, health and gold amount 
    void Update(){
        healthSlider.value = currHealth;
        staminaSlider.value = stamina;
        textHp.text = "Health: " + currHealth.ToString();
        textScore.text = "Gold: " + score.ToString();
    }
}
