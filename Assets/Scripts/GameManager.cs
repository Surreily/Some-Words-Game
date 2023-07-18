using System.Collections.Generic;
using System.IO;
using System.Linq;
using Surreily.SomeWords.Scripts.Json.Game;
using Surreily.SomeWords.Scripts.Level;
using Surreily.SomeWords.Scripts.Map;
using Surreily.SomeWords.Scripts.Materials;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private MaterialStore materialStore;
    private HashSet<string> gameDictionary;
    private MapManager mapManager;
    private LevelManager levelManager;
    private CameraMovement cameraMovement;

    private GameObject mapCanvasObject;

    private GameObject levelObject;

    [SerializeField]
    public Camera MainCamera;

    public CameraMovement CameraMovement => cameraMovement;

    public GameState State { get; private set; }

    public void Start() {
        State = GameState.Map;

        mapCanvasObject = GameObject.Find("Map Canvas");

        cameraMovement = MainCamera.GetComponent<CameraMovement>();

        JsonGame game = LoadFromJson();

        SetUpMaterialStore();
        SetUpGameDictionary();
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

        mapManager.LoadMap(game);

        mapCanvasObject.SetActive(true);

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

        mapCanvasObject.SetActive(false);

        State = GameState.Level;
    }

    public void CloseLevel() {
        Destroy(levelObject);

        levelObject = null;

        State = GameState.Map;
    }
}
