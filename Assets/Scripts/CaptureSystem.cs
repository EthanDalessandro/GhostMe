using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CaptureSystem : MonoBehaviour
{
    private int heightScreen = 512;
    private int widthScreen = 512;
    private int depthScreen = 12;

    [SerializeField] private Transform target;
    [SerializeField] private float score;
    [SerializeField] private Camera cam;
    [SerializeField] private GridPointSensor ghostSensors;
    public List<Sprite> screenSprites;

    private void Start()
    {
        screenSprites.Clear();
        ghostSensors = target.gameObject.GetComponent<GridPointSensor>();
    }

    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
        return targetFrom + (source - sourceFrom) * (targetTo - targetFrom) / (sourceTo - sourceFrom);
    }

    public void Capture(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CheckGhostCollisionRay();
            CaptureScreen();
        }
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

    public void CaptureScreen()
    {
        RenderTexture renderTexture = new RenderTexture(widthScreen, heightScreen, depthScreen);
        Rect rect = new Rect(0, 0, widthScreen, heightScreen);
        Texture2D texture = new Texture2D(widthScreen, heightScreen, TextureFormat.RGBA32, false);

        cam.targetTexture = renderTexture;
        cam.Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        cam.targetTexture = null;
        RenderTexture.active = currentRenderTexture;
        Destroy(renderTexture);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

        screenSprites.Add(sprite);
    }
}
