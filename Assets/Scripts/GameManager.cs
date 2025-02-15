using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 1200f; // 20 minutos en segundos
    
    public GameState CurrentGameState { get; private set; }
    public float RemainingTime { get; private set; }
    public event Action<GameState> OnGameStateChanged;
    public event Action<float> OnTimeUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        RemainingTime = gameDuration;
        ChangeGameState(GameState.Playing);
    }

    private void Update()
    {
        if (CurrentGameState == GameState.Playing)
        {
            UpdateGameTime();
        }
    }

    private void UpdateGameTime()
    {
        RemainingTime -= Time.deltaTime;
        OnTimeUpdated?.Invoke(RemainingTime);

        if (RemainingTime <= 0)
        {
            GameOver(false);
        }
    }

    public void GameOver(bool victory)
    {
        ChangeGameState(victory ? GameState.Victory : GameState.GameOver);
        UIManager.Instance.ShowMessage(victory ? "¡Has completado la entrega a tiempo!" : "¡Se acabó el tiempo!", !victory);
    }

    public void PauseGame()
    {
        if (CurrentGameState == GameState.Playing)
        {
            ChangeGameState(GameState.Paused);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (CurrentGameState == GameState.Paused)
        {
            ChangeGameState(GameState.Playing);
            Time.timeScale = 1f;
        }
    }

    private void ChangeGameState(GameState newState)
    {
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}