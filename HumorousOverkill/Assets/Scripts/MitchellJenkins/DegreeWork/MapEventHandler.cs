///////////////////////////////////////////////////////
//
// Map Event Handler
//
///////////////////////////////////////////////////////

#define DEBUG_LOG
using System;
using UnityEngine;
#if DEBUG_LOG
public static class _Debug {
    public static void Log(string msg) { Debug.Log(msg); }
}
#else
public static class _Debug {
    public static void Log (string msg) { }
}
#endif

// Event Handle
public delegate void __eHandle<S, E> (S sender, E eventArgs);

// Event Args
public class __eArg<_T> {
    /// <summary>
    /// Arguments for the event
    /// </summary>
    /// <param name="sender">Object that sent the event</param>
    /// <param name="target">Object that is being targeted</param>
    /// <param name="value">Value that is being sent</param>
    /// <param name="type">Type of object that send the event</param>
    public __eArg (
        _T sender,              // Object that sent the event
        System.Object target,   // Object that is being targeted
        System.Object value,    // Value that is being sent
        System.Type type        // Type of object that send the event
        ) { this.arg = sender; this.target = target; this.value = value; this.type = type; }
    public System.Object target { get; private set; }
    public System.Object value { get; private set; }
    public System.Type type { get; private set; }
    public _T arg { get; private set; }
}

public static class EventHandlingSystem {
    public static void Add(__eHandle<System.Object, __eArg<GameEvent>> func) {
        __event<GameEvent>.HandleEvent += func;
    }
    public static void Invoke (System.Object sender, System.Object target, GameEvent e) {
        __event<MapState>.InvokeEvent(
                sender,
                new __eArg<MapState>(
                    MapState.NOTENABLED,
                    __event<MapState>.SendToAll,
                    null,
                    null));
    }
}

// Event
public class __event<_T> {
    /// <summary>
    /// Empty object that tells the handler that it should be read by all
    /// </summary>
    public static System.Object SendToAll;
    /// <summary>
    /// The variable that stores all the events
    /// </summary>
    public static event __eHandle<System.Object, __eArg<_T>> HandleEvent = null;
    /// <summary>
    /// Invokes an event using the arguments that where passed
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">arguments for the event</param>
    public static void InvokeEvent (
        System.Object sender,   // The sender of the event
        __eArg<_T> e            // arguments for the event
        ) { if (HandleEvent != null) HandleEvent(sender, e); }
}

// Event states that are passed as events
public enum MapState{
    PING,
    NOTENABLED,
    INIT,
    CLOSE,
    MINIMAP,
    FULLSCREENMAP
}

class MapEventHandler : MonoBehaviour {
    public bool isEnabled = true;

    private void Awake () {
        // Raise and Invoke Event
        if (isEnabled)
            __event<MapState>.HandleEvent += new __eHandle<System.Object, __eArg<MapState>>(OnHandleEvent);
        else
            // !!!!!!!!!!!!!!!!!! WARNING !!!!!!!!!!!!!!!!!!
            // If isEnabled is false, invoke NOTENABLED event
            // Doing so will disable all that handle this event
            // !!!!!!!!!!!!!!!!!! WARNING !!!!!!!!!!!!!!!!!!
            __event<MapState>.InvokeEvent(
                this,
                new __eArg<MapState>(
                    MapState.NOTENABLED,
                    __event<MapState>.SendToAll,
                    null,
                    GetType() ));

        // Ping its location to all others that are listening
        __event<MapState>.InvokeEvent(
            this,
            new __eArg<MapState>(
                MapState.PING,
                this,
                this,
                GetType() ));
    }

    private void Start () {
        // Invoke event for init
        __event<MapState>.InvokeEvent(
            this,
            new __eArg<MapState>(
                MapState.INIT,
                __event<MapState>.SendToAll,
                null,
                GetType() ));
    }

    public void Update () {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            // Invoke event for fullscreen Map
            __event<MapState>.InvokeEvent(
                this,
                new __eArg<MapState>(
                    MapState.FULLSCREENMAP,
                    __event<MapState>.SendToAll,
                    null,
                    GetType() ));
        } if (Input.GetKeyUp(KeyCode.Tab)) {
            // Invoke event for mini Map
            __event<MapState>.InvokeEvent(
                this,
                new __eArg<MapState>(
                    MapState.MINIMAP,
                    __event<MapState>.SendToAll,
                    null,
                    GetType() ));
        }
    }

    private void OnApplicationQuit () {
        // Invoke event for close
        __event<MapState>.InvokeEvent(
            this,
            new __eArg<MapState>(
                MapState.CLOSE,
                __event<MapState>.SendToAll,
                null,
                GetType() ));
    }

    public void OnHandleEvent (object s, __eArg<MapState> e) {
        // Log Events that happen after Event has been Raised
        //if (e.target != (System.Object)this && s != (System.Object)this)
        //    _Debug.Log(":: Args (" + e.arg.ToString() + ") To <" + e.target + ">\nfrom <" + s + "> This <" + this +">");
        //if (e.target == __event<MapState>.SendToAll && s != (System.Object)this)
        //    _Debug.Log(":: Args (" + e.arg.ToString() + ") To <ALL>\nfrom <" + s + "> This <" + this + ">");
    }
}