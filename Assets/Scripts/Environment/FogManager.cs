using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform = null;

    private void Update()
    {
        transform.position = new Vector3(playerTransform.position.x , 0, playerTransform.position.z);
    }
}
