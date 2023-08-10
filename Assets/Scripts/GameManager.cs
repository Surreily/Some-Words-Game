using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Surreily.SomeWords.Scripts.Json.Game;
using Surreily.SomeWords.Scripts.Level;
using Surreily.SomeWords.Scripts.Map;
using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Model.Game;
using UnityEngine;

namespace Surreily.SomeWords.Scripts {
    public class GameManager : MonoBehaviour, IGameManager {
        private MapUi mapUi;
        private LevelUi levelUi;
        private MapManager mapManager;
        private LevelManager levelManager;
        private CameraMovement cameraMovement;

        [SerializeField]
        public Camera MainCamera;

        public CameraMovement CameraMovement => cameraMovement;

        public MaterialStore MaterialStore { get; private set; }

        public HashSet<string> Dictionary { get; private set; }

        public GameState State { get; private set; }

        #region Start

        public void Start() {
            SetUpMaterialStore();
            SetUpGameDictionary();

            SetUpMapUi();
            SetUpLevelUi();

            SetUpMapManager();
            SetUpLevelManager();

            State = GameState.Map;

            cameraMovement = MainCamera.GetComponent<CameraMovement>();

            GameModel game = LoadFromJson();

            OpenMap(game);
        }

        private void SetUpMaterialStore() {
            GlobalTimer timer = gameObject.GetComponent<GlobalTimer>();

            MaterialStore = new MaterialStore(timer);
        }

        private void SetUpGameDictionary() {
            List<string> words = File.ReadLines(Path.Combine(Application.dataPath, "Data/Dictionary.txt"))
                .ToList();

            Dictionary = new HashSet<string>(words);
        }

        private void SetUpMapUi() {
            mapUi = gameObject.AddComponent<MapUi>();
            mapUi.MaterialStore = MaterialStore;
        }

        private void SetUpLevelUi() {
            levelUi = new LevelUi(MaterialStore);
        }

        private void SetUpMapManager() {
            mapManager = gameObject.AddComponent<MapManager>();
            mapManager.GameManager = this;

            mapManager.MapUi = mapUi;
        }

        private void SetUpLevelManager() {
            levelManager = gameObject.AddComponent<LevelManager>();
            levelManager.GameManager = this;
        }

        #endregion

        public void OpenMap(GameModel game) {
            mapManager.OpenMap(game);
            mapUi.EnableUi();

            State = GameState.Map;
        }

        public void CloseMap() {
            // TODO: Close map
            // TODO: Set game state to "select game"
            throw new NotImplementedException();
        }

        private GameModel LoadFromJson() {
            string json = File.ReadAllText(Path.Combine(Application.dataPath, "Data/SomeLettersGamePack.json"));
            JsonGame jsonGame = JsonUtility.FromJson<JsonGame>(json);

            return GameModelLoader.Load(jsonGame);
        }

        public void OpenLevel(LevelModel level) {
            mapUi.DisableUi();
            levelUi.EnableUi();

            levelManager.OpenLevel(level);

            State = GameState.Level;
        }

        public void CloseLevel() {
            levelManager.CloseLevel();
            levelUi.DisableUi();
            mapUi.EnableUi();

            State = GameState.Map;
        }
    }

}
