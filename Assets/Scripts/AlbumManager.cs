using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AlbumManager : MonoBehaviour
{
    public bool albumOpen = false;

    [SerializeField] private GameObject albumCanvas;
    [SerializeField] private GameObject shotPrefab;

    [SerializeField] private CaptureSystem listOfScreen;

    public void OpenCloseAlbum(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(albumOpen == false)
            {
                albumOpen = true;
                albumCanvas.SetActive(true);

                for (int i = 0; i < listOfScreen.screenSprites.Count; i++)
                {
                    GameObject actualShot = Instantiate(shotPrefab, albumCanvas.transform, false);
                    actualShot.GetComponent<Image>().sprite = listOfScreen.screenSprites[i];
                }
                listOfScreen.screenSprites.Clear();
            }
            else
            {
                albumOpen = false;
                albumCanvas.SetActive(false);
            }
        }
    }
}
