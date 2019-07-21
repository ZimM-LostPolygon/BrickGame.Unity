using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace BrickGame
{
    public class BrickGameView : MonoBehaviour
    {
        private BrickGame _brickGame;
        private BlockView[][] _playfieldBlocks;

        private List<BlockView> _borderBlocks = new List<BlockView>();

        public Color BorderColor;
        public GameObject BlockPrefab;

        public KeyCode MoveLeftKeyCode;
        public KeyCode MoveRightKeyCode;
        public KeyCode RotateClockwiseKeyCode;
        public KeyCode RotateCounterClockwiseKeyCode;
        public KeyCode SoftDropKeyCode;
        public KeyCode HardDropKeyCode;

        public int VisiblePlayfieldHeight;
        public int PlayfieldWidth;
        public int PlayfieldHeight;
        public float Gravity = 0.3f;

        public Text ScoreText;

        private void Start()
        {
            _brickGame = new BrickGame(PlayfieldWidth, PlayfieldHeight, VisiblePlayfieldHeight, Gravity);
            _brickGame.Start();

            _playfieldBlocks =
                BrickGameUtility.Create2DArray<BlockView>(_brickGame.PlayfieldWidth, _brickGame.VisiblePlayfieldHeight);

            for (int i = 0; i < _brickGame.VisiblePlayfieldHeight; i++)
            {
                Vector2Int position = new Vector2Int(-1, i);
                _borderBlocks.Add(CreateBlock(position));
                position = new Vector2Int(_brickGame.PlayfieldWidth, i);
                _borderBlocks.Add(CreateBlock(position));
            }

            for (int i = -1; i < _brickGame.PlayfieldWidth + 1; i++)
            {
                Vector2Int position = new Vector2Int(i, -1);
                _borderBlocks.Add(CreateBlock(position));
            }

            _borderBlocks.ForEach(block => block.Color = BorderColor);

            for (int i = 0; i < _playfieldBlocks.Length; i++)
            {
                for (int j = 0; j < _playfieldBlocks[i].Length; j++)
                {
                    _playfieldBlocks[i][j] = CreateBlock(new Vector2Int(i, j));
                }
            }
        }

        public void Update()
        {
            _brickGame.Update(Time.deltaTime);
            ScoreText.text = $"<b>Score:</b> {_brickGame.Score}";

            PlayfieldCell[][] compositeField = _brickGame.CalculateCompositeField();
            for (int i = 0; i < _playfieldBlocks.Length; i++)
            {
                for (int j = 0; j < _playfieldBlocks[i].Length; j++)
                {
                    _playfieldBlocks[i][j].SetVisible(compositeField[i][j].Value != 0);
                    _playfieldBlocks[i][j].Color = compositeField[i][j].Color;
                }
            }

            if (Input.GetKeyDown(RotateClockwiseKeyCode))
            {
                _brickGame.RotateActivePiece(PieceRotationDirection.Clockwise);
            }

            if (Input.GetKeyDown(RotateCounterClockwiseKeyCode))
            {
                _brickGame.RotateActivePiece(PieceRotationDirection.CounterClockwise);
            }

            if (Input.GetKeyDown(MoveLeftKeyCode))
            {
                _brickGame.MoveActivePiece(false);
            }

            if (Input.GetKeyDown(MoveRightKeyCode))
            {
                _brickGame.MoveActivePiece(true);
            }

            if (Input.GetKeyDown(SoftDropKeyCode))
            {
                _brickGame.DoGravityStep();
            }
        }

        private BlockView CreateBlock(Vector2Int position)
        {
            GameObject gameObject = Instantiate(BlockPrefab, (Vector2) position, Quaternion.identity);
            BlockView blockView = gameObject.GetComponent<BlockView>();
            return blockView;
        }
    }
}