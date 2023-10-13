using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movePos;
    private float time;
    private bool isMoving;
    private Rigidbody rb;

    [SerializeField]private float currentHeadRotationX = 0f;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform head;
    [SerializeField] private float rotateHeadSpeed;

    [SerializeField] private AnimationCurve speedCurve;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void FixedUpdate()
    {
        Movement();
        RotateCamera(); // Commentairte
    }

    private void RotateCamera()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue(); // je récupère la position de ma souris sur l'axe X et Y

        float mouseYRotation = (-mousePosition.y * rotateHeadSpeed * Time.fixedDeltaTime); // je modifie la mousePosition.y
        float mouseXRotation = (mousePosition.x * rotateHeadSpeed * Time.fixedDeltaTime); // je modifie la mousePosition.x

        currentHeadRotationX = Mathf.Clamp(mouseYRotation, -50f, 50f); //Je limites l'axe de la caméra en X pour éviter qu'il se torde le dos

        transform.localRotation = Quaternion.Euler(0, mouseXRotation, 0); //Ici je tourne le player
        head.localRotation = Quaternion.Euler(currentHeadRotationX, 0, 0); // Ici je tourne le composant "Head" qui a pour enfant la caméra
    }

    private void Movement()
    {
        if (isMoving)
        {
            time += Time.fixedDeltaTime;
            moveSpeed = speedCurve.Evaluate(time);
        }

        Vector3 direction = transform.TransformDirection(new Vector3(movePos.x * moveSpeed, rb.velocity.y, movePos.y * moveSpeed)); // On utilise Transform direction pour transformer nos rotation en local vers des rotation en world

        rb.velocity = direction; // la velocité va prendre la valeur de movPos de X pour l'axe X et pour l'axe Z on va lui donner la valeur de movPos de Y
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            movePos = context.ReadValue<Vector2>(); // Quand on appuie sur les touches WASD on modifie la valeur movePos
            isMoving = true;
        }
        if(context.canceled)
        {
            movePos = Vector2.zero;
            isMoving = false;
            time = 0;
            moveSpeed = 0;
        }
    }
}
