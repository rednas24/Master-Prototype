using UnityEngine;

// Battery lying in the world can be picked up and placed into battery inventory
public class BatteryPickup : MonoBehaviour, Interactable
{
    [SerializeField] private AudioClip pickupSound;

    public string GetPrompt(GameObject player)
    {
        return CanInteract(player) ? "Pick up battery" : "Batteries full";
    }

    public bool CanInteract(GameObject player)
    {
        BatteryInventory inventory = player.GetComponent<BatteryInventory>();
        return inventory != null && !inventory.IsFull;
    }

    public void Interact(GameObject player)
    {
        if (!player.GetComponent<BatteryInventory>().TryAdd())
            return;

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }
}