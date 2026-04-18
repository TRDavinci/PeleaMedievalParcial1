using System.Collections.Generic;
using UnityEngine;


public enum EventType
{
    OnWeaponDropped,
    OnSpawnRequest
}

public class EventManager : MonoBehaviour
{
   
        public delegate void MethodToSubscribe(params object[] parameters);
        static Dictionary<EventType, MethodToSubscribe> _events;

        public static void SubscribeToEvent(EventType eventType, MethodToSubscribe methodToSubscribe)
        {
            if (_events == null) _events = new Dictionary<EventType, MethodToSubscribe>();

            _events.TryAdd(eventType, null);

            _events[eventType] += methodToSubscribe;
        }

        public static void UnsubscribeToEvent(EventType eventType, MethodToSubscribe methodToSubscribe)
        {
            if (_events == null) return;

            if (!_events.ContainsKey(eventType)) return;

            _events[eventType] -= methodToSubscribe;
        }

        public static void TriggerEvent(EventType eventType, params object[] parameters)
        {
            if (_events == null) return;

            if (!_events.ContainsKey(eventType)) return;

            if (_events[eventType] == null) return;

            _events[eventType](parameters);
        }
    

}
