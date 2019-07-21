using System;
using System.Collections.Generic;

namespace BrickGame
{
    public class PieceRandomBagGenerator
    {
        private static readonly PieceDefinition[] _definitions =
        {
            PiecesDefinitions.I,
            PiecesDefinitions.J,
            PiecesDefinitions.L,
            PiecesDefinitions.O,
            PiecesDefinitions.S,
            PiecesDefinitions.T,
            PiecesDefinitions.Z,
        };

        private List<PieceDefinition> _currentBag;
        private Random _random = new Random();

        public PieceRandomBagGenerator()
        {
            FillBag();
        }

        public PieceDefinition NextRandomPiece()
        {
            if (_currentBag.Count == 0)
            {
                FillBag();
            }

            int pieceIndex = _random.Next(0, _currentBag.Count);
            PieceDefinition pieceDefinition = _currentBag[pieceIndex];
            _currentBag.RemoveAt(pieceIndex);
            return pieceDefinition;
        }

        private void FillBag()
        {
            _currentBag = new List<PieceDefinition>(_definitions);
        }
    }
}