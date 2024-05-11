using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public InputActionAsset inputActions;
    private Canvas pauseUI;
    private InputAction menu;
    public GameStats gameStats;

    // Start is called before the first frame update
    void Start()
    {
        pauseUI = GameObject.FindWithTag("PauseMenu").GetComponent<Canvas>();
        pauseUI.enabled = false; // Disable the menu initially
        menu = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
        menu.Enable();
        menu.performed += MenuToggle;
    }

    public void OnDestroy()
    {
        menu.performed -= MenuToggle;
    }

    public void MenuToggle(InputAction.CallbackContext context)
    {
        if (!gameStats.gameover)
        {
            pauseUI.enabled = !pauseUI.enabled;

            if (pauseUI.enabled)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        else 
        {
            Time.timeScale = 1f;
        }
    }

    public void Return()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }

    // Exit the game
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
