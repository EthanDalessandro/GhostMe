using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int captureAdded = 3;

    [SerializeField] private AudioSource mainAudio;
    [SerializeField] private AudioClip pickUpSound;

    private void Awake()
    {
        mainAudio = FindObjectOfType<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainAudio.PlayOneShot(pickUpSound);
            other.GetComponent<CaptureSystem>().captureLeft += captureAdded;
            other.GetComponent<CaptureSystem>().cameraCaptureLeftTextHUD.text = other.GetComponent<CaptureSystem>().captureLeft.ToString();
            Destroy(gameObject);
        }
    }
}
