using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DoorFunctions : MonoBehaviour
{
  public Transform door;
  public float openHeight = -2f;
  public float closeHeight = 14.82f;
  public float speed = 2f;
  public float detectionDistance = 3f;
  public LayerMask playerLayer;
  public Transform raycastOrigin;
 
  private bool isDoorOpen = false;
 
  public void Start()
  {
    door.position = new Vector3(door.position.x, closeHeight, door.position.z);
  }
 
  public void Update()
  {
    RaycastHit hit;
 
    bool playerDetectedForward = Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, detectionDistance, playerLayer);
    bool playerDetectedBackward = Physics.Raycast(raycastOrigin.position, -raycastOrigin.forward, out hit, detectionDistance, playerLayer);
 
    if (playerDetectedForward || playerDetectedBackward)
    {
      OpenDoor();
    }
    else
    {
      CloseDoor();
    }
  }
 
  void OpenDoor()
  {
    if (!isDoorOpen)
    {
      StopAllCoroutines();
      StartCoroutine(MoveDoor(openHeight));
      isDoorOpen = true;
    }
  }
 
  void CloseDoor()
  {
    if (isDoorOpen)
    {
      StopAllCoroutines();
      StartCoroutine(MoveDoor(closeHeight));
      isDoorOpen = false;
    }
  }
 
  IEnumerator MoveDoor(float targetHeight)
  {
    Vector3 targetPosition = new Vector3(door.position.x, targetHeight, door.position.z);
 
    while (Vector3.Distance(door.position, targetPosition) > 0.01f)
    {
      door.position = Vector3.MoveTowards(door.position, targetPosition, speed * Time.deltaTime);
      yield return null;
    }
 
    door.position = targetPosition;
  }
 
  void OnDrawGizmos()
  {
    if (raycastOrigin != null)
    {
Gizmos.color = Color.red;
      Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + raycastOrigin.forward * detectionDistance);
      Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position - raycastOrigin.forward * detectionDistance);
    }
  }
}