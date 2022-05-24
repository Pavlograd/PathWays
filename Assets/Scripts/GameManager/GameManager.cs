using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public float size;
    public PlayerData playerData;
    public int score = 0;
    bool gameOver = false;
    [SerializeField] EndGame endGame;
    [SerializeField] Pause pause;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        size = playerData.startSize;
    }

    void Update()
    {
        size += playerData.speed * Time.deltaTime;
    }

    public void IncreaseScore()
    {
        if (!gameOver) score++;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        Pause.PauseGame(); // Pause game
        gameOver = true; // set value for other scripts
    }

    public void End()
    {
        Debug.Log("End");
        Pause.PauseGame(); // Prevent further score
        Debug.Log(score); // Final score

        endGame.gameObject.SetActive(true); // UI for lose/win
        endGame.Init(score);
    }
}
