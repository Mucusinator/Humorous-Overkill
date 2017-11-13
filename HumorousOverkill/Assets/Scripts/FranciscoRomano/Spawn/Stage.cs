using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FranciscoRomano.Spawn
{
    [System.Serializable]
    public class Stage
    {
        // :: variables
        public int index;
        public Group current;
        public List<Group> groups;
        // :: constructors
        public Stage() : this(new List<Group>()) { }
        public Stage(Stage other) : this(other.groups) { }
        public Stage(List<Group> groups)
        {
            index = 0;
            current = null;
            this.groups = new List<Group>();
            foreach (Group wave in groups) this.groups.Add(new Group(wave));
        }
        // :: functions
        public void Next()
        {
            if (IsEmpty()) return;
            current = new Group(groups[index++]);
        }
        public void Reset()
        {
            index = 0;
            current = null;
        }
        public bool IsEmpty()
        {
            return IsGroupEmpty() && index == groups.Count;
        }
        public bool IsGroupEmpty()
        {
            return current == null || current.IsEmpty();
        }
        public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (IsGroupEmpty()) return null;
            return current.Create(position, rotation, parent);
        }
    }
}
