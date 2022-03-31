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
  public FireManager fireManager;
  public List<Vector3Int> fireBaseSlidersLocations = new List<Vector3Int>();
  public List<Slider> fireBaseSlidersList = new List<Slider>(14);

  public static bool IsEndGame = false;
  public static bool IsGamePaused = false;

  void Start()
  {
    // Populate coordinates of sliders
    fireBaseSlidersLocations.Add(new Vector3Int(-305, -230, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(-305, 50, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(-210, 330, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(-160, 190, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(-70, -140, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(-25, -280, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(-25, 0, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(25, 280, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(70, 470, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(165, -90, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(210, -230, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(210, 280, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(305, 50, 0));
    fireBaseSlidersLocations.Add(new Vector3Int(350, -140, 0));

    Debug.Log(fireBaseSlidersLocations.Count);
    for(var i = 0; i < fireBaseSlidersLocations.Count; i++){
      fireBaseSlidersList.Add(fireBaseSlider);
    }

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

  public void InstantiateFireHealth(Vector3Int localPlace){
    for(var i = 0; i < fireManager.allAvailableFireSpots.Count; i++){
      if(fireManager.allAvailableFireSpots[i] == localPlace){
        Slider instantiatedSlider = Instantiate(fireBaseSlider, fireBaseSlidersLocations[i], Quaternion.identity);
        instantiatedSlider.transform.SetParent(HUD.transform, false);
        fireBaseSlidersList.Insert(i, instantiatedSlider);
        StartSingleFireTimer(instantiatedSlider);
      }
    }
  }

  public void DeleteFireSlider(Vector3Int localPlace){
    for(var i = 0; i < fireManager.allAvailableFireSpots.Count; i++){
      if(fireManager.allAvailableFireSpots[i] == localPlace){
        // Stop the Couroutine
        
        // Destroy object.
        Destroy(fireBaseSlidersList[i].gameObject);
        // Remove from list.
        fireBaseSlidersList.RemoveAt(i);
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