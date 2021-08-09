using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Draws a health bar of a character.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        /// <summary>
        /// The offset with which to render the health bar.
        /// </summary>
        [Range(0.1f, 0.5f)]
        public float OffsetY;

        /// <summary>
        /// The sprite renderer.
        /// </summary>
        private SpriteRenderer sprite;

        /// <summary>
        /// The line renderer.
        /// </summary>
        private LineRenderer line;

        private void Start()
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.useWorldSpace = false;

            sprite = GetComponentInParent<SpriteRenderer>();

            var character = GetComponentInParent<Character>();

            character.OnHealthChange += (_, __, fraction) =>
            {
                DrawHealthBar(fraction);
            };
        }

        /// <summary>
        /// Draws the health bar.
        /// </summary>
        public void DrawHealthBar(float healthFraction)
        {
            Debug.Log($"Drawing health bar for fraction {healthFraction}");

            var spriteExtentX = sprite.sprite.bounds.extents.x;
            var spriteExtentY = sprite.sprite.bounds.extents.y;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * healthFraction;

            line.SetPosition(0, new Vector3(start, spriteExtentY + OffsetY, 0));
            line.SetPosition(1, new Vector3(start + width, spriteExtentY + OffsetY, 0));
        }
    }
}
