using UnityEngine;
using System;
using System.Collections.Generic;

namespace EventHandler {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class BindListenerAttribute : Attribute {
        #region CONSTRUCTOR
        /// <summary>
        /// Tells the compiler that this object is speaking to another
        /// </summary>
        /// <param name="key">Key to identify the speaker</param>
        /// <param name="type">The type of listener</param>
        public BindListenerAttribute (
            String key,             // Key to identify the listener
            System.Type type        // The type of listener
            )
            { this.key = key; listener = GameObject.FindObjectsOfType(type) as EventListener[]; }
        #endregion

        #region ATTRIBUTES
        // Holds the EventListener
        protected EventListener[] listener;
        public EventListener[] Listener { get { return this.listener; } }

        // Holds the Key
        protected String key;
        public String Key { get { return this.key; } }
        #endregion
    }

    #region SUMMARY
    /// <summary>
    /// Event Listener is a class that can be inherited from to allow for event handling
    /// </summary>
    #endregion
    public class EventListener : MonoBehaviour {
        #region DELEGATES
        /// <summary>
        /// Void callback
        /// </summary>
        public delegate void v_func ();

        /// <summary>
        /// float callback
        /// </summary>
        /// <param name="value">float value</param>
        public delegate void f_func (float value);

        /// <summary>
        /// string callback
        /// </summary>
        /// <param name="value">string value</param>
        public delegate void s_func (string value);

        /// <summary>
        /// Object callback
        /// </summary>
        /// <param name="value">Object value</param>
        public delegate void o_func (UnityEngine.Object value);

        /// <summary>
        /// Object array callback
        /// </summary>
        /// <param name="value">Object array</param>
        public delegate void oa_func (UnityEngine.Object[] value);

        #endregion

        #region VIRTUAL FUNCTIONS
        /// <summary>
        /// Handles events and returns a result
        /// </summary>
        /// <param name="e">Event type</param>
        virtual public bool HandleEvent (
            GameEvent e                // Event type
                                        // Null Input
            )
            // Base return value is FAILURE
            { return false; }

        /// <summary>
        /// Handles events and returns a result
        /// </summary>
        /// <param name="e">Event type</param>
        /// <param name="value">Input</param>
        virtual public bool HandleEvent (
            GameEvent e,               // Event type
            float value                 // Input
            )
            // Base return value is FAILURE
            { return false; }

        /// <summary>
        /// Handles events and returns a result
        /// </summary>
        /// <param name="e">Event type</param>
        /// <param name="value">Input</param>
        virtual public bool HandleEvent (
            GameEvent e,               // Event type
            UnityEngine.Object value    // Input
            )
            // Base return value is FAILURE
            { return false; }

        /// <summary>
        /// Handles events and returns a result
        /// </summary>
        /// <param name="e">Event type</param>
        /// <param name="func">Void callback</param>
        virtual public bool HandleEvent (
            GameEvent e,               // Event type
            v_func func                 // Void callback
            )
            // Base return value is FAILURE
            { return false; }

        /// <summary>
        /// Handles events and returns a result
        /// </summary>
        /// <param name="e">Event type</param>
        /// <param name="func">float callback</param>
        /// <param name="f">float value</param>
        virtual public bool HandleEvent (
            GameEvent e,               // Event type
            f_func func,                // float callback
            float f = 0f                // float value
            )
            // Base return value is FAILURE
            { return false; }

        /// <summary>
        /// Handles events and returns a result
        /// </summary>
        /// <param name="e">Event type</param>
        /// <param name="func">string callback</param>
        /// <param name="s">string value</param>
        virtual public bool HandleEvent (
            GameEvent e,               // Event type
            s_func func,                // string callback
            string s = ""               // string value
            )
            // Base return value is FAILURE
            { return false; }

        /// <summary>
        /// Handles events and returns a result
        /// </summary>
        /// <param name="e">Event type</param>
        /// <param name="func">object callback</param>
        /// <param name="o">object value</param>
        virtual public bool HandleEvent (
            GameEvent e,               // Event type
            o_func func,                // object callback
            UnityEngine.Object o = null // object value
            )
            // Base return value is FAILURE
            { return false; }

