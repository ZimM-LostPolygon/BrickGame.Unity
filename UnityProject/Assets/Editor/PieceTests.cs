using System.Collections;
using System.Collections.Generic;
using System.Text;
using BrickGame;
using NUnit.Framework;
using UnityEngine;

namespace BrickGame
{
    public class PieceTests
    {
        public static PieceDefinition TestPiece = new PieceDefinition(
            new[]
            {
                new[] {0, 0, 0, 1},
                new[] {1, 1, 1, 1},
                new[] {0, 0, 1, 0}
            },
            PiecesDefinitions.WallKickDataGeneric,
            Color.red
        );

        [Test]
        public void RotatePieceClockwiseTest()
        {
            Piece piece = new Piece(TestPiece, Vector2Int.zero);
            Debug.Log(PieceToString(piece));
            piece.Rotate(PieceRotationDirection.Clockwise);
            Debug.Log("----");
            Debug.Log(PieceToString(piece));
            Assert.AreEqual("010\n010\n110\n011", PieceToString(piece));
        }

        [Test]
        public void RotatePieceCounterClockwiseTest()
        {
            Piece piece = new Piece(TestPiece, Vector2Int.zero);
            Debug.Log(PieceToString(piece));
            piece.Rotate(PieceRotationDirection.CounterClockwise);
            Debug.Log("----");
            Debug.Log(PieceToString(piece));
            Assert.AreEqual("110\n011\n010\n010", PieceToString(piece));
        }

        [Test]
        public void PieceBoundingBoxTest()
        {
            Piece oPiece = new Piece(PiecesDefinitions.O, Vector2Int.zero);
            (Vector2Int min, Vector2Int max) oPieceBBox = oPiece.CalculateBoundingBox();
            Assert.AreEqual(new Vector2Int(1, 1), oPieceBBox.min);
            Assert.AreEqual(new Vector2Int(2, 2), oPieceBBox.max);

            Piece iPiece = new Piece(PiecesDefinitions.I, Vector2Int.zero);
            (Vector2Int min, Vector2Int max) iPieceBBox = iPiece.CalculateBoundingBox();
            Assert.AreEqual(new Vector2Int(1, 0), iPieceBBox.min);
            Assert.AreEqual(new Vector2Int(1, 3), iPieceBBox.max);

            Piece sPiece = new Piece(PiecesDefinitions.S, Vector2Int.zero);
            (Vector2Int min, Vector2Int max) sPieceBBox = sPiece.CalculateBoundingBox();
            Assert.AreEqual(new Vector2Int(0, 0), sPieceBBox.min);
            Assert.AreEqual(new Vector2Int(1, 2), sPieceBBox.max);
        }

        [Test]
        public void OneLineClearTest()
        {
            BrickGame brickGame = new BrickGame(3, 5, 20,1);
            for (int i = 0; i < brickGame.PlayfieldWidth; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    brickGame.Playfield[i][j] = new PlayfieldCell(1);
                }
            }

            brickGame.HandlePieceLock();
            Assert.AreEqual("00000\n00000\n00000", PlayfieldToString(brickGame.Playfield));
        }

        [Test]
        public void TwoLineClearTest()
        {
            BrickGame brickGame = new BrickGame(3, 5, 20, 1);
            for (int i = 0; i < brickGame.PlayfieldWidth; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    brickGame.Playfield[i][j] = new PlayfieldCell(1);
                }
            }

            brickGame.HandlePieceLock();
            Assert.AreEqual("00000\n00000\n00000", PlayfieldToString(brickGame.Playfield));
        }

        [Test]
        public void TwoLineAndNotchClearTest()
        {
            BrickGame brickGame = new BrickGame(3, 5, 20, 1);
            for (int i = 0; i < brickGame.PlayfieldWidth; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    brickGame.Playfield[i][j] = new PlayfieldCell(1);
                }
            }

            brickGame.Playfield[1][2] = new PlayfieldCell(1);

            brickGame.HandlePieceLock();
            Assert.AreEqual("00000\n10000\n00000", PlayfieldToString(brickGame.Playfield));
        }

        private string PlayfieldToString(PlayfieldCell[][] field)
        {
            return BrickGameUtility.FieldToString(field, (x, y) => field[x][y].Value > 0);
        }

        private string PieceToString(Piece piece)
        {
            return BrickGameUtility.FieldToString(piece.Field, (x, y) => piece.Field[x][y] > 0);
        }
    }
}