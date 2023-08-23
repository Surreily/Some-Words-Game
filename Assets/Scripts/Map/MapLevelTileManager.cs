using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Model.Game;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapLevelTileManager : MonoBehaviour {
        private static readonly Color ClosedLevelMultiplier = new Color(0.3f, 0.3f, 0.3f);

        private SpriteRenderer backgroundSpriteRenderer;

        [SerializeField]
        public MaterialStore MaterialStore { get; set; }

        [SerializeField]
        public LevelModel Level { get; set; }

        [SerializeField]
        public int X { get; set; }

        [SerializeField]
        public int Y { get; set; }

        [SerializeField]
        public LevelState State { get; set; }

        [SerializeField]
        public Color Color { get; set; }

        #region Start

        public void Start() {
            SetUpBackground();
        }

        private void SetUpBackground() {
            GameObject backgroundObject = new GameObject("Background");
            backgroundObject.transform.parent = gameObject.transform;
            backgroundObject.transform.localPosition = Vector3.zero;
            backgroundObject.transform.localScale = Vector3.one;

            backgroundSpriteRenderer = backgroundObject.AddComponent<SpriteRenderer>();
            Redraw();
        }

        #endregion

        public void Redraw() {
            backgroundSpriteRenderer.enabled = State != LevelState.Hidden;
            backgroundSpriteRenderer.sprite = MaterialStore.Map.GetLevelSprite();
            backgroundSpriteRenderer.color = Color;

            if (State == LevelState.Closed) {
                backgroundSpriteRenderer.color *= ClosedLevelMultiplier;
            }
        }
    }
}
