using UnityEngine;

// Makes key interacble like the battery
public class KeyPickup : MonoBehaviour, Interactable
{
    [SerializeField] private string keyId = "Key";
    [SerializeField] private AudioClip pickupSound;

    public string GetPrompt(GameObject player) => "Pick up key";

    public bool CanInteract(GameObject player) => player.GetComponent<KeyInventory>() != null;

    public void Interact(GameObject player)
    {
        player.GetComponent<KeyInventory>().Add(keyId);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }
}