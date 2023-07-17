using System.Collections.Generic;
using System.IO;
using System.Linq;
using Surreily.SomeWords.Scripts.Map;
using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Level;
using UnityEngine;
using UnityEngine.UI;
using Surreily.SomeWords.Scripts.Json.Game;

public class GameManager : MonoBehaviour {
    private GameObject canvasObject;
    private MaterialStore materialStore;
    private HashSet<string> gameDictionary;
    private MapManager mapManager;
    private LevelManager levelManager;
    private CameraMovement cameraMovement;

    private GameObject levelObject;

    [SerializeField]
    public Camera MainCamera;

    public GameObject CanvasObject => canvasObject;

    public CameraMovement CameraMovement => cameraMovement;

    public GameState State { get; private set; }

    public void Start() {
        State = GameState.Map;

        cameraMovement = MainCamera.GetComponent<CameraMovement>();

        JsonGame game = LoadFromJson();

        SetUpCanvas();
        SetUpMaterialStore();
        SetUpGameDictionary();
        OpenMap(game);
    }

    private void SetUpCanvas() {
        canvasObject = new GameObject();

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = MainCamera;

        canvasObject.AddComponent<CanvasScaler>();
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

        State = GameState.Map;
    }

    private JsonGame LoadFromJson() {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Data/SomeLettersGamePack.json"));
        return JsonUtility.FromJson<JsonGame>(json);
    }

    public void OpenLevel(JsonLevel level) {
        levelObject = new GameObject();
        levelObject.transform.position = new Vector3(level.X, level.Y, 0f);

        LevelManager levelManager = levelObject.AddComponent<LevelManager>();
        levelManager.MaterialStore = materialStore;
        levelManager.GameDictionary = gameDictionary;
        levelManager.LoadBoard(level);

        State = GameState.Level;
    }

    public void CloseLevel() {
        GameObject.Destroy(levelObject);

        levelObject = null;

        State = GameState.Map;
    }
}
