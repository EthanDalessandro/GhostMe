using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    [SerializeField] private float _baterryTimer;
    [SerializeField] private float _remainingTime;
    [SerializeField] private TMP_Text _batteryLeftTimer;
    private Slider _batterySlider;
    private TMP_Text _batteryPercent;

    private void Awake()
    {
        _batteryPercent = GetComponentInChildren<TMP_Text>();

        _batterySlider = GetComponent<Slider>();
        _remainingTime = _baterryTimer * 60;
        _batterySlider.maxValue = _remainingTime;
    }

    private void Update()
    {
        _remainingTime -= Time.deltaTime;
        UpdateBatterySliderValue();
        UpdateBatteryPercent();
        UpdateTimeLeft();
    }

    private void UpdateBatterySliderValue()
    {
        if (_batterySlider == null) { return; }
        _batterySlider.value = _remainingTime;
    }

    public void UpdateBatteryPercent()
    {
        if (_batteryPercent == null) { return; }
        int percent = 100 * (int)_batterySlider.value;
        percent /= (int)_batterySlider.maxValue;
        _batteryPercent.text = percent + "%";
    }

    private void UpdateTimeLeft()
    {
        if (_batteryLeftTimer == null) { return; }
        int leftTime = (int)_remainingTime;
        if (leftTime >= 60)
        {
            _batteryLeftTimer.text = leftTime / 60 + "m";
            return;
        }
        _batteryLeftTimer.text = leftTime + "s";
    }
}
