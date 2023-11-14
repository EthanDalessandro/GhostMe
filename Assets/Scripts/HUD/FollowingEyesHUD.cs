using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FollowingEyesHUD : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform body;
    [SerializeField] private Transform eyesTransform;

    [SerializeField] private float eyesLookAtSpeed;
    [SerializeField] private float bodyLookAtSpeed;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [Header("Wide-Eyes Animation")]
    [SerializeField] private int requireClickToAnimate;
    [SerializeField] private int animationDuration;
    private Animator eyesAnimator;

    private Vector2 bodyOrigin;
    private Vector2 eyesTransformOrigin;

    private int clickCount;
    private bool isAnimated;

    void Start()
    {
        eyesAnimator = GetComponentInChildren<Animator>();
        bodyOrigin = body.position;
        eyesTransformOrigin = eyesTransform.position;
    }

    void Update()
    {
        LookAtMovement(body, bodyOrigin, bodyLookAtSpeed, offsetX / 4, offsetY / 4);
        LookAtMovement(eyesTransform, eyesTransformOrigin, eyesLookAtSpeed, offsetX, offsetY);
    }

    void LookAtMovement(Transform objectToMove, Vector2 ObjectToMoveOrigin, float speedOfLook, float offSetX, float offSetY)
    {
        if (isAnimated == true) { return; }
        Vector2 mousePos = Mouse.current.position.ReadValue();

        objectToMove.position = Vector2.MoveTowards(objectToMove.position, mousePos, speedOfLook);
        Vector2 clampedPosition = objectToMove.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, ObjectToMoveOrigin.x - offSetX, ObjectToMoveOrigin.x + offSetX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, ObjectToMoveOrigin.y - offSetY, ObjectToMoveOrigin.y + offSetY);
        objectToMove.position = clampedPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(clickCount > requireClickToAnimate) { return; }
        clickCount++;
        if(clickCount >= requireClickToAnimate)
        {
            StartCoroutine(EyesLockedAnimation());
        }
    }

    IEnumerator EyesLockedAnimation()
    {
        isAnimated = true;
        eyesAnimator.SetBool("IsBlinking", false);
        yield return new WaitForSeconds(animationDuration);
        eyesAnimator.SetBool("IsBlinking", true);
        clickCount = 0;
        isAnimated = false;
    }
}
