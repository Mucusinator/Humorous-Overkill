using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FranciscoRomano.UI
{
    public class UIObject : MonoBehaviour
    {
        public enum TYPE
        {
            TEXT,
            IMAGE,
            BUTTON,
            IMAGE_ARRAY,
        }

        public TYPE type;
        public GameEvent gameEvent;
    }
}