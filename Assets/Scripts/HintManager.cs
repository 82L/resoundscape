using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CanvasGroup recordingHint;
    [SerializeField] private CanvasGroup pointingHint;

    private void Start()
    {
        pointingHint.DOFade(0f, 0f);
        recordingHint.DOFade(0f, 0f);
    }

    private bool _pointingHintShowed = false;
    public void ShowPointingHint()
    {
        recordingHint.DOFade(0f, 0.3f);
        pointingHint.DOFade(1f, 0.3f);
        _pointingHintShowed = true;
    }

    public void HidePointingHint()
    {
        pointingHint.DOFade(0f, 0.3f);
        _pointingHintShowed = false;

    }
    public void HideRecordingHint()
    {
        recordingHint.DOFade(0f, 0.3f);

        if (_pointingHintShowed)
        {
            pointingHint.DOFade(1f, 0.3f);
        }
    }

    public void ShowRecordingHint()
    {
        pointingHint.DOFade(0f, 0.3f);
        recordingHint.DOFade(1f, 0.3f);
    }


}
