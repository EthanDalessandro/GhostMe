using UnityEngine;
using UnityEngine.UI;

public class EndShotListManager : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private int _shotOffset;
    [SerializeField] private int shotCount;
    [SerializeField] private int shotListY;
    [SerializeField] private Vector2 _shotScale;
    private RectTransform _shotListRectTransform;

    private void Start()
    {
        _shotListRectTransform = GetComponent<RectTransform>();
        ScaleShotList();
        ScaleScrollBar();
    }

    private void Update()
    {
        _shotListRectTransform.anchoredPosition = new Vector2(0, GetShotListY());
    }

    private void ScaleShotList()
    {
        if (!ScoreStoring.instance) { return; }
        shotCount = ScoreStoring.instance.ScreenShotsTextures.Count;
        shotListY = _shotOffset * shotCount;
        shotListY += (int)_shotScale.y * shotCount;
        _shotListRectTransform.sizeDelta = new Vector2(1000, shotListY);
    }

    private void ScaleScrollBar()
    {
        if (_scrollbar == null) { return; }
        float screenSizeLength = GetScreenSizeLength();
        if(screenSizeLength >= 1)
        {
            _scrollbar.size = 1;
            return;
        }
        _scrollbar.size = screenSizeLength;
    }

    private float GetShotListY()
    {
        if (_scrollbar == null) { return _shotListRectTransform.anchoredPosition.y; }
        float shotListPosition = _scrollbar.value * _shotListRectTransform.sizeDelta.y;
        return shotListPosition;
    }

    private float GetScreenSizeLength()
    {
        return 1000 / _shotListRectTransform.sizeDelta.y;
    }
}
