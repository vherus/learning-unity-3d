using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform playerRoot, lookRoot;

    [SerializeField]
    private bool invert;

    [SerializeField]
    private bool canUnlock = true;

    [SerializeField]
    private float sensitivity = 5f;

    [SerializeField]
    private int smoothSteps = 10;

    [SerializeField]
    private float smoothWeight = 0.4f;

    [SerializeField]
    private float rollAngle = 0f;

    [SerializeField]
    private float rollSpeed = 3f;

    [SerializeField]
    private Vector2 defaultLookLimits = new Vector2(-70f, 80f);

    private Vector2 lookAngles;
    private Vector2 currentMouseLook;
    private Vector2 smoothMove;
    private float currentRollAngle;
    private int lastLookFrame;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        LockUnlockCursor();

        if (Cursor.lockState == CursorLockMode.Locked) {
            HandleMouseMovement();
        }
    }

    private void HandleMouseMovement() {
        float mouseY = Input.GetAxis(Axis.MOUSE_Y);
        float mouseX = Input.GetAxis(Axis.MOUSE_X);

        currentMouseLook = new Vector2(mouseY, mouseX);

        lookAngles.x += currentMouseLook.x * sensitivity * (invert ? 1f : -1f);
        lookAngles.y += currentMouseLook.y * sensitivity;

        LimitCameraYMovement();

        RollCamera();

        RotateCamera();
    }

    private void RotateCamera() {
        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngle);
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }

    // Creates a drunk effect if rollAngle > 0f
    private void RollCamera() {
        float rollTo = Input.GetAxisRaw(Axis.MOUSE_X) * rollAngle;
        float rollInterval = Time.deltaTime * rollSpeed;

        currentRollAngle = Mathf.Lerp(currentRollAngle, rollTo, rollInterval);
    }

    private void LimitCameraYMovement() {
        lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLookLimits.x, defaultLookLimits.y);
    }

    private void LockUnlockCursor() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
