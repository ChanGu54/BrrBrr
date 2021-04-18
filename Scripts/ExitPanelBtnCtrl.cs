using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPanelBtnCtrl : MonoBehaviour
{
    public void LoadBtnClicked()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartBtnClicked()
    {
        Debug.Log("Restart!");
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenuBtnClicked()
    {
        Debug.Log("MainMenu");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
