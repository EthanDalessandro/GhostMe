using UnityEngine;
using UnityEngine.InputSystem;

public class FollowingEyesHUD : MonoBehaviour
{
    public Transform body;
    private Vector2 bodyOrigin;

    public Transform leftEye;
    private Vector2 leftEyeOrigin;

    public Transform rightEye;
    private Vector2 rightEyeOrigin;

    public float lookAtSpeed;
    public float offsetX;
    public float offsetY;

    private void Start()
    {
        leftEyeOrigin = leftEye.position;
        rightEyeOrigin = rightEye.position;
        bodyOrigin = body.position;
    }

    private void Update()
    {
        LookAtMovement(leftEye, leftEyeOrigin, lookAtSpeed, offsetX, offsetY);
        LookAtMovement(rightEye, rightEyeOrigin, lookAtSpeed, offsetX, offsetY);
        LookAtMovement(body, bodyOrigin, lookAtSpeed, offsetX / 3, offsetY / 3);
    }

    private void LookAtMovement(Transform objectToMove, Vector2 ObjectToMoveOrigin, float speedOfLook, float offSetX, float offSetY)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        objectToMove.position = Vector2.MoveTowards(objectToMove.position, mousePos, speedOfLook);
        Vector2 clampedPosition = objectToMove.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, ObjectToMoveOrigin.x - offSetX, ObjectToMoveOrigin.x + offSetX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, ObjectToMoveOrigin.y - offSetY, ObjectToMoveOrigin.y + offSetY);
        objectToMove.position = clampedPosition;
    }
}
