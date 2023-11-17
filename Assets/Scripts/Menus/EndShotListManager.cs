using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndShotListManager : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] GameObject _returnButton;
    [SerializeField] GameObject _shotPrefab;
    [SerializeField] int _shotOffset;
    [SerializeField] [Range(10f, 400f)] float _shotListMoveSpeed;
    [SerializeField] bool _isInitial;

    RectTransform _shotListRectTransform;
    Vector2 _moveTarget;
    bool _moveShotList;
    int shotCount;

    IEnumerator Start()
    {
        _shotListRectTransform = GetComponent<RectTransform>();
        ScaleShotList();
        CreateShots();
        SetMoveTarget();
        yield return new WaitForSeconds(1);
        _moveShotList = true;
    }

    void Update()
    {
        if(_isInitial) 
        {
            if (_moveShotList) 
            {
                _shotListRectTransform.anchoredPosition = Vector2.MoveTowards(_shotListRectTransform.anchoredPosition, _moveTarget, _shotListMoveSpeed * Time.deltaTime);
            }
            if (_shotListRectTransform.anchoredPosition.y >= _moveTarget.y - 0.01f)
            {
                _moveShotList = false;
                _isInitial = false;
            }
            return; 
        }
        _scrollbar.enabled = true;
        _returnButton.SetActive(true);
        _shotListRectTransform.anchoredPosition = new Vector2(0, GetScrollbarY());
    }

    void ScaleShotList()
    {
        if (!ScoreStoring.instance) { return; }
        shotCount = ScoreStoring.instance.ScreenShotsTextures.Count;
        int shotListY = _shotOffset * shotCount;
        shotListY += 300 * shotCount;
        _shotListRectTransform.sizeDelta = new Vector2(1000, shotListY);
    }

    float GetScrollbarY()
    {
        if (!_scrollbar) { return _shotListRectTransform.anchoredPosition.y; }
        float shotListPosition = _scrollbar.value * (_shotListRectTransform.sizeDelta.y - 850);
        return shotListPosition;
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
            cloneRectTransform.localScale = Vector3.one;
            shotXposition = 100;
            if (i % 2 == 0) { shotXposition = 0; }
            if (i != 0) { shotYposition += -400; }
            cloneRectTransform.anchoredPosition = new Vector2(shotXposition, shotYposition);
        }
        ScoreStoring.instance.ScreenShotsTextures.Clear();
    }

    void SetMoveTarget()
    {
        if (!_isInitial) { return;}
        _moveTarget = new Vector2(_shotListRectTransform.anchoredPosition.x, _shotListRectTransform.rect.height - 850);
    } 

    void SetShotsData(GameObject shot ,int i)
    {
        Image shotImage = shot.GetComponentInChildren<Image>();
        TMP_Text shotPoint = shot.GetComponentInChildren<TMP_Text>();

        shotImage.sprite = ScoreStoring.instance.ScreenShotsTextures[i];
        shotPoint.text = ScoreStoring.instance.ScreenShotsPoints[i] + " p";
    }
}
