using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Configuration")]
    [SerializeField] private string gameSceneName = "GameScene";
    
    [Header("Menu References")]
    [SerializeField] private MainMenuAnimations menuAnimations;
    [SerializeField] private Canvas mainMenuCanvas;

    private void Start()
    {
        // Asegurar que el cursor esté visible y libre en el menú
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Verificar que la escena existe
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("Game scene name is not set in MenuManager!");
        }

        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.enabled = true;
        }

        if (menuAnimations != null)
        {
            menuAnimations.InitializeAnimations();
        }
    }

    public void StartGame()
    {
        Debug.Log("Starting game...");
        
        if (menuAnimations != null)
        {
            menuAnimations.enabled = false;
        }

        // Nos aseguramos de destruir este MenuManager al cambiar de escena
        Destroy(gameObject);
        
        // Cargamos la escena
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}