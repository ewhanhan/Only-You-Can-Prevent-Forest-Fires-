using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
  public Transform player;
  public float cameraSpeed = 5f;

  private void FixedUpdate()
  {
    Vector3 playerPos = player.position;
    playerPos.z = transform.position.z;
    var _newCamPos = new Vector3((float) Math.Round(playerPos.x), (float) Math.Round(playerPos.y),
      (float) Math.Round(playerPos.z));

    transform.position = Vector3.Lerp(transform.position, _newCamPos, cameraSpeed * Time.deltaTime);
  }
}