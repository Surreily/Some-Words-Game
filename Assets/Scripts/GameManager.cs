using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private MaterialStore materialStore;
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
        SetUpMaterialStore();
        SetUpBoardManager();

        boardManager.LoadBoard(LoadFromJson().Levels.First());
    }

    private void SetUpMaterialStore() {
        LoadTextureUtility loadTextureUtility = new LoadTextureUtility();

        Texture2D border1 = LoadTextureUtility.LoadTexture(
            Path.Combine(Application.dataPath, "Textures/Border 1.png"));
        Texture2D border2 = LoadTextureUtility.LoadTexture(
            Path.Combine(Application.dataPath, "Textures/Border 2.png"));
        Texture2D border3 = LoadTextureUtility.LoadTexture(
            Path.Combine(Application.dataPath, "Textures/Border 3.png"));
        Texture2D border4 = LoadTextureUtility.LoadTexture(
            Path.Combine(Application.dataPath, "Textures/Border 4.png"));

        GlobalTimer timer = gameObject.GetComponent<GlobalTimer>();

        materialStore = new MaterialStore(timer);

        materialStore.Register("Border", border1, border2, border3, border4);
        materialStore.Register("Cursor", Resources.LoadAll<Sprite>("Sprites/Cursor"));
    }

    private void SetUpBoardManager() {
        boardManager = gameObject.AddComponent<BoardManager>();
        boardManager.MaterialStore = materialStore;
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
