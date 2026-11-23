using UnityEngine;

// Anything the player can look at and press E on
public interface Interactable
{
    string GetPrompt(GameObject player);

    bool CanInteract(GameObject player);

    void Interact(GameObject player);
}