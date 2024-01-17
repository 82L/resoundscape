using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSourceManager))]
[RequireComponent(typeof(CapsuleCollider))]
public class ElementManager : MonoBehaviour
{
    private AudioSourceManager _audioSourceManager;

    private bool _isPointed = false;
    private Outline _outline = null;
    [SerializeField] private GameEvent onModelPointed;
    [SerializeField] private GameEvent onModelUnpointed;

    [FormerlySerializedAs("_outlineWidthMax")] [SerializeField] private float outlineWidthMax = 8f;
    [FormerlySerializedAs("_outlineWidthMin")] [SerializeField] private float outlineWidthMin = 0f;
    // Start is called before the first frame update
    private void Start()
    {
        _audioSourceManager = GetComponent<AudioSourceManager>();
        if (outlineWidthMax > 0f)
        {
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineHidden;
            _outline.OutlineColor = Color.white;
            _outline.OutlineWidth = outlineWidthMin;
        }
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
            if (_outline is not null)
            {
                _outline.OutlineMode = Outline.Mode.OutlineVisible;
                DOTween.To(() => _outline.OutlineWidth, x => _outline.OutlineWidth = x,outlineWidthMax, 0.3f );
            }
        }
    }

    public void NotPointedAt()
    {
        if (_isPointed)
        {
            _isPointed = false;
            onModelUnpointed.Raise();
            if (_outline is not null)
            {
                DOTween.To(() => _outline.OutlineWidth, x => _outline.OutlineWidth = x, outlineWidthMin, 0.3f)
                    .onComplete = RemoveOutline;
            }
           
        }
    }

    private void RemoveOutline()
    {
        _outline.OutlineMode = Outline.Mode.OutlineHidden;
    }
  
}
