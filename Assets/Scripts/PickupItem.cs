using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum ItemType { Gun, Scanner }
    public ItemType itemType;

    public GameObject playerGun;
    public GameObject playerScanner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemType == ItemType.Gun)
            {
                playerGun.SetActive(true);
                Debug.Log("Picked up the Gun!");
            }
            else if (itemType == ItemType.Scanner)
            {
                playerScanner.SetActive(true);
                Debug.Log("Picked up the Scanner!");
            }

            Destroy(gameObject); 
        }
    }
}
