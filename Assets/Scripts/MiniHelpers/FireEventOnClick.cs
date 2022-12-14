using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireEventOnClick : MonoBehaviour, IPointerDownHandler
{
    public GameObject gO;
    public string messageToSend;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        gO.SendMessage(messageToSend);
    }
}
