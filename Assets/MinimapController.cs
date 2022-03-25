using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
  public Transform player;

  private void LateUpdate()
  {
    Vector3 playerPos = player.position;
    playerPos.z = transform.position.z;
    transform.position = new Vector3((float) Math.Round(playerPos.x), (float) Math.Round(playerPos.y),
      (float) Math.Round(playerPos.z));
  }
}