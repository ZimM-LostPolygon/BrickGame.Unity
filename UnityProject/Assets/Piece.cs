using System;
using UnityEngine;

namespace BrickGame
{
    public class Piece
    {
        public PieceDefinition Definition { get; }
        public Vector2Int Position { get; set; }
        public int[][] Field { get; private set; }

        public int Rotation { get; private set; }

        public Piece(PieceDefinition definition, Vector2Int position)
        {
            Definition = definition;
            Position = position;

            Field = Definition.Shape.ShallowClone();
        }

        public (Vector2Int min, Vector2Int max) CalculateBoundingBox()
        {
            (Vector2Int min, Vector2Int max) = BrickGameUtility.CalculateFieldBoundingBox(Field, (i, j) => Field[i][j] > 0);
            min += Position;
            max += Position;

            return (min, max);
        }

        public void Rotate(int rotation)
        {
            Field = Definition.Shape.ShallowClone();
            for (int i = 0; i < rotation; i++)
            {
                Rotate(PieceRotationDirection.Clockwise);
            }
            Rotation = rotation;
        }

        public int CalculateNextRotation(PieceRotationDirection direction)
        {
            int rotation = Rotation;
            switch (direction)
            {
                case PieceRotationDirection.Clockwise:
                    rotation++;
                    if (rotation > 3)
                    {
                        rotation = 0;
                    }
                    break;
                case PieceRotationDirection.CounterClockwise:
                    rotation--;
                    if (rotation < 0)
                    {
                        rotation = 3;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return rotation;
        }

        public void Rotate(PieceRotationDirection direction)
        {
            Rotation = CalculateNextRotation(direction);
            switch (direction)
            {
                case PieceRotationDirection.Clockwise:
                    Field = BrickGameUtility.RotateClockwise(Field);
                    break;
                case PieceRotationDirection.CounterClockwise:
                    Field = BrickGameUtility.RotateCounterClockwise(Field);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}