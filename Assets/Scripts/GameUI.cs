using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TMP_Text textHp;
    public TMP_Text textScore;
    public int healthPoints;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        textHp.text = "Health: " + healthPoints.ToString();
        textScore.text = "Score: " + score.ToString();
    }
}
