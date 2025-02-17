using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Configuration")]
    [SerializeField] private string gameSceneName = "GameScene";
    
    [Header("Animation")]
    [SerializeField] private CanvasGroup mainMenuCanvas;

    private void Start()
    {
        // Asegurar que el cursor esté visible y libre
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.alpha = 1;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}