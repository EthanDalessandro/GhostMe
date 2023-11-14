using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int captureAdded = 3;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CaptureSystem>().captureLeft += captureAdded;
            other.GetComponent<CaptureSystem>().cameraCaptureLeftTextHUD.text = other.GetComponent<CaptureSystem>().captureLeft.ToString();
            Destroy(gameObject);
        }
    }
}