        /// <summary>
        /// Handles events and returns a result
        /// </summary>
        /// <param name="e">Event type</param>
        /// <param name="func">object callback</param>
        /// <param name="o">object value</param>
        virtual public bool HandleEvent (
            GameEvent e,                   // Event type
            oa_func func,                   // object callback
            UnityEngine.Object[] oa = null  // object array value
            )
            // Base return value is FAILURE
            { return false; }

        #endregion
    }

    #region SUMMARY
    /// <summary>
    /// Event Handle is a class that can be inherited from to allow for event management.
    /// --Inherits from Event Listener--
    /// </summary>
    #endregion
    public class EventHandle : EventListener {
        #region PRIVATE VARABLES
        private BindListenerAttribute listeningToAttr;
        public BindListenerAttribute ListeningToAttr { get { return listeningToAttr; } }
        private Dictionary<String, EventListener[]> listeners = new Dictionary<String, EventListener[]>();
        public Dictionary<String, EventListener[]> Listeners { get { return listeners; } }
        #endregion

        #region FUNCTIONS
        /// <summary>
        /// Call the __BindEventListeners on start
        /// </summary>
        public virtual void Awake () { __BindEventListeners(); }

        /// <summary>
        /// Binds all the Listeners to this object
        /// </summary>
        private bool __BindEventListeners () {
            foreach (Attribute attr in this.GetType().GetCustomAttributes(true)) {
                listeningToAttr = attr as BindListenerAttribute;
                if (listeningToAttr != null) {
                    listeners.Add(listeningToAttr.Key, listeningToAttr.Listener);
                }
            }
            return true;
        }

        /// <summary>
        /// Returns a listener from the key
        /// </summary>
        /// <param name="Key">Key to identify the listener</param
        /// <param name="index">Index to identify the position</param>
        public EventListener GetEventListener (
            String Key,                 // Key to identify the listener
            int index                   // Index
            )
            { return listeners[Key][index]; }

        /// <summary>
        /// Returns a listener from the key
        /// </summary>
        /// <param name="Key">Key to identify the listener</param
        public EventListener GetEventListener (
            String Key                  // Key to identify the listener
            )
            { return listeners[Key][0]; }

        /// <summary>
        /// Returns a listener from the key
        /// </summary>
        /// <param name="Key">Key to identify the listener</param
        public EventListener GetEventListener (
            String Key,                 // Key to identify the listener
            UnityEngine.GameObject obj
            )
            {
            foreach (EventListener e in listeners[Key]) {
                if (e.gameObject == obj) return listeners[Key][0];
            } return null;
        }

        /// <summary>
        /// Returns a listener from the object
        /// </summary>
        /// <param name="obj">Object to get the listener from</param
        public EventListener GetEventListener (
            UnityEngine.GameObject obj  //Object to get the listener from
            )
            { return obj.GetComponent<EventListener>(); }

        /// <summary>
        /// Updates a type for the dictionary
        /// </summary>
        /// <param name="Key">Key to identify the listener</param>
        /// <param name="type">Type to update</param>
        /// <returns></returns>
        public bool UpdateBindedList(
            string Key,                 // Key to identify the listener
            System.Type type            // Type to update
            )
            { listeners[Key] =  GameObject.FindObjectsOfType(type) as EventListener[]; return true; }
        

        #endregion
    }

    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(EventHandle), true)]
    public class EventHandleEditor : UnityEditor.Editor {
        #region PRIVATE VARIABLES
        // Checks if we should show the listeners
        public bool Showboundlisteners;
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Override OnInspectorGUI
        /// </summary>
        public override void OnInspectorGUI () {
            if (Showboundlisteners = UnityEditor.EditorGUILayout.ToggleLeft("Show listeners", Showboundlisteners)) {
                EventHandle m_eventHandle = (EventHandle)target;
                UnityEditor.EditorGUILayout.LabelField("Listeners:", UnityEditor.EditorStyles.boldLabel);

                foreach (KeyValuePair<string, EventListener[]> l in m_eventHandle.Listeners)
                    for (int i = 0; i < l.Value.Length; i++) {
                        UnityEditor.EditorGUILayout.ObjectField(l.Key, l.Value[i], typeof(EventHandle), true);
                    }

                UnityEditor.EditorGUILayout.Space();
            }
            base.OnInspectorGUI();
        }
        #endregion
    }
}