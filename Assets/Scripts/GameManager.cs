using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("End Game Screen")]
    [SerializeField] private EndGameScreen endGameScreen;

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 1200f;
    [SerializeField] private MissionData missionData;
    
    public GameState CurrentGameState { get; private set; }
    public float RemainingTime { get; private set; }
    public event Action<GameState> OnGameStateChanged;
    public event Action<float> OnTimeUpdated;

    private int completedMissions = 0;

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
        completedMissions = 0;
        ChangeGameState(GameState.Playing);
        
        // Suscribirse al evento de misión completada
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnMissionCompleted += HandleMissionCompleted;
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse de eventos al destruir
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnMissionCompleted -= HandleMissionCompleted;
        }
    }

    private void HandleMissionCompleted(Mission mission)
    {
        completedMissions++;
        Debug.Log($"Misión completada. Total: {completedMissions}");
        
        if (mission.nextMission == null)
        {
            GameOver(true);
        }
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
        
        string timeLeft = FormatTimeLeft(RemainingTime);
        string missionsCompleted = $"{completedMissions}/6";
        
        endGameScreen.SetupButtons(
            () => RestartGame(),
            () => ReturnToMenu()
        );
        
        endGameScreen.Show(victory, timeLeft, missionsCompleted);
    }

    private string FormatTimeLeft(float timeInSeconds)
    {
        timeInSeconds = Mathf.Max(0, timeInSeconds);
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
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

    private void RestartGame()
    {
        // Asegurarse de que el tiempo esté normalizado
        Time.timeScale = 1f;
        
        // Ocultar la pantalla de fin de juego
        if (endGameScreen != null)
        {
            endGameScreen.Hide();
        }
        
        // Restablecer el estado del juego
        RemainingTime = gameDuration;
        completedMissions = 0;
        ChangeGameState(GameState.Playing);
        
        // Reiniciar el KeySearchManager
        if (KeySearchManager.Instance != null)
        {
            Destroy(KeySearchManager.Instance.gameObject);
        }
        
        // Reiniciar el MissionManager
        if (MissionManager.Instance != null)
        {
            Destroy(MissionManager.Instance.gameObject);
        }
        
        // Recargar la escena actual
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        if (endGameScreen != null)
        {
            endGameScreen.Hide();
        }
        //SceneManager.LoadScene("MainMenu");
    }
}