using System;

namespace Nevermind.MemorySystem
{
    [Serializable]
    public struct Memory : IEquatable<Memory>
    {
        public MemoryKey Key;
        public MemoryType Type;
        
        public Memory(MemoryKey key, MemoryType type = MemoryType.Fragile)
        {
            Key = key;
            Type = type;
        }

        public bool Equals(Memory other)
        {
            return Key == other.Key && Type == other.Type;
        }
        public override bool Equals(object obj)
        {
            if (obj is not Memory memory) return false;
            
            return Equals(memory);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Type.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Key}-{Type}";
        }
    }

    public enum MemoryKey
    {
        Test
    }

    public enum MemoryType
    {
        Permanent = 2,
        SemiPermanent = 1,
        Fragile = 0
    }
}