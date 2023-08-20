using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Model.Game;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapPathTileManager : MonoBehaviour {
        private static readonly Color ClosedPathMultiplier = new Color(0.3f, 0.3f, 0.3f);

        [SerializeField]
        public MaterialStore MaterialStore { get; set; }

        [SerializeField]
        public int X { get; set; }

        [SerializeField]
        public int Y { get; set; }

        [SerializeField]
        public PathTileType Type { get; set; }

        [SerializeField]
        public PathState State { get; set; }

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

            SpriteRenderer backgroundSpriteRenderer = backgroundObject.AddComponent<SpriteRenderer>();
            backgroundSpriteRenderer.sprite = MaterialStore.Map.GetPathSprite(Type);
            backgroundSpriteRenderer.color = Color;

            if (State == PathState.Closed) {
                backgroundSpriteRenderer.color *= ClosedPathMultiplier;
            }
        }

        #endregion

    }
}
