using UnityEngine;

namespace BrickGame
{
    public struct PlayfieldCell
    {
        public int Value;
        public Color Color;

        public PlayfieldCell(int value, Color color)
        {
            Value = value;
            Color = color;
        }

        public PlayfieldCell(int value) : this(value, Color.white)
        {
        }
    }
}