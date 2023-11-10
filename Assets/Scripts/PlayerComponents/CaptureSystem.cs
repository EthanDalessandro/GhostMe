using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CaptureSystem : MonoBehaviour
{
    private int heightScreen = 512;
    private int widthScreen = 512;
    private int depthScreen = 12;

    [Header("Reference To other Objects")]
    [SerializeField] private List<Transform> targets;
    public TextMeshProUGUI cameraCaptureLeftTextHUD;
    [SerializeField] private GameObject ShutterEffectObject;

    [Header("Sensors")]
    [SerializeField] private Camera cam;
    [SerializeField] private List<GridPointSensor> ghostSensors;

    [Header("Sounds")]
    [SerializeField, Range(0f, 1f)] private float volume;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cameraShutterSFX;

    [SerializeField] private float score;

    [Header("Capture Properties")]
    [SerializeField] private bool canCapture = true;
    [SerializeField] private float captureRate;
    public int captureLeft; 

    [Header("Sprites")]
    public List<Sprite> screenSprites;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        canCapture = true;
        screenSprites.Clear();

        foreach (Transform t in targets)
        {
            ghostSensors.Add(t.GetComponent<GridPointSensor>());
        }
    }

    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
        return targetFrom + (source - sourceFrom) * (targetTo - targetFrom) / (sourceTo - sourceFrom);
    }

    public void Capture(InputAction.CallbackContext context)
    {
        if (context.performed && canCapture == true && captureLeft > 0)
        {
            //Gameplay /****
            captureLeft--;
            StartCoroutine(captureDelay(captureRate));
            CheckGhostCollisionRay();
            CaptureScreen();
            //Gameplay ****/

            //HUD /****
            cameraCaptureLeftTextHUD.text = captureLeft.ToString();
            ShutterEffectObject.SetActive(false);
            ShutterEffectObject.SetActive(true);
            //HUD ****/
        }
    }

    private void CheckGhostCollisionRay()
    {
        score = 0;
        audioSource.PlayOneShot(cameraShutterSFX, volume);
        if (targets.Count > 0)
        {
            foreach (GridPointSensor sensor in ghostSensors)
            {
                for (int i = 0; i < sensor.Points.Count; i++)
                {
                    Vector3 direction = sensor.Points[i] - transform.position;
                    direction = direction.normalized;
                    score += CheckVisibility(direction);
                }
            }
            score /= ghostSensors[0].Points.Count;
            score *= 100;
        }
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

    public IEnumerator captureDelay(float time)
    {
        canCapture = false;
        yield return new WaitForSeconds(time);
        canCapture = true;
    }
}
