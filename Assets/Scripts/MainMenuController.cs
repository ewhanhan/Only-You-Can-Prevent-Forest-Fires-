using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
  public GameObject startMenuUI;
  public GameObject instructionsUI;
  public GameObject creditsUI;
  public GameObject[] menuUIArray;

  void Start()
  {
    ActivateStartMenu();
  }

  public void StartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void QuitGame()
  {
    Application.Quit();
  }

  public void ActivateInstructionsMenu()
  {
    ActivateOnlySpecificMenu(instructionsUI);
  }

  public void ActivateStartMenu()
  {
    ActivateOnlySpecificMenu(startMenuUI);
  }

  public void ActivateCreditMenu()
  {
    ActivateOnlySpecificMenu(creditsUI);
  }

  private void ActivateOnlySpecificMenu(GameObject menuUI)
  {
    foreach (var menuUIObj in menuUIArray)
    {
      if (menuUIObj == menuUI)
      {
        menuUI.SetActive(true);
      }
      else
      {
        menuUIObj.SetActive(false);
      }
    }
  }
}