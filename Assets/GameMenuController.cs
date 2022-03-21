using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
  public GameObject pauseDisplay;
  public GameObject endGameDisplay;
  public GameObject HUD;
  public GameObject[] _UIArr;

  public static bool IsEndGame = false;
  public static bool IsGamePaused = false;

  void Start()
  {
    IsEndGame = false;
    ResumeTime();
    ActivateOnlySpecificMenu(HUD);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape) && !IsEndGame)
    {
      IsGamePaused = true;
      PauseGame();
    }

    if (Input.GetMouseButtonDown(0))
    {
      Debug.Log("test");
    }
  }

  public void ResumeGame()
  {
    Debug.Log("test");
    ResumeTime();
    IsGamePaused = false;
    ActivateOnlySpecificMenu(HUD);
  }

  void PauseGame()
  {
    StopTime();
    ActivateOnlySpecificMenu(pauseDisplay);
  }

  void EndGame()
  {
    StopTime();
    ActivateOnlySpecificMenu(endGameDisplay);
  }

  private void ResumeTime()
  {
    Time.timeScale = 1;
  }

  private void StopTime()
  {
    Time.timeScale = 0;
  }

  private void ActivateOnlySpecificMenu(GameObject specificUI)
  {
    foreach (var UI in _UIArr)
    {
      if (UI == specificUI)
      {
        specificUI.SetActive(true);
      }
      else
      {
        UI.SetActive(false);
      }
    }
  }

  public void ReturnStartMenu()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
  }

  public void QuitGame()
  {
    Application.Quit();
    if (EditorApplication.isPlaying)
    {
      EditorApplication.ExitPlaymode();
    }
  }

  public void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}