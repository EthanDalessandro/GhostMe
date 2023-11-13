using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _buttonXCurve;
    [SerializeField] private AnimationCurve _buttonYCurve;
    private Vector2 _buttonOrigin;
    private float _curvePosition;

    private void Awake()
    {
        _buttonOrigin = transform.position;
    }

    private void Update()
    {
        SetButtonPosition();
        UpdateCurvePosition();
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
        return _buttonOrigin.x + _buttonXCurve.Evaluate(_curvePosition);
    }

    private float SetButtonOnY()
    {
        return _buttonOrigin.y + _buttonYCurve.Evaluate(_curvePosition);
    }
}
