using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Model.Game;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapLevelTileManager : MonoBehaviour {
        [SerializeField]
        public MaterialStore MaterialStore { get; set; }

        [SerializeField]
        public LevelModel Level { get; set; }

        [SerializeField]
        public int X { get; set; }

        [SerializeField]
        public int Y { get; set; }

        [SerializeField]
        public bool IsOpen { get; set; }

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
            backgroundSpriteRenderer.sprite = MaterialStore.Map.GetLevelSprite();
        }

        #endregion
    }
}
