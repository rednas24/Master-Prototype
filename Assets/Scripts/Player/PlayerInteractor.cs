using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Shows a promt when a intem that can be picked up is hovered over
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float reach = 2f;
    [SerializeField] private TMP_Text promptText;

    private Transform cameraTransform;
    private InputAction interactAction;

    private void Awake()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        interactAction = InputSystem.actions.FindAction("Player/Interact", throwIfNotFound: true);
    }

    private void Update()
    {
        Interactable target = FindTarget();
        UpdatePrompt(target);

        if (target != null && interactAction.WasPressedThisFrame() && target.CanInteract(gameObject))
            target.Interact(gameObject);
    }

    private Interactable FindTarget()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, reach, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            return null;

        return hit.collider.GetComponentInParent<Interactable>();
    }

    private void UpdatePrompt(Interactable target)
    {
        if (promptText == null)
            return;

        if (target == null)
        {
            promptText.text = "";
            return;
        }

        string prompt = target.GetPrompt(gameObject);
        promptText.text = target.CanInteract(gameObject) ? $"[E] {prompt}" : prompt;
    }
}