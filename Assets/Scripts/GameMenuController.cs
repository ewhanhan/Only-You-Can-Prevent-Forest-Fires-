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
  public CamperManager camperManager;
  public List<Vector3Int> fireBaseSlidersLocations = new List<Vector3Int>();
  public List<Slider> fireBaseSlidersList = new List<Slider>();
  public List<Slider> camperBaseSlidersList = new List<Slider>();
  Coroutine[] MyFireCoroutines = new Coroutine[14];
  Coroutine[] MyCamperCoroutines = new Coroutine[14];
  private List<int> activeFireSliders = new List<int>();
  private List<int> activeCamperSliders = new List<int>();

  public static bool IsEndGame = false;
  public static bool IsGamePaused = false;
  public int healthDecreaseTime = 30; // seconds it takes for health bars to reach 0

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

    IsEndGame = false;
    ResumeTime();
    ActivateOnlySpecificMenu(HUD);
    Coroutine healthTimerDecrease = StartCoroutine(DecreseHealthTimer());
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
        fireBaseSlidersList[i].gameObject.SetActive(true);
        activeFireSliders.Add(i);
        StartSingleFireTimer(fireBaseSlidersList[i], i);
      }
    }
  }

  public void InstantiateCamperHealth(Vector3Int localPlace){
    for(var i = 0; i < camperManager.allAvailableCamperSpots.Count; i++){
      if(camperManager.allAvailableCamperSpots[i] == localPlace){
        camperBaseSlidersList[i].gameObject.SetActive(true);
        activeCamperSliders.Add(i);
        StartSingleCamperTimer(camperBaseSlidersList[i], i);
      }
    }
  }

  public void DeleteFireSlider(Vector3Int localPlace){
    for(var i = 0; i < fireManager.allAvailableFireSpots.Count; i++){
      if(fireManager.allAvailableFireSpots[i] == localPlace){
        // Stop the Couroutine
        StopCoroutine(MyFireCoroutines[i]);
        fireBaseSlidersList[i].gameObject.SetActive(false);
        fireBaseSlidersList[i].value = 1;
        activeFireSliders.Remove(i);      
      }
    }
  }

   public void DeleteCamperSlider(Vector3Int localPlace){
    for(var i = 0; i < camperManager.allAvailableCamperSpots.Count; i++){
      if(camperManager.allAvailableCamperSpots[i] == localPlace){
        // Stop the Couroutine
        StopCoroutine(MyCamperCoroutines[i]);
        camperBaseSlidersList[i].gameObject.SetActive(false);
        camperBaseSlidersList[i].value = 1;  
        activeCamperSliders.Remove(i);      
      }
    }
  }

  void StartSingleFireTimer(Slider fireSlider, int i){
     MyFireCoroutines[i] = StartCoroutine(DecreseHealthSliders(fireSlider));
  }

  void StartAllFireTimers(){
    Debug.Log(activeFireSliders.Count);
    for(var i = 0; i < activeFireSliders.Count; i++){
      fireBaseSlidersList[activeFireSliders[i]].gameObject.SetActive(true);
      MyFireCoroutines[activeFireSliders[i]] = StartCoroutine(DecreseHealthSliders(fireBaseSlidersList[activeFireSliders[i]]));
    }
  }

  void PauseAllFireTimers(){
    for(var i = 0; i < fireBaseSlidersList.Count; i++){
      fireBaseSlidersList[i].gameObject.SetActive(false);
    }
  }

  void StartSingleCamperTimer(Slider camperSlider, int i){
    MyCamperCoroutines[i] = StartCoroutine(DecreseHealthSliders(camperSlider));
  }

  void StartAllCamperTimers(){
    for(var i = 0; i < activeCamperSliders.Count; i++){
      camperBaseSlidersList[activeCamperSliders[i]].gameObject.SetActive(true);
      MyCamperCoroutines[activeCamperSliders[i]] = StartCoroutine(DecreseHealthSliders(camperBaseSlidersList[i]));
    }
  }

  void PauseAllCamperTimers(){
    for(var i = 0; i < camperBaseSlidersList.Count; i++){
      camperBaseSlidersList[i].gameObject.SetActive(false);
    }
  }

  public void ResumeGame()
  {
    ResumeTime();
    IsGamePaused = false;
    ActivateOnlySpecificMenu(HUD);
    StartAllFireTimers();
    StartAllCamperTimers();
  }

  void PauseGame()
  {
    if (IsGamePaused)
    {
      StopTime();
      StopAllCoroutines();
      ActivateOnlySpecificMenu(pauseDisplay);
      PauseAllFireTimers();
      PauseAllCamperTimers();
    }
    else
    {
      ResumeGame();
    }
  }

  public void EndGame()
  {
    StopTime();
    for(var i = 0; i < fireBaseSlidersList.Count; i++){
      fireBaseSlidersList[i].gameObject.SetActive(false);
      fireBaseSlidersList[i].value = 1;
    }
    for(var i = 0; i < camperBaseSlidersList.Count; i++){
      camperBaseSlidersList[i].gameObject.SetActive(false);
      camperBaseSlidersList[i].value = 1;
    }
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

  public IEnumerator DecreseHealthSliders(Slider slider){
    float timeSlice = (slider.value / healthDecreaseTime);
    while (slider.value >= 0){
      slider.value -= timeSlice;
      yield return new WaitForSeconds(1);
      if (slider.value <= 0){
          EndGame();
          break;
      }
    }
  }

  public IEnumerator DecreseHealthTimer(){
    while (true){
      yield return new WaitForSeconds(20);
      healthDecreaseTime -= 1;
      if(healthDecreaseTime == 10){
        break;
      }
    }
  }
}