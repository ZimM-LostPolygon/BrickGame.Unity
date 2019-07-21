using UnityEngine;

namespace BrickGame
{
    public class BlockView : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        public Color Color
        {
            get => _spriteRenderer.color;
            set => _spriteRenderer.color = value;
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}