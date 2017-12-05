using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class Card : MonoBehaviour {

    private void Awake()
    {
        var trigger = GetComponent<EventTrigger>();

        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(data => { Click(); });

        trigger.triggers.Add(entry);
    }

    public void Click() {
        var back = transform.GetChild(2).gameObject;
        back.SetActive(!back.activeSelf);
    }
}
