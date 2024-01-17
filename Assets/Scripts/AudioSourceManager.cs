using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour
{
    [SerializeField]private bool isLooping = false;
    [SerializeField]private float minWaitBetweenPlays = 1f;
    [SerializeField]private float maxWaitBetweenPlays = 5f;
    [Space]
    [Header("Debugging")]
    [SerializeField] private float waitTimeCountdown = -1f;
    [SerializeField] private AudioClip audioClip;
    
    private long duration = 0;
    private long _timeSincePlay = 0;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        if(audioClip != null)
            _audioSource.clip = audioClip;
    }

    public void SetNewAudioClip(AudioClip mAudioClip, long mDuration)
    {
        Debug.Log(mAudioClip);
        audioClip = mAudioClip;
        _audioSource.clip = mAudioClip;
        if (isLooping)
        {
            _audioSource.loop = true;
        }

        duration = mDuration;
        _audioSource.Play();
        _timeSincePlay = new DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();
        waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
    }

    private void TryToPlayAudioClip()
    {
        long currentTime = new DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();
        if (!_audioSource.isPlaying && _audioSource.clip is not null)
        {
            if (waitTimeCountdown < 0f)
            {
                _audioSource.Play();
                _timeSincePlay = new DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();
                waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            }
            else
            {
                waitTimeCountdown -= Time.deltaTime;
            }
        }
        else if (_timeSincePlay + duration < currentTime)
        {
            _audioSource.Stop();
        }
    }
    // Update is called once per frame
    private void Update()
    {
        TryToPlayAudioClip();
    }
}
