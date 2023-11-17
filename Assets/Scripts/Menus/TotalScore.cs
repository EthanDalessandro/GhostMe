using System.Collections;
using TMPro;
using UnityEngine;

public class TotalScore : MonoBehaviour
{
    [SerializeField][Range(0.1f, 10f)] float _startCountDelay; 
    [SerializeField][Range(0.1f, 10f)] float _countDelay;
    TMP_Text _totalPointText;
    Animator _totalPointAnimator;
    int _totalPoints;

    void Start()
    {
        _totalPointText = GetComponent<TMP_Text>();
        _totalPointAnimator = GetComponent<Animator>();
        StartCoroutine(SetTotalScore());
    }

    IEnumerator SetTotalScore()
    {
        yield return new WaitForSeconds(_startCountDelay);
        for(int i = 0; i < ScoreStoring.instance.ScreenShotsPoints.Count; i++)
        {
            _totalPointAnimator.SetBool("DoEffect", true);
            _totalPoints += ScoreStoring.instance.ScreenShotsPoints[i];
            _totalPointText.text = _totalPoints + " p";
            yield return new WaitForSeconds(0.4f);
            _totalPointAnimator.SetBool("DoEffect", false);
            yield return new WaitForSeconds(_countDelay - 0.4f);
        }
        yield return null;
        ScoreStoring.instance.ScreenShotsPoints.Clear();
    }
}
