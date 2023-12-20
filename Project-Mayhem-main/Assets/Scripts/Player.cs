using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] LayerMask aimLayerMask;

    public static Vector3 Direction;
    public static Animator animator;

    private Vector3 movement;

    int VelocityXHash;
    int VelocityZHash;

    float velocityZ;
    float velocityX;

    float horizontal;
    float vertical;

    public static Player playerInstance;
    private Rigidbody playerRb;

    void Awake()
    {
        playerInstance = this;
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        gameObject.name = "Player";
        VelocityXHash = Animator.StringToHash("VelocityX");
        VelocityZHash = Animator.StringToHash("VelocityZ");
    }

    void Update()
    {
        bool RightClicked = Input.GetMouseButton(1);
        bool LeftClicked = Input.GetMouseButton(0);
        bool OnePressed = Input.GetKeyDown(KeyCode.Alpha1);
        AimTowardMouse();

        // Reading the input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Animating
        velocityZ = Vector3.Dot(movement, transform.forward);
        velocityX = Vector3.Dot(movement, transform.right);

        animator.SetFloat(VelocityZHash, velocityZ, 0.1f, Time.deltaTime);
        animator.SetFloat(VelocityXHash, velocityX, 0.1f, Time.deltaTime);

        // Check if the player is not pressing WASD or not moving
        if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
        {
            // Activate isKinematic on Rigidbody
            playerRb.isKinematic = true;
        }
        else
        {
            // Deactivate isKinematic on Rigidbody
            playerRb.isKinematic = false;
        }
    }

    void FixedUpdate()
    {
        MovePlayer(movement);
    }

    void MovePlayer(Vector3 direction)
    {
        // Move the player directly without using Rigidbody
        transform.position += direction * speed * Time.deltaTime;
    }

    void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            var targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y;
            var direction = targetPosition - transform.position;
            direction.Normalize();
            transform.forward = direction;
            Direction = direction;
        }
    }

    public void PlayerDie()
    {
        GameManager.restartText.text = "<i>PRESS R TO RESTART</i>";
        gameObject.SetActive(false);
    }
}
