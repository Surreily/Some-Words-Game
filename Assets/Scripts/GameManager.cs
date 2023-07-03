using System.Collections.Generic;
using System.IO;
using System.Linq;
using Surreily.SomeWords.Scripts.Map;
using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Level;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private MaterialStore materialStore;
    private HashSet<string> gameDictionary;
    private MapManager mapManager;
    private LevelManager levelManager;
    private CameraMovement cameraMovement;

    [SerializeField]
    public Camera MainCamera;

    public CameraMovement CameraMovement => cameraMovement;

    public void Start() {
        cameraMovement = MainCamera.GetComponent<CameraMovement>();

        SetUpMaterialStore();
        SetUpGameDictionary();
        SetUpMapManager();
        ////SetUpLevelManager();

        JsonGamePack gamePack = LoadFromJson();

        mapManager.LoadMap(gamePack.Map);

        // TODO: Load levels when selected from the map.
        ////levelManager.LoadBoard(LoadFromJson().Levels.First());
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
        GameObject mapManagerObject = new GameObject("Map Manager");
        mapManagerObject.transform.parent = transform;
        
        mapManager = mapManagerObject.AddComponent<MapManager>();
        mapManager.GameManager = this;
        mapManager.MaterialStore = materialStore;
    }

    private void SetUpLevelManager() {
        GameObject levelManagerObject = new GameObject("Level Manager");
        levelManagerObject.transform.parent = transform;

        levelManager = gameObject.AddComponent<LevelManager>();
        levelManager.MaterialStore = materialStore;
        levelManager.GameDictionary = gameDictionary;
    }

    private JsonGamePack LoadFromJson() {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Data/SomeLettersGamePack.json"));
        return JsonUtility.FromJson<JsonGamePack>(json);
    }
}
