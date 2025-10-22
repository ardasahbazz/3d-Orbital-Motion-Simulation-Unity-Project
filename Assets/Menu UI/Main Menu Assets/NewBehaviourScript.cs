using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour 
{
  public float speed = 0.5f;

  private Vector3 startPos;

  void Start() 
  {
    startPos = transform.position; 
  }

  void Update()
  {
    if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
    {
      float distX = Input.GetAxis("Mouse X") * speed;
      float distY = Input.GetAxis("Mouse Y") * speed;

      transform.position = new Vector3(startPos.x - distX, startPos.y - distY, startPos.z);
    }
  }
}