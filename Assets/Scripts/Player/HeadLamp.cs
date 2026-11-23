using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//headlamp, turn on with F, begins to flicker when low battery to give feedback to the player
[RequireComponent(typeof(Light))]
public class Headlamp : MonoBehaviour
{
    [System.Serializable]
    private struct FlickerStage
    {
        [Range(0f, 1f)] public float atBattery;
        public Vector2 secondsBetween;
        public Vector2Int blinksPerBurst;
    }

    [Header("Battery")]
    [SerializeField] private float batteryLifeSeconds = 300f;
    [SerializeField, Range(0f, 1f)] private float battery = 1f;
    [SerializeField] private float swapSeconds = 1.5f;

    [Header("Dimming")]
    [SerializeField, Range(0f, 1f)] private float dimBelow = 0.1f;
    [SerializeField, Range(0f, 1f)] private float brightnessAtEmpty = 0.3f;

    [Header("Flicker stages (highest battery first)")]
    [SerializeField] private FlickerStage[] stages =
    {
        new FlickerStage { atBattery = 0.5f, secondsBetween = new Vector2(30f, 45f), blinksPerBurst = new Vector2Int(1, 2) },
        new FlickerStage { atBattery = 0.2f, secondsBetween = new Vector2(8f, 15f),  blinksPerBurst = new Vector2Int(2, 3) },
        new FlickerStage { atBattery = 0.1f, secondsBetween = new Vector2(2f, 5f),   blinksPerBurst = new Vector2Int(3, 5) },
    };

    private Light lamp;
    private float baseIntensity;
    private float flickerMultiplier = 1f;
    private int currentStage = -1;
    private float nextFlickerTime;
    private Coroutine flickerRoutine;

    public float Battery => battery;
    public bool IsSwapping { get; private set; }

    public void SwapBattery()
    {
        if (!IsSwapping)
            StartCoroutine(Swap());
    }

    private IEnumerator Swap()
    {
        IsSwapping = true;
        bool turnOnAfter = lamp.enabled || battery <= 0f;
        SetOn(false);

        yield return new WaitForSeconds(swapSeconds);

        battery = 1f;
        SetOn(turnOnAfter);
        IsSwapping = false;
    }

    private void Awake()
    {
        lamp = GetComponent<Light>();
        baseIntensity = lamp.intensity;
    }

    private void Update()
    {
        
        if (!IsSwapping && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            SetOn(!lamp.enabled);

        if (!lamp.enabled)
            return;

        battery = Mathf.Max(0f, battery - Time.deltaTime / batteryLifeSeconds);
        if (battery <= 0f)
        {
            SetOn(false);
            return;
        }

        UpdateFlicker();
        lamp.intensity = baseIntensity * Brightness() * flickerMultiplier;
    }

    private void SetOn(bool on)
    {
        lamp.enabled = on && battery > 0f;
        if (!lamp.enabled)
            StopFlicker();
    }

    private void UpdateFlicker()
    {
        int stage = -1;
        for (int i = 0; i < stages.Length; i++)
            if (battery <= stages[i].atBattery)
                stage = i;

        if (stage != currentStage)
        {
            currentStage = stage;
            nextFlickerTime = Time.time;
        }

        if (stage < 0 || flickerRoutine != null || Time.time < nextFlickerTime)
            return;

        FlickerStage s = stages[stage];
        int blinks = Random.Range(s.blinksPerBurst.x, s.blinksPerBurst.y + 1);
        flickerRoutine = StartCoroutine(Flicker(blinks));
        nextFlickerTime = Time.time + Random.Range(s.secondsBetween.x, s.secondsBetween.y);
    }

    private IEnumerator Flicker(int blinks)
    {
        for (int i = 0; i < blinks; i++)
        {
            flickerMultiplier = Random.Range(0f, 0.3f);
            yield return new WaitForSeconds(Random.Range(0.04f, 0.12f));
            flickerMultiplier = 1f;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }
        flickerRoutine = null;
    }

    private void StopFlicker()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);
        flickerRoutine = null;
        flickerMultiplier = 1f;
    }

    private float Brightness()
    {
        if (battery >= dimBelow)
            return 1f;
        return Mathf.Lerp(brightnessAtEmpty, 1f, battery / dimBelow);
    }
}