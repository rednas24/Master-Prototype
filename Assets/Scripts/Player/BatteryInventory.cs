using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Spare the player can carrie around with a max of 3
public class BatteryInventory : MonoBehaviour
{
    [SerializeField] private int maxBatteries = 3;
    [SerializeField] private int batteries = 0;
    [SerializeField] private TMP_Text countText;

    private Headlamp headlamp;

    public bool IsFull => batteries >= maxBatteries;

    private void Awake()
    {
        headlamp = GetComponentInChildren<Headlamp>();
        UpdateHUD();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            TrySwap();
    }

    public bool TryAdd()
    {
        if (IsFull)
            return false;

        batteries++;
        UpdateHUD();
        return true;
    }

    private void TrySwap()
    {
        if (batteries <= 0 || headlamp.IsSwapping || headlamp.Battery >= 1f)
            return;

        batteries--;
        UpdateHUD();
        headlamp.SwapBattery();
    }

    private void UpdateHUD()
    {
        if (countText != null)
            countText.text = $"Batteries {batteries}/{maxBatteries}";
    }
}