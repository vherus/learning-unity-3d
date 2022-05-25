using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private float gravity = 28f;

    private float vVelocity;
    private CharacterController cController;
    private Vector3 mDirection;

    void Awake() {
        cController = GetComponent<CharacterController>();
    }

    void FixedUpdate() {
        HandleMove();
    }

    void HandleMove() {
        float hInput = Input.GetAxis(Axis.HORIZONTAL);
        float vInput = Input.GetAxis(Axis.VERTICAL);

        mDirection = transform.TransformDirection(new Vector3(hInput, 0f, vInput));
        mDirection *= speed * Time.deltaTime;
        
        ApplyGravity();

        cController.Move(mDirection);
    }

    void ApplyGravity() {
        vVelocity -= gravity * Time.deltaTime;

        HandleJump();

        mDirection.y = vVelocity * Time.deltaTime;
    }

    void HandleJump() {
        if (cController.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            vVelocity = jumpForce;
        }
    }
}
