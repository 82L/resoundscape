using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioRecorder 
{
    // Boolean flags shows if the microphone is connected   
    private static bool _micConnected = false;    
    
    //The maximum and minimum available recording frequencies    
    private static int _minFreq;    
    private static int _maxFreq;
    

    private static AudioClip _currentAudioClip;
    // Start is called before the first frame update
    private static void  Initialize()
    {
        if (_micConnected)
        {
            return;
        }
        //Check if there is at least one microphone connected    
        if(Microphone.devices.Length <= 0)    
        {    
            //Throw a warning message at the console if there isn't    
            Debug.LogWarning("Microphone not connected!");    
        }    
        else //At least one microphone is present    
        {
           
            //Set our flag 'micConnected' to true    
            _micConnected = true;    
           
            //Get the default microphone recording capabilities    
            Microphone.GetDeviceCaps(null, out _minFreq, out _maxFreq);    
    
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...    
            if(_minFreq == 0 && _maxFreq == 0)    
            {    
                //...meaning 44100 Hz can be used as the recording sampling rate    
                _maxFreq = 44100;    
            }    
            
        }    
    }
    

    public  static void StartRecording(AudioClip startClip)
    {
        _currentAudioClip = startClip;
        Initialize();
        if(_micConnected)    
        {    
            //If the audio from any microphone isn't being captured    
            if(!Microphone.IsRecording(null))
            {
                _currentAudioClip = null;
                //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource    
                _currentAudioClip= Microphone.Start(null, true, 20, _maxFreq);    
                
            }    
           
        }    
        else // No microphone    
        {
            //Print a red "Microphone not connected!" message at the center of the screen    
        }    
    }

    public static AudioClip EndRecording()
    {
        Microphone.End(null); //Stop the audio recording    
        Debug.Log(_currentAudioClip);
        return _currentAudioClip;
    }
 
}
