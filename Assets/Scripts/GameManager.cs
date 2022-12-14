using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private MaterialStore materialStore;
    private HashSet<string> gameDictionary;
    private LevelManager levelManager;

    [SerializeField]
    public Material backgroundMaterial;

    [SerializeField]
    public Material characterMaterial;

    [SerializeField]
    public AudioClip cursorAudioClip;

    [SerializeField]
    public AudioClip interactAudioClip;

    [SerializeField]
    public AudioClip invalidAudioClip;

    public void Start() {
        SetUpMaterialStore();
        SetUpGameDictionary();
        SetUpLevelManager();

        levelManager.LoadBoard(LoadFromJson().Levels.First());
    }

    private void SetUpMaterialStore() {
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
    }

    private void SetUpGameDictionary() {
        List<string> words = File.ReadLines(Path.Combine(Application.dataPath, "Data/Dictionary.txt"))
            .ToList();

        gameDictionary = new HashSet<string>(words);
    }

    private void SetUpLevelManager() {
        levelManager = gameObject.AddComponent<LevelManager>();
        levelManager.MaterialStore = materialStore;
        levelManager.GameDictionary = gameDictionary;
    }

    private JsonGamePack LoadFromJson() {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Data/SomeLettersGamePack.json"));
        return JsonUtility.FromJson<JsonGamePack>(json);
    }
}
