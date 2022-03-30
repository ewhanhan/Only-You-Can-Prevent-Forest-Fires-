using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Timer : MonoBehaviour
{
  float currentTime;
  public Text currentTimeText;
  public PlayerController playerController;

  // Update is called once per frame
  void Update()
  {
    if (playerController.playerActive)
    {
      currentTime += Time.deltaTime;
      currentTimeText.text = currentTime.ToString();
    }
  }
}