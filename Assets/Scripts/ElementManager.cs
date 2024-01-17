using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSourceManager))]
public class ElementManager : MonoBehaviour
{
    private AudioSourceManager _audioSourceManager;

    private bool _isPointed = false;
    private Outline _outline;
    [SerializeField] private GameEvent onModelPointed;
    [SerializeField] private GameEvent onModelUnpointed;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSourceManager = GetComponent<AudioSourceManager>();
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineHidden;
        _outline.OutlineColor = Color.black;
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
            _isPointed = true;
            onModelPointed.Raise();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
        }
    }

    public void NotPointedAt()
    {
        if (_isPointed)
        {
            _isPointed = false;
            onModelUnpointed.Raise();
            _outline.OutlineMode = Outline.Mode.OutlineHidden;

        }
    }
  
}
