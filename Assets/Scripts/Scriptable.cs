using UnityEngine;

namespace Aquapunk
{
    public class Scriptable : ScriptableObject
    {
        public int id;

        public string title;
        public string info;

        public Sprite icon;

        public Player owner;
    }
}

