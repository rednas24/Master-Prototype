using UnityEngine;

//decides what animation to play at what time
[RequireComponent(typeof(Animator))]
public class HandsAnimator : MonoBehaviour
{
    [SerializeField] private float speedDampTime = 0.1f;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private Animator animator;
    private CharacterController controller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        Vector3 velocity = controller.velocity;
        velocity.y = 0f;
        animator.SetFloat(SpeedHash, velocity.magnitude, speedDampTime, Time.deltaTime);
    }
}