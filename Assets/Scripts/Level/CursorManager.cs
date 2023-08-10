using Surreily.SomeWords.Scripts.Materials;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Level {
    public class CursorManager : MonoBehaviour {
        private MovableBehaviour movableBehaviour;

        public MaterialStore MaterialStore { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        #region Start

        public void Start() {
            SetUpMovableBehaviour();
            SetUpSpriteRenderer();
        }

        private void SetUpMovableBehaviour() {
            movableBehaviour = gameObject.AddComponent<MovableBehaviour>();
            movableBehaviour.speed = 15f;
            movableBehaviour.distance = 1f;
        }

        private void SetUpSpriteRenderer() {
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = MaterialStore.Ui.CursorSprite;
        }

        #endregion

        public void Move(Direction direction) {
            movableBehaviour.Move(direction);
        }

    }
}
