using UnityEngine;
using UnityEngine.UI;

public class ScrollBarScaler : MonoBehaviour
{
    [SerializeField] RectTransform _shotListRectTransform;
    Scrollbar _scrollbar;

    private void Start()
    {
        _scrollbar = GetComponent<Scrollbar>();
        ScaleScrollBar();
    }

    void ScaleScrollBar()
    {
        if (!_scrollbar) { return; }
        float screenSizeLength = 1000 / _shotListRectTransform.sizeDelta.y;
        if (screenSizeLength >= 1)
        {
            _scrollbar.size = 1;
            return;
        }
        _scrollbar.size = screenSizeLength;
    }
}
