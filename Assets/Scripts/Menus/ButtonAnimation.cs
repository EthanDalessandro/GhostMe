using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    private enum CurveSign { POSITIVE, NEGATIVE }

    [SerializeField] private CurveSign _curveSign;
    [SerializeField] private AnimationCurve _buttonXCurve;
    [SerializeField] private AnimationCurve _buttonYCurve;

    private Vector2 _buttonOrigin;
    private float _curvePosition;
    private int _sign;

    private void Awake()
    {
        _buttonOrigin = transform.position;
    }

    private void Update()
    {
        GetCurveSign();
        SetButtonPosition();
        UpdateCurvePosition();
    }

    private void GetCurveSign()
    {
        switch (_curveSign)
        {
            case CurveSign.POSITIVE: _sign = 1; break;
            case CurveSign.NEGATIVE: _sign = -1; break;
            default: _sign = 1; break;
        }
    }

    private void SetButtonPosition()
    {
        Vector2 buttonPosition = transform.position;
        buttonPosition.x = SetButtonOnX();
        buttonPosition.y = SetButtonOnY();
        transform.position = buttonPosition;
    }

    private void UpdateCurvePosition()
    {
        _curvePosition += Time.deltaTime;
        if (_curvePosition >= 8)
            _curvePosition = 0;
    }

    private float SetButtonOnX()
    {
        return _buttonOrigin.x + (_buttonXCurve.Evaluate(_curvePosition) * _sign);
    }

    private float SetButtonOnY()
    {
        return _buttonOrigin.y + (_buttonYCurve.Evaluate(_curvePosition) * _sign);
    }
}
