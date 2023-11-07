using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int captureAdded = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CaptureSystem>().captureLeft += captureAdded;
            Destroy(gameObject);
        }
    }
}
