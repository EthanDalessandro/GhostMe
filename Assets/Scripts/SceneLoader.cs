using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    Image _fadeImage;

    Animator _fadeAnimator;

    private void Awake()
    {
        _fadeImage = GetComponent<Image>();
        _fadeAnimator = _fadeImage.GetComponent<Animator>();
        instance = this;
    }

    public void LoadScene(string sceneToLoad)
    {
        StartCoroutine(LoadSceneTransition(sceneToLoad));
    }

    IEnumerator LoadSceneTransition(string sceneToLoad)
    {

        if (_fadeImage) 
        { 
            _fadeAnimator.SetBool("DoFade", true); 
            yield return new WaitForSeconds(1f);
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
