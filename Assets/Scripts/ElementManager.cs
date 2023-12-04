using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSourceManager))]
public class ElementManager : MonoBehaviour
{
    private AudioSourceManager _audioSourceManager;

    private bool _isPointed = false;
    [SerializeField] private GameEvent onModelPointed;
    [SerializeField] private GameEvent onModelUnpointed;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSourceManager = GetComponent<AudioSourceManager>();
    }

    public void OnPointing(GameObject mPointedObject)
    {
        if (mPointedObject == null)
        {
            _isPointed = false;
        }

        if (mPointedObject.name == gameObject.name)
        {
            _isPointed = true;
        }
    }

    public void SetRecording(AudioClip mAudioClip, long mDuration)
    {
        _audioSourceManager.SetNewAudioClip(mAudioClip, mDuration);
    }
    public void PointedAt()
    {
        if (!_isPointed)
        {
            onModelPointed.Raise();
        }
    }

    public void NotPointedAt()
    {
        if (_isPointed)
        {
            onModelUnpointed.Raise();

        }
    }
  
}
