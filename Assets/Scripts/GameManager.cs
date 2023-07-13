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

    [SerializeField]
    public Camera MainCamera;

    public GameObject CanvasObject => canvasObject;

    public CameraMovement CameraMovement => cameraMovement;

    public void Start() {
        cameraMovement = MainCamera.GetComponent<CameraMovement>();

        SetUpCanvas();
        SetUpMaterialStore();
        SetUpGameDictionary();
        SetUpMapManager();
        ////SetUpLevelManager();

        JsonGame game = LoadFromJson();

        mapManager.LoadMap(game);

        // TODO: Load levels when selected from the map.
        ////levelManager.LoadBoard(LoadFromJson().Levels.First());
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

    private JsonGame LoadFromJson() {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Data/SomeLettersGamePack.json"));
        return JsonUtility.FromJson<JsonGame>(json);
    }
}
