using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrickGame
{
    public class PieceDefinition
    {
        public int[][] Shape { get; }

        public Vector2Int[][] WallKickData { get; }

        public Color Color { get; }

        public PieceDefinition(int[][] shape, Vector2Int[][] wallKickData, Color color)
        {
            if (wallKickData.Length != 8)
                throw new ArgumentOutOfRangeException(nameof(wallKickData));

            Shape = BrickGameUtility.RotateClockwise(shape);
            //Shape = shape;
            WallKickData = wallKickData;
            Color = color;
        }

        public (Vector2Int min, Vector2Int max) CalculateBoundingBox()
        {
            return BrickGameUtility.CalculateFieldBoundingBox(Shape, (i, j) => Shape[i][j] > 0);
        }
    }

    public static class PiecesDefinitions
    {
        public static IReadOnlyDictionary<(int fromRotation, int toRotation), int> WallKickDataRotationIndex =
            new Dictionary<(int fromRotation, int toRotation), int>
            {
                {(0, 1), 0},
                {(1, 0), 1},
                {(1, 2), 2},
                {(2, 1), 3},
                {(2, 3), 4},
                {(3, 2), 5},
                {(3, 0), 6},
                {(0, 3), 7},
            };

        public static Vector2Int[][] WallKickDataGeneric =
        {
            // 0>>1
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(-1, -2),
            },
            // 1>>0
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 2),
            },
            // 1>>2
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 2),
            },
            // 2>>1
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(-1, -2),
            },
            // 2>>3
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(1, -2),
            },
            // 3>>2
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(-1, 2),
            },
            // 3>>0
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(-1, 2),
            },
            // 0>>3
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(1, -2),
            },
        };

        public static Vector2Int[][] WallKickDataIPiece =
        {
            // 0>>1
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(-1, -2),
            },
            // 1>>0
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 2),
            },
            // 1>>2
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 2),
            },
            // 2>>1
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(-1, -2),
            },
            // 2>>3
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(1, -2),
            },
            // 3>>2
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(-1, 2),
            },
            // 3>>0
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, -1),
                new Vector2Int(0, 2),
                new Vector2Int(-1, 2),
            },
            // 0>>3
            new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(0, -2),
                new Vector2Int(1, -2),
            },
        };

        public static PieceDefinition I = new PieceDefinition(
            new[]
            {
                new[] {0, 0, 0, 0},
                new[] {1, 1, 1, 1},
                new[] {0, 0, 0, 0},
                new[] {0, 0, 0, 0},
            },
            WallKickDataIPiece,
            Color.cyan
        );

        public static PieceDefinition J = new PieceDefinition(
            new[]
            {
                new[] {1, 0, 0},
                new[] {1, 1, 1},
                new[] {0, 0, 0},
            },
            WallKickDataGeneric,
            Color.blue
        );

        public static PieceDefinition L = new PieceDefinition(
            new[]
            {
                new[] {0, 0, 1},
                new[] {1, 1, 1},
                new[] {0, 0, 0},
            },
            WallKickDataGeneric,
            new Color32(255, 69, 0, 255)
        );

        public static PieceDefinition O = new PieceDefinition(
            new[]
            {
                new[] {0, 0, 0, 0},
                new[] {0, 1, 1, 0},
                new[] {0, 1, 1, 0},
                new[] {0, 0, 0, 0}
            },
            WallKickDataGeneric,
            Color.yellow
        );

        public static PieceDefinition S = new PieceDefinition(
            new[]
            {
                new[] {0, 1, 1},
                new[] {1, 1, 0},
                new[] {0, 0, 0},
            },
            WallKickDataGeneric,
            Color.green
        );

        public static PieceDefinition T = new PieceDefinition(
            new[]
            {
                new[] {0, 1, 0},
                new[] {1, 1, 1},
                new[] {0, 0, 0},
            },
            WallKickDataGeneric,
            Color.magenta
        );

        public static PieceDefinition Z = new PieceDefinition(
            new[]
            {
                new[] {1, 1, 0},
                new[] {0, 1, 1},
                new[] {0, 0, 0},
            },
            WallKickDataGeneric,
            Color.red
        );
    }
}