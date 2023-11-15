using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndShotListManager : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] GameObject _shotPrefab;
    [SerializeField] Vector2 _shotScale;
    [SerializeField] int _shotOffset;
    [SerializeField][Range(0.1f, 10f)] float _shotListMoveSpeed;
    [SerializeField] bool _isInitial;

    RectTransform _shotListRectTransform;
    Vector2 _moveTarget;
    bool _moveShotList;
    int shotCount;

    IEnumerator Start()
    {
        _shotListRectTransform = GetComponent<RectTransform>();
        ScaleShotList();
        ScaleScrollBar();
        CreateShots();
        SetMoveTarget();
        yield return new WaitForSeconds(1);
        _moveShotList = true;
    }

    void Update()
    {
        if(_isInitial) 
        {
            if (_moveShotList) { transform.position = Vector2.MoveTowards(transform.position, _moveTarget, _shotListMoveSpeed); }
            if (transform.position.y >= _moveTarget.y) 
            {
                _moveShotList = false;
                _isInitial = false;
            }
            return; 
        }
        _shotListRectTransform.anchoredPosition = new Vector2(0, GetShotListY());
    }

    void ScaleShotList()
    {
        if (!ScoreStoring.instance) { return; }
        shotCount = ScoreStoring.instance.ScreenShotsTextures.Count;
        int shotListY = _shotOffset * shotCount;
        shotListY += (int)_shotScale.y * shotCount;
        _shotListRectTransform.sizeDelta = new Vector2(1000, shotListY);
    }

    void ScaleScrollBar()
    {
        if (!_scrollbar) { return; }
        float screenSizeLength = GetScreenSizeLength();
        if(screenSizeLength >= 1)
        {
            _scrollbar.size = 1;
            return;
        }
        _scrollbar.size = screenSizeLength;
    }

    float GetShotListY()
    {
        if (!_scrollbar) { return _shotListRectTransform.anchoredPosition.y; }
        float shotListPosition = _scrollbar.value * (_shotListRectTransform.sizeDelta.y - 850);
        return shotListPosition;
    }

    float GetScreenSizeLength()
    {
        return 1000 / _shotListRectTransform.sizeDelta.y;
    }

    void CreateShots()
    {
        if(!_shotPrefab) { return; }
        int shotXposition;
        int shotYposition = -100;
        for (int i = 0; i < shotCount; i++)
        {
            GameObject clone = Instantiate(_shotPrefab);
            SetShotsData(clone, i);
            RectTransform cloneRectTransform = clone.GetComponent<RectTransform>();
            cloneRectTransform.SetParent(transform);
            shotXposition = 100;
            if (i % 2 == 0) { shotXposition = 0; }
            if (i != 0) { shotYposition += -400; }
            cloneRectTransform.anchoredPosition = new Vector2(shotXposition, shotYposition);
        }
    }

    void SetMoveTarget()
    {
        if (!_isInitial) { return;}
        _moveTarget = new Vector2(transform.position.x, transform.position.y);
        _moveTarget.y += _shotListRectTransform.sizeDelta.y + (_shotListRectTransform.sizeDelta.y / 2);
    } 

    void SetShotsData(GameObject shot ,int i)
    {
        Image shotImage = shot.GetComponentInChildren<Image>();
        TMP_Text shotPoint = shot.GetComponentInChildren<TMP_Text>();

        shotImage.sprite = ScoreStoring.instance.ScreenShotsTextures[i];
        shotPoint.text = ScoreStoring.instance.ScreenShotsPoints[i] + " p";
    }
}
