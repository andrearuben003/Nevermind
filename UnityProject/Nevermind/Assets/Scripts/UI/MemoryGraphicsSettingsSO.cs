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
        private List<SerTuple<MemoryType, Color>> typeColors = new ();
        [SerializeField]
        private List<SerTuple<MemoryKey, Sprite>> memoryIcons = new ();
        
        public readonly Dictionary<MemoryType, Color> TypeColors = new Dictionary<MemoryType, Color>();
        public readonly Dictionary<MemoryKey, Sprite> MemoryIcons = new Dictionary<MemoryKey, Sprite>();

        private void OnValidate()
        {
            TypeColors.Clear();
            foreach (var typeColor in typeColors)
                TypeColors.Add(typeColor.Item1, typeColor.Item2);
            
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