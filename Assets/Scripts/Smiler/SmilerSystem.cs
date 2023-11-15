using Cinemachine;
using System.Collections;
using UnityEngine;

public class SmilerSystem : MonoBehaviour
{
    [Header("Spawning System Properties")]
    [SerializeField] private float timeBeforeSpawnSmiler;
    [SerializeField, Range(0, 100)] private float chanceToSpawn;
    private float timeBeforeSpawnSmilerBackup;

    [Header("Reference To Other Objects")]
    [SerializeField] private GameObject smilerPrefab;
    [SerializeField] private CinemachineImpulseSource cameraShakeSource;

    [Header("Audio Properties")]
    [SerializeField, Range(0, 1)] private float volume;
    [SerializeField] private AudioClip smilerSpawnSfx;
    [SerializeField] private AudioClip ScreamerSfx;
    [SerializeField] private AudioSource mainAudioSource;

    [Header("General Properties")]
    [SerializeField] private float maxDistanceOfSpawn;
    [SerializeField] private float timeBeforeKillObject;
    [SerializeField] private float timeBetweenImpulse;
    [SerializeField] private bool canShakeCamera;
    [SerializeField] private GameObject actualSmiler;

    private void Start()
    {
        timeBeforeSpawnSmilerBackup = timeBeforeSpawnSmiler;
    }

    private void Update()
    {
        ChanceToSpawnSmiler();
        if(actualSmiler != null)
        {
            if(Vector3.Dot(transform.forward, (actualSmiler.transform.position - transform.position).normalized) >= 0.9f && canShakeCamera)
            {
                StartCoroutine(ShakeCameraDelay());
                cameraShakeSource.GenerateImpulse();
                mainAudioSource.PlayOneShot(ScreamerSfx, volume * 1.25f);
                StartCoroutine(DestroyActualSmiler(0.25f));
            }
        }
    }

    private void ChanceToSpawnSmiler()
    {
        timeBeforeSpawnSmiler -= Time.deltaTime;

        if (timeBeforeSpawnSmiler <= 0)
        {
            if (Random.Range(0, 101) < chanceToSpawn && actualSmiler == null)
            {
                SpawnSmiler(smilerPrefab);
                chanceToSpawn -= 15;
            }
            timeBeforeSpawnSmiler = timeBeforeSpawnSmilerBackup;
        }
        chanceToSpawn += 0.08f * Time.deltaTime;
        chanceToSpawn = Mathf.Clamp(chanceToSpawn, 0, 70);
    }

    public void SpawnSmiler(GameObject smilerPrefab)
    {
        if(maxDistanceOfSpawn <= 2)
        {
            maxDistanceOfSpawn = 2;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward * -1, out hit, maxDistanceOfSpawn))
        {
            if (hit.collider != null)
            {
                actualSmiler = Instantiate(smilerPrefab, transform.forward * -Random.Range(2, hit.distance), Quaternion.identity);
                actualSmiler.transform.rotation = Quaternion.LookRotation(transform.position - actualSmiler.transform.position);
                actualSmiler.transform.position = new Vector3(actualSmiler.transform.position.x, actualSmiler.transform.position.y + 1, actualSmiler.transform.position.z);

                mainAudioSource.PlayOneShot(smilerSpawnSfx, volume / 4);

                StartCoroutine(DestroyActualSmiler(timeBeforeKillObject));
            }
        }
        else
        {
            actualSmiler = Instantiate(smilerPrefab, transform.forward * -Random.Range(2, maxDistanceOfSpawn), Quaternion.identity);
            actualSmiler.transform.rotation = Quaternion.LookRotation(transform.position - actualSmiler.transform.position);
            actualSmiler.transform.position = new Vector3(actualSmiler.transform.position.x, actualSmiler.transform.position.y + 1, actualSmiler.transform.position.z);

            mainAudioSource.PlayOneShot(smilerSpawnSfx, volume);
            StartCoroutine(DestroyActualSmiler(timeBeforeKillObject));
        }
    }

    public IEnumerator DestroyActualSmiler(float time)
    {
        yield return new WaitForSeconds(time);
        if(actualSmiler != null)
        {
            Destroy(actualSmiler);
            actualSmiler = null;
        }
    }

    public IEnumerator ShakeCameraDelay()
    {
        canShakeCamera = false;
        yield return new WaitForSeconds(timeBetweenImpulse);
        canShakeCamera = true;
    }
}
