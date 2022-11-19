using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private BoardManager boardManager;

    [SerializeField]
    public Material backgroundMaterial;

    [SerializeField]
    public Material characterMaterial;

    [SerializeField]
    public Material cursorMaterial;

    [SerializeField]
    public AudioClip cursorAudioClip;

    [SerializeField]
    public AudioClip interactAudioClip;

    [SerializeField]
    public AudioClip invalidAudioClip;

    public void Start() {
        SetUpBoardManager();

        boardManager.LoadBoard(LoadFromJson().Levels.First());
    }

    private void SetUpBoardManager() {
        boardManager = gameObject.AddComponent<BoardManager>();
        boardManager.backgroundMaterial = backgroundMaterial;
        boardManager.characterMaterial = characterMaterial;
        boardManager.cursorMaterial = cursorMaterial;
        boardManager.cursorAudioClip = cursorAudioClip;
        boardManager.interactAudioClip = interactAudioClip;
        boardManager.invalidAudioClip = invalidAudioClip;
    }

    private JsonGamePack LoadFromJson() {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Data/SomeLettersGamePack.json"));
        return JsonUtility.FromJson<JsonGamePack>(json);
    }
}
