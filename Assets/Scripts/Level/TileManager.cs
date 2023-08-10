using Surreily.SomeWords.Scripts.Materials;
using TMPro;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Level {
    public class TileManager : MonoBehaviour {
        private MovableBehaviour movableBehaviour;
        private PulseAnimationBehaviour pulseAnimationBehaviour;

        [SerializeField]
        public MaterialStore MaterialStore { get; set; }

        [SerializeField]
        public int X { get; set; }

        [SerializeField]
        public int Y { get; set; }

        [SerializeField]
        public char Character { get; set; }

        [SerializeField]
        public TileState State { get; set; }

        #region Start

        public void Start() {
            SetUpBackground();
            SetUpCharacter();
            SetUpMovableBehaviour();
        }

        private void SetUpBackground() {
            GameObject backgroundObject = new GameObject("Background");
            backgroundObject.transform.parent = gameObject.transform;
            backgroundObject.transform.localPosition = Vector3.zero;
            backgroundObject.transform.localScale = Vector3.one;

            SpriteRenderer backgroundRenderer = backgroundObject.AddComponent<SpriteRenderer>();
            backgroundRenderer.sprite = MaterialStore.Level.DefaultTileSprite;
        }

        private void SetUpCharacter() {
            GameObject characterObject = new GameObject("Character");
            characterObject.transform.parent = gameObject.transform;
            characterObject.transform.localPosition = Vector3.zero;

            RectTransform rectTransform = characterObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;

            TMP_Text textMeshProText = characterObject.AddComponent<TextMeshPro>();
            textMeshProText.font = Resources.Load<TMP_FontAsset>("Fonts/VGA Font");
            textMeshProText.text = Character.ToString();
            textMeshProText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            textMeshProText.verticalAlignment = VerticalAlignmentOptions.Middle;
            textMeshProText.fontSize = 8f;

            pulseAnimationBehaviour = characterObject.AddComponent<PulseAnimationBehaviour>();
            pulseAnimationBehaviour.Scale = 2f;
            pulseAnimationBehaviour.Speed = 5f;
        }

        private void SetUpMovableBehaviour() {
            movableBehaviour = gameObject.AddComponent<MovableBehaviour>();
            movableBehaviour.speed = 15f;
            movableBehaviour.distance = 1f;
        }

        #endregion

        public void Move(Direction direction) {
            movableBehaviour.Move(direction);
        }

        public void Pulse() {
            pulseAnimationBehaviour.Pulse();
        }

    }
}
