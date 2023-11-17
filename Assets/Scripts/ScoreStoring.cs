using System.Collections.Generic;
using UnityEngine;

public class ScoreStoring : MonoBehaviour
{
    public static ScoreStoring instance;
    [SerializeField] private List<Sprite> screenShotsTextures = new List<Sprite>();
    [SerializeField] private List<int> screenShotsPoints = new List<int>();

    public List<Sprite> ScreenShotsTextures { get { return screenShotsTextures; } set { screenShotsTextures = value; } }
    public List<int> ScreenShotsPoints { get { return screenShotsPoints; } set { screenShotsPoints = value; } }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null) { instance = this; }
    }
}
