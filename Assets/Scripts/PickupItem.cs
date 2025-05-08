using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum ItemType { Gun, Scanner, Keycard }
    public ItemType itemType;

    public GameObject playerGun;
    public GameObject playerScanner;
    public GameObject keycard;


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
            else if (itemType == ItemType.Keycard)
            {
                Debug.Log("picked up the keycard!");
            }

            Destroy(gameObject); 
        }
    }
}
