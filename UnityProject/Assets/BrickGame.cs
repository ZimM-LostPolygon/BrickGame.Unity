using System.Linq.Expressions;
using UnityEngine;

namespace BrickGame
{
    public class BrickGame
    {
        public int VisiblePlayfieldHeight {get;}
        public int PlayfieldWidth {get;}
        public int PlayfieldHeight {get;}
        public PlayfieldCell[][] Playfield;

        public int Score { get; private set; }
        public int Level { get; private set; }

        private Piece _activePiece;

        private float _currentGravity;
        private float _currentGravityTimer;
        private PieceRandomBagGenerator _pieceRandomBagGenerator = new PieceRandomBagGenerator();

        public Piece ActivePiece => _activePiece;

        public BrickGame(int playfieldWidth, int playfieldHeight, int visiblePlayfieldHeight, float initialGravity)
        {
            VisiblePlayfieldHeight = visiblePlayfieldHeight;
            PlayfieldWidth = playfieldWidth;
            PlayfieldHeight = playfieldHeight;
            Playfield = BrickGameUtility.Create2DArray<PlayfieldCell>(PlayfieldWidth, PlayfieldHeight);
            _currentGravity = initialGravity;
        }

        public void RotateActivePiece(PieceRotationDirection direction)
        {
            if (_activePiece == null)
                return;

            Vector2Int initialPosition = _activePiece.Position;
            int initialRotation = _activePiece.Rotation;
            int desiredRotation = _activePiece.CalculateNextRotation(direction);

            Vector2Int[] wallKickData =
                _activePiece.Definition.WallKickData[
                    PiecesDefinitions.WallKickDataRotationIndex[(initialRotation, desiredRotation)]];

            _activePiece.Rotate(direction);

            for (int i = 0; i < wallKickData.Length; i++)
            {
                Vector2Int wallKickDataTest = wallKickData[i];
                _activePiece.Position = initialPosition + wallKickDataTest;

                if (TestOverlap(_activePiece))
                {
                    return;
                }
            }

            // All failed
            _activePiece.Position = initialPosition;
            _activePiece.Rotate(initialRotation);
        }

        public void MoveActivePiece(bool isRight)
        {
            if (_activePiece == null)
                return;

            Vector2Int initialPosition = _activePiece.Position;

            Vector2Int position = _activePiece.Position;
            position.x += isRight ? 1 : -1;

            _activePiece.Position = position;
            if (!TestOverlap(_activePiece))
            {
                _activePiece.Position = initialPosition;
            }
        }

        private bool TestPieceAgainstWall(Piece piece)
        {
            (Vector2Int min, Vector2Int max) pieceBox = piece.CalculateBoundingBox();

            if (pieceBox.min.x < 0)
                return false;

            if (pieceBox.max.x >= PlayfieldWidth)
                return false;

            if (pieceBox.min.y < 0)
                return false;

            return true;
        }

        private bool TestOverlap(Piece piece)
        {
            if (!TestPieceAgainstWall(piece))
                return false;

            PlayfieldCell[][] compositeField = CalculateCompositeField();
            for (int i = 0; i < compositeField.Length; i++)
            {
                for (int j = 0; j < compositeField[i].Length; j++)
                {
                    if (compositeField[i][j].Value > 1)
                        return false;
                }
            }

            return true;
        }

        public PlayfieldCell[][] CalculateCompositeField()
        {
            PlayfieldCell[][] composite = Playfield.ShallowClone();
            if (_activePiece != null)
            {
                for (int i = 0; i < _activePiece.Field.Length; i++)
                {
                    for (int j = 0; j < _activePiece.Field[i].Length; j++)
                    {
                        Vector2Int cellPosition = new Vector2Int(i, j) + _activePiece.Position;
                        if (_activePiece.Field[i][j] != 0)
                        {
                            composite[cellPosition.x][cellPosition.y].Value++;
                            composite[cellPosition.x][cellPosition.y].Color = _activePiece.Definition.Color;
                        }
                    }
                }
            }

            return composite;
        }

        public void Start()
        {
            SpawnPiece();
        }

        public void Update(float deltaTime)
        {
            _currentGravityTimer += deltaTime;
            if (_currentGravityTimer > _currentGravity)
            {
                DoGravityStep();
                _currentGravityTimer = 0;
            }
        }

        public void DoGravityStep()
        {
            Vector2Int initialPosition = _activePiece.Position;
            Vector2Int position = _activePiece.Position;
            position.y--;
            _activePiece.Position = position;

            if (!TestOverlap(_activePiece))
            {
                _activePiece.Position = initialPosition;
                Playfield = CalculateCompositeField();
                HandlePieceLock();
                return;
            }
        }

        public void HandlePieceLock()
        {
            SpawnPiece();
            bool lineCleared = true;
            int linesClearedCount = 0;
            while (lineCleared)
            {
                lineCleared = false;
                for (int y = 0; y < PlayfieldHeight; y++)
                {
                    bool isLineFilled = true;
                    for (int x = 0; x < PlayfieldWidth; x++)
                    {
                        if (Playfield[x][y].Value == 0)
                        {
                            isLineFilled = false;
                            break;
                        }
                    }

                    if (!isLineFilled)
                        continue;

                    linesClearedCount++;
                    lineCleared = true;
                    for (int x = 0; x < PlayfieldWidth; x++)
                    {
                        Playfield[x][y].Value = 0;
                    }

                    for (int yAbove = y; yAbove < PlayfieldHeight - 1; yAbove++)
                    {
                        for (int x = 0; x < PlayfieldWidth; x++)
                        {
                            Playfield[x][yAbove] = Playfield[x][yAbove + 1];
                        }
                    }

                    break;
                }
            }

            int addedScore;
            switch (linesClearedCount)
            {
                case 1:
                    addedScore = 40 * (Level + 1);
                    break;
                case 2:
                    addedScore = 100 * (Level + 1);
                    break;
                case 3:
                    addedScore = 300 * (Level + 1);
                    break;
                case 4:
                    addedScore = 1200 * (Level + 1);
                    break;
                default:
                    addedScore = 0;
                    break;
            }

            Score += addedScore;
        }

        private void SpawnPiece()
        {
            PieceDefinition pieceDefinition = _pieceRandomBagGenerator.NextRandomPiece();
            Vector2Int spawnPosition = CalculateSpawnPosition(pieceDefinition);
            Piece piece = new Piece(pieceDefinition, spawnPosition);
            _activePiece = piece;
        }

        private Vector2Int CalculateSpawnPosition(PieceDefinition pieceDefinition)
        {
            (Vector2Int min, Vector2Int max) boundingBox = pieceDefinition.CalculateBoundingBox();
            int pieceWidth = boundingBox.max.x - boundingBox.min.x;
            return new Vector2Int(PlayfieldWidth / 2 - pieceWidth / 2, 20);
        }

        public enum State
        {
            ControllingPiece
        }
    }
}