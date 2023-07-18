using System.Collections.Generic;
using System.IO;
using System.Linq;
using Surreily.SomeWords.Scripts.Json.Game;
using Surreily.SomeWords.Scripts.Level;
using Surreily.SomeWords.Scripts.Map;
using Surreily.SomeWords.Scripts.Materials;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private MaterialStore materialStore;
    private HashSet<string> gameDictionary;
    private MapUi mapUi;
    private MapManager mapManager;
    private CameraMovement cameraMovement;

    private GameObject levelObject;

    [SerializeField]
    public Camera MainCamera;

    public CameraMovement CameraMovement => cameraMovement;

    public GameState State { get; private set; }

    public void Start() {
        SetUpMaterialStore();
        SetUpGameDictionary();

        State = GameState.Map;

        cameraMovement = MainCamera.GetComponent<CameraMovement>();

        JsonGame game = LoadFromJson();

        mapUi = gameObject.AddComponent<MapUi>();
        mapUi.MaterialStore = materialStore;

        mapUi.EnableUi();

        OpenMap(game);
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

    private void OpenMap(JsonGame game) {
        GameObject mapManagerObject = new GameObject("Map Manager");
        mapManagerObject.transform.parent = transform;
        
        mapManager = mapManagerObject.AddComponent<MapManager>();
        mapManager.GameManager = this;
        mapManager.MaterialStore = materialStore;
        mapManager.MapUi = mapUi;

        mapManager.LoadMap(game);

        State = GameState.Map;
    }

    private JsonGame LoadFromJson() {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Data/SomeLettersGamePack.json"));
        return JsonUtility.FromJson<JsonGame>(json);
    }

    public void OpenLevel(JsonLevel level) {
        levelObject = new GameObject();
        levelObject.transform.position = new Vector3(level.X - level.StartX, level.Y - level.StartY, 0f);

        LevelManager levelManager = levelObject.AddComponent<LevelManager>();
        levelManager.MaterialStore = materialStore;
        levelManager.GameDictionary = gameDictionary;
        levelManager.LoadBoard(level);

        State = GameState.Level;
    }

    public void CloseLevel() {
        Destroy(levelObject);
        levelObject = null;

        mapUi.EnableUi();

        State = GameState.Map;
    }
}
