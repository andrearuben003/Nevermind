using System;
using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;

namespace Nevermind.UI
{
    [CreateAssetMenu(fileName = "Memory Graphics Settings", menuName = "Scriptable Objects/Memory Graphics Settings")]
    public class MemoryGraphicsSettingsSO : ScriptableObject
    {
        [SerializeField]
        private List<SerTuple<Memory, Sprite>> memoryIcons = new ();
        
        public readonly Dictionary<Memory, Sprite> MemoryIcons = new Dictionary<Memory, Sprite>();

        private void OnValidate()
        {
            MemoryIcons.Clear();
            foreach (var memoryIcon in memoryIcons)
                MemoryIcons.Add(memoryIcon.Item1, memoryIcon.Item2);
        }

        [Serializable]
        private class SerTuple<T1, T2>
        {
            public T1 Item1;
            public T2 Item2;
        }
    }
}