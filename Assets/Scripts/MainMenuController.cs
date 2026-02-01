using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject settingsPanel; //
    public GameObject menuPanel;     
    public GameObject narratorPanel; 

    // 1. Start ：
    public void ShowNarrator()
    {
        menuPanel.SetActive(false);     
        narratorPanel.SetActive(true);  
    }
    public void FinalStartGame()
    {
        SceneManager.LoadScene("Level 1"); //
    }

    // 2. Settings 
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // 3. Quit 
    public void QuitGame()
    {
        UnityEngine.Debug.Log("Game Exited");

    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}