using UnityEngine;
using UnityEngine.InputSystem;

public class CaptureSystem : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float score;
    [SerializeField] private Camera cam;
    [SerializeField] private GridPointSensor ghostSensors;

    private void Start()
    {
        ghostSensors = target.gameObject.GetComponent<GridPointSensor>();
    }

    public void Capture(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CheckGhostCollisionRay();
        }
    }

    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
        return targetFrom + (source - sourceFrom) * (targetTo - targetFrom) / (sourceTo - sourceFrom);
    }

    private void CheckGhostCollisionRay()
    {
         score = 0;

        for (int i = 0; i < ghostSensors.Points.Count; i++)
        {
            Vector3 direction = ghostSensors.Points[i] - transform.position;
            direction = direction.normalized;
            score += CheckVisibility(direction);
        }

        score /= ghostSensors.Points.Count;
    }

    private float CheckVisibility(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 45f))
        {
            if (hit.transform.gameObject.CompareTag("Ghost"))
            {
                float cosAngle = Vector3.Dot(cam.transform.forward, direction);
                float factor = Remap(cosAngle, 0.6f, 0.8f, 0, 1);
                factor = Mathf.Clamp01(factor);

                factor *= factor;

                //Debug.DrawRay(transform.position, direction * hit.distance, Color.red * factor * 1 / Mathf.Max(1, hit.distance - 5));

                return factor * 1 / Mathf.Max(1, hit.distance - 5);
            }
        }

        return 0;
    }
}
