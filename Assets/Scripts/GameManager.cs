using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private MaterialStore materialStore;
    private HashSet<string> gameDictionary;
    private MapManager mapManager;
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
        SetUpMapManager();
        SetUpLevelManager();

        JsonGamePack gamePack = LoadFromJson();

        mapManager.LoadMap(gamePack.Map);
        levelManager.LoadBoard(LoadFromJson().Levels.First());
    }

    private void SetUpMaterialStore() {
        GlobalTimer timer = gameObject.GetComponent<GlobalTimer>();

        materialStore = new MaterialStore(timer);
    }

    private void SetUpGameDictionary() {
        List<string> words = File.ReadLines(Path.Combine(Application.dataPath, "Data/Dictionary.txt"))
            .ToList();

        gameDictionary = new HashSet<string>(words);
    }

    private void SetUpMapManager() {
        mapManager = gameObject.AddComponent<MapManager>();
        mapManager.MaterialStore = materialStore;
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
