using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TMP_Text textHp;
    public int healthPoints;
    // Start is called before the first frame update
    void Start()
    {
        textHp.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        textHp.text = "Health: " + healthPoints.ToString();
    }
}
