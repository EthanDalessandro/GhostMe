using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AlbumManager : MonoBehaviour
{
    public bool albumOpen = false;

    [SerializeField] private GameObject albumCanvas;
    [SerializeField] private GameObject CanvasToDeactivate;
    [SerializeField] private GameObject shotPrefab;

    [SerializeField] private CaptureSystem listOfScreen;

    public void OpenCloseAlbum(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(albumOpen == false)
            {
                Cursor.lockState = CursorLockMode.Confined;
                albumOpen = true;
                albumCanvas.SetActive(true);
                CanvasToDeactivate.SetActive(false);

                for (int i = 0; i < listOfScreen.screenSprites.Count; i++)
                {
                    GameObject actualShot = Instantiate(shotPrefab, albumCanvas.transform, false);
                    GameObject actualShotImage = GameObject.FindWithTag("ImageShot");
                    actualShotImage.GetComponent<Image>().sprite = listOfScreen.screenSprites[i];
                    actualShotImage.tag = "Untagged";
                }
                listOfScreen.screenSprites.Clear();
            }
            else
            {
                Cursor.lockState= CursorLockMode.Locked;
                albumOpen = false;
                albumCanvas.SetActive(false);
                CanvasToDeactivate.SetActive(true);
            }
        }
    }
}
