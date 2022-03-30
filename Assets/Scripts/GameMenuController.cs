using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
  public GameObject pauseDisplay;
  public GameObject endGameDisplay;
  public GameObject controlsDisplay;
  public GameObject objectivesDisplay;
  public GameObject HUD;
  public GameObject[] _UIArr;
  public Slider fireBaseSlider;
  public List<Slider> fireBaseSlidersList;

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
      IsGamePaused = !IsGamePaused;
      PauseGame();
    }
  }

  public void InstantiateFireHealth(Vector3 localPlace){
    Slider instantiatedSlider = Instantiate(fireBaseSlider, localPlace, Quaternion.identity);
    instantiatedSlider.transform.SetParent(HUD.transform, false);
    fireBaseSlidersList.Add(instantiatedSlider);
    StartSingleFireTimer(instantiatedSlider);
  }

  public void DeleteFireSlider(Vector3Int localPlace){
    foreach(Slider fireSlider in fireBaseSlidersList){
      if(fireSlider.transform.position == localPlace){
        // Stop the Couroutine
        
        // Destroy object.
        Destroy(fireSlider.gameObject);
        // Remove from list.
        fireBaseSlidersList.Remove(fireSlider);
      }
    }
  }

  void StartSingleFireTimer(Slider fireSlider){
    StartCoroutine(DecreseFireBaseSlider(fireSlider));
  }

  void StartAllFireTimers(){
    foreach(Slider fireSlider in fireBaseSlidersList){
      StartCoroutine(DecreseFireBaseSlider(fireSlider));
    }
  }

  public void ResumeGame()
  {
    ResumeTime();
    IsGamePaused = false;
    ActivateOnlySpecificMenu(HUD);
    StartAllFireTimers();
  }

  void PauseGame()
  {
    if (IsGamePaused)
    {
      StopTime();
      StopAllCoroutines();
      ActivateOnlySpecificMenu(pauseDisplay);
    }
    else
    {
      ResumeGame();
    }
  }

  public void EndGame()
  {
    StopTime();
    ActivateOnlySpecificMenu(endGameDisplay);
  }

  private void ResumeTime()
  {
    Time.timeScale = 1;
  }

  public void StopTime()
  {
    Time.timeScale = 0;
  }

  public void ActivateOnlySpecificMenu(GameObject specificUI)
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

  public void ObjectivesMenu()
  {
    ActivateOnlySpecificMenu(objectivesDisplay);
  }

  public void ControlsMenu()
  {
    ActivateOnlySpecificMenu(controlsDisplay);
  }

  public void ReturnPauseMenu()
  {
    ActivateOnlySpecificMenu(pauseDisplay);
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

  public IEnumerator DecreseFireBaseSlider(Slider slider){
    float timeSlice = (slider.value / 30);
    while (slider.value >= 0){
        slider.value -= timeSlice;
        yield return new WaitForSeconds(1);
        if (slider.value <= 0){
            EndGame();
            break;
        }
    }
  }
}