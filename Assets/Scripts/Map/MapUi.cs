using Surreily.SomeWords.Scripts.Materials;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapUi : MonoBehaviour {
        private GameObject mapUiObject;
        private TextMeshProUGUI levelTitleText;

        public MaterialStore MaterialStore { get; set; }

        public void EnableUi() {
            GameObject canvasObject = GameObject.Find("Canvas");

            mapUiObject = new GameObject("Map UI");
            mapUiObject.transform.parent = canvasObject.transform;

            GameObject panelObject = new GameObject("Panel");
            panelObject.transform.parent = canvasObject.transform;

            Image image = panelObject.AddComponent<Image>();
            image.sprite = MaterialStore.Ui.PanelSprite;
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 0.25f;
            image.rectTransform.anchorMin = new Vector2(0f, 1f);
            image.rectTransform.anchorMax = new Vector2(1f, 1f);
            image.rectTransform.pivot = new Vector2(0.5f, 1f);
            image.rectTransform.offsetMin = new Vector2(8f, -56f);
            image.rectTransform.offsetMax = new Vector2(-8f, -8f);

            GameObject levelTitleObject = new GameObject("Level Title");
            levelTitleObject.transform.parent = panelObject.transform;

            levelTitleText = levelTitleObject.AddComponent<TextMeshProUGUI>();
            levelTitleText.text = "Test";
            levelTitleText.font = MaterialStore.Font.VgaFont;
            levelTitleText.fontSize = 32f;
            levelTitleText.rectTransform.anchorMin = new Vector2(0f, 1f);
            levelTitleText.rectTransform.anchorMax = new Vector2(1f, 1f);
            levelTitleText.rectTransform.pivot = new Vector2(0.5f, 1f);
            levelTitleText.rectTransform.offsetMin = new Vector2(8f, -40f);
            levelTitleText.rectTransform.offsetMax = new Vector2(-8f, -8f);
        }

        public void DisableUi() {
            Destroy(mapUiObject);

            mapUiObject = null;
            levelTitleText = null;
        }

        public void SetLevelTitleText(string text) {
            levelTitleText.text = text;
        }

        public void ClearLevelTitleText() {
            levelTitleText.text = string.Empty;
        }
    }
}
