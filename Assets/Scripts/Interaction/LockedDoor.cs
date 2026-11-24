using DoorScript;
using UnityEngine;

// makes sure the door is locked
[RequireComponent(typeof(Door))]
public class LockedDoor : MonoBehaviour, Interactable
{
    [SerializeField] private string keyId = "Key";
    [SerializeField] private bool locked = true;
    [SerializeField] private bool consumeKey = true;
    [SerializeField] private AudioClip unlockSound;

    private Door door;

    private void Awake()
    {
        door = GetComponent<Door>();
    }

    public string GetPrompt(GameObject player)
    {
        if (!locked)
            return door.open ? "Close door" : "Open door";

        return HasKey(player) ? "Unlock door" : "Locked - needs a key";
    }

    public bool CanInteract(GameObject player)
    {
        return !locked || HasKey(player);
    }

    public void Interact(GameObject player)
    {
        if (locked)
            Unlock(player);

        door.OpenDoor(); //script already included in package
    }

    private void Unlock(GameObject player)
    {
        locked = false;

        if (consumeKey)
            player.GetComponent<KeyInventory>().Remove(keyId);

        if (unlockSound != null)
            AudioSource.PlayClipAtPoint(unlockSound, transform.position);
    }

    private bool HasKey(GameObject player)
    {
        KeyInventory keys = player.GetComponent<KeyInventory>();
        return keys != null && keys.Has(keyId);
    }
}