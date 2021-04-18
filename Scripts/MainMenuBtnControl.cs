using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBtnControl : MonoBehaviour
{
    public GameObject exitPanel;

    public void OnStartBtnClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnGarageBtnClicked()
    {
        SceneManager.LoadScene("Garage");
    }

    public void OnExitBtnClicked()
    {
        exitPanel.SetActive(true);
    }

    public void OnExitYesBtnClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnExitNoBtnClicked()
    {
        exitPanel.SetActive(false);
    }
}
