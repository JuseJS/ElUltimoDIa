using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("End Game Screen")]
    [SerializeField] private EndGameScreen endGameScreen;

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 1200f; // 20 minutos en segundos
    [SerializeField] private MissionData missionData; // Referencia a las misiones del juego
    
    public GameState CurrentGameState { get; private set; }
    public float RemainingTime { get; private set; }
    public event Action<GameState> OnGameStateChanged;
    public event Action<float> OnTimeUpdated;

    private int totalMissions = 6; // Total de misiones en el juego
    private int completedMissions = 0; // Contador de misiones completadas

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
        
        // Calcular estadísticas
        string timeLeft = FormatTimeLeft(RemainingTime);
        string missionsCompleted = $"{completedMissions}/{totalMissions}";
        
        // Configurar y mostrar la pantalla
        endGameScreen.SetupButtons(
            () => RestartGame(),
            () => ReturnToMenu()
        );
        
        endGameScreen.Show(victory, timeLeft, missionsCompleted);
    }

    private string FormatTimeLeft(float timeInSeconds)
    {
        timeInSeconds = Mathf.Max(0, timeInSeconds); // Asegurarse de que no sea negativo
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void IncrementCompletedMissions()
    {
        completedMissions++;
        // Opcional: Verificar victoria si se completaron todas las misiones
        if (completedMissions >= totalMissions)
        {
            GameOver(true);
        }
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
        endGameScreen.Hide();
        InitializeGame();
        // Aquí puedes añadir lógica adicional de reinicio
        // como recargar la escena o resetear otros componentes
    }
    
    private void ReturnToMenu()
    {
        endGameScreen.Hide();
        // Aquí puedes añadir la lógica para volver al menú principal
        // Por ejemplo:
        // SceneManager.LoadScene("MainMenu");
    }
}