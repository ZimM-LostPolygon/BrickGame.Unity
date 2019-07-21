using System;
using System.Text;
using UnityEngine;

namespace BrickGame
{
    public static class BrickGameUtility
    {
        public static T[][] RotateClockwise<T>(T[][] arr)
        {
            T[][] rotated = RotateCounterClockwise(arr);
            rotated = RotateCounterClockwise(rotated);
            rotated = RotateCounterClockwise(rotated);

            return rotated;
        }

        public static T[][] RotateCounterClockwise<T>(T[][] arr)
        {
            T[][] rotated = Create2DArray<T>(arr[0].Length, arr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    int rotatedI = arr[i].Length - 1 - j;
                    int rotatedJ = i;

                    rotated[rotatedI][rotatedJ] = arr[i][j];
                }
            }

            return rotated;
        }

        public static T[][] Create2DArray<T>(int width, int height)
        {
            T[][] arr = new T[width][];
            for (int i = 0; i < width; i++)
            {
                arr[i] = new T[height];
            }

            return arr;
        }

        public static T[][] ShallowClone<T>(this T[][] arr)
        {
            T[][] clone = new T[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
            {
                clone[i] = new T[arr[i].Length];
                for (int j = 0; j < arr[i].Length; j++)
                {
                    clone[i][j] = arr[i][j];
                }
            }

            return clone;
        }

        public static (Vector2Int min, Vector2Int max) CalculateFieldBoundingBox<T>(T[][] field, Func<int, int, bool> getOccupiedFunc)
        {
            Vector2Int min = new Vector2Int(100, 100);
            Vector2Int max = new Vector2Int(-100, -100);
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    bool empty = !getOccupiedFunc(i, j);
                    if (empty)
                        continue;

                    if (j < min.x)
                    {
                        min.x = i;
                    }

                    if (j < min.y)
                    {
                        min.y = j;
                    }

                    if (i > max.x)
                    {
                        max.x = i;
                    }

                    if (j > max.y)
                    {
                        max.y = j;
                    }
                }
            }

            return (min, max);
        }

        public static string FieldToString<T>(T[][] field, Func<int, int, bool> getOccupiedFunc)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    sb.Append(getOccupiedFunc(i, j) ? "1" : "0");
                }

                sb.Append("\n");
            }

            return sb.ToString().Trim();
        }
    }
}