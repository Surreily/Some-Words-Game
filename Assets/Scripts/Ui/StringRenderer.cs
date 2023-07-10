using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Ui {
    public class StringRenderer : MonoBehaviour {
        private static readonly Vector3 characterTranslateVector = new Vector3(1f / 16f * 4.5f, 1f / 16f * 1.5f, 0f);

        private readonly IList<GameObject> characterObjects;

        public MaterialStore MaterialStore { get; set; }

        public string Text { get; set; }

        public float HorizontalSpacing { get; set; } = 0.6f;

        public float VerticalSpacing { get; set; } = 1f;

        public int MaxCharactersPerLine { get; set; } = 20;

        public StringRenderer() : base() {
            characterObjects = new List<GameObject>();
        }

        public void Refresh() {
            ClearCharacterObjects();

            IEnumerable<string> lines = Text.SplitIntoLines(MaxCharactersPerLine);

            for (int i = 0; i < lines.Count(); i++) {
                RenderLine(lines.ElementAt(i), i);
            }
        }

        private void RenderLine(string line, int lineNumber) {
            for (int i = 0; i < line.Length; i++) {
                RenderCharacter(line[i], lineNumber, i);
            }
        }

        private void RenderCharacter(char character, int lineNumber, int characterNumber) {
            if (character == ' ') {
                return;
            }

            GameObject characterObject = new GameObject();
            characterObject.transform.parent = transform;
            characterObject.transform.localPosition = new Vector3(
                HorizontalSpacing * characterNumber,
                VerticalSpacing * -lineNumber,
                0f);
            characterObject.transform.Translate(characterTranslateVector, Space.Self);

            TileRenderer tileRenderer = characterObject.AddComponent<TileRenderer>();
            tileRenderer.Material = MaterialStore.Font.GetWhiteFontMaterial(character);

            characterObjects.Add(characterObject);
        }

        private void ClearCharacterObjects() {
            foreach (GameObject characterObject in characterObjects) {
                Destroy(characterObject);
            }

            characterObjects.Clear();
        }
    }
}
