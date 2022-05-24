using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameManager : MonoBehaviour
{
    public static UIGameManager instance { get; private set; }
    [SerializeField] TMP_Text scoreText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        Score();
    }

    void Score()
    {
        scoreText.text = "Score: " + GameManager.instance.score.ToString();
    }
}
