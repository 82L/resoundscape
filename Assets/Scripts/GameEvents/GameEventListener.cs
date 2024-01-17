using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameEventListener : MonoBehaviour
{
    public GameEvent @event;
    public UnityEvent response;

    private void OnEnable()
    {
        @event.RegisterListener(this);
    }
    private void OnDisable()
    {
        @event.UnregisterListener(this);
    }
    public void OnEventRaised()
    {
        response.Invoke();
    }
}
