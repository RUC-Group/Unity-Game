using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TMP_Text textHp;
    public TMP_Text textScore;
    public RawImage key1;
    public RawImage key2;
    public Slider mSlider;

    public int currHealth;
    public int score;

    // Update is called once per frame
    void Update(){
        mSlider.value = currHealth;
        textHp.text = "Health: " + currHealth.ToString();
        textScore.text = "Score: " + score.ToString();
    }
}
