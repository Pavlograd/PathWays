using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGame : MonoBehaviour
{
    [SerializeField] Pause pause;
    [SerializeField] TMP_Text textScore;

    public void Replay()
    {
        pause.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoHome()
    {
        pause.LoadScene("Game");
    }

    public void Init(int score)
    {
        textScore.text = "Your final score: " + score.ToString();
    }
}