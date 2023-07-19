using Surreily.SomeWords.Scripts.Materials;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Surreily.SomeWords.Scripts.Level {
    public class LevelUi {
        private readonly MaterialStore materialStore;

        private GameObject panelObject;
        private TextMeshProUGUI goalText;

        public LevelUi(MaterialStore materialStore) {
            this.materialStore = materialStore;
        }

        public void BuildUi() {
            GameObject canvasObject = GameObject.Find("Canvas");

            panelObject = new GameObject("Panel");
            panelObject.transform.parent = canvasObject.transform;

            Image image = panelObject.AddComponent<Image>();
            image.sprite = materialStore.Ui.PanelSprite;
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 0.25f;
            image.rectTransform.anchorMin = new Vector2(0f, 1f);
            image.rectTransform.anchorMax = new Vector2(1f, 1f);
            image.rectTransform.pivot = new Vector2(0.5f, 1f);
            image.rectTransform.offsetMin = new Vector2(8f, -56f);
            image.rectTransform.offsetMax = new Vector2(-8f, -8f);

            GameObject levelTitleObject = new GameObject("Level Title");
            levelTitleObject.transform.parent = panelObject.transform;

            goalText = levelTitleObject.AddComponent<TextMeshProUGUI>();
            goalText.text = "Test";
            goalText.font = materialStore.Font.VgaFont;
            goalText.fontSize = 32f;
            goalText.rectTransform.anchorMin = new Vector2(0f, 1f);
            goalText.rectTransform.anchorMax = new Vector2(1f, 1f);
            goalText.rectTransform.pivot = new Vector2(0.5f, 1f);
            goalText.rectTransform.offsetMin = new Vector2(8f, -40f);
            goalText.rectTransform.offsetMax = new Vector2(-8f, -8f);
        }

        public void DestroyUi() {
            GameObject.Destroy(panelObject);

            panelObject = null;
            goalText = null;
        }
    }
}
