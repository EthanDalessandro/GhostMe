using UnityEngine;
using UnityEngine.InputSystem;
/*
On peut utiliser un rect transform et comparer la position de la target et voir si elle se trouve dans le rect
on peut utiliser des box cast pour detecter le fantôme
on peut utiliser la camera directement
*/
public class CaptureSystem : MonoBehaviour
{
    private Vector3 targetToViewPortPos;
    
    [SerializeField] private Transform target;
    [SerializeField] private float score;
    [SerializeField] private Camera cam;

    private void Update()
    {
        targetToViewPortPos = cam.WorldToViewportPoint(target.position);
    }

    public void Capture(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (targetToViewPortPos.x >= 0
            && targetToViewPortPos.x <= 1
            && targetToViewPortPos.y >= 0
            && targetToViewPortPos.y <= 1
            && targetToViewPortPos.z <= 45f
            && targetToViewPortPos.z >= 0)
            {
                Debug.Log("Dedans");
            }
            else
            {
                Debug.Log("Pas Dedans");
            }
        }
    }
}
