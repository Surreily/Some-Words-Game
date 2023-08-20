using System;
using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Json.Game;
using Surreily.SomeWords.Scripts.Model.Game;

namespace Surreily.SomeWords.Scripts {
    public static class GameModelLoader {
        public static GameModel Load(JsonGame jsonGame) {
            GameModel gameModel = new GameModel {
                Name = jsonGame.Name,
                Description = jsonGame.Description,
                StartX = jsonGame.StartX,
                StartY = jsonGame.StartY,
            };

            gameModel.Paths = LoadPaths(jsonGame.Paths);
            gameModel.Levels = LoadLevels(jsonGame.Levels);
            gameModel.Decorations = LoadDecorations(jsonGame.Decorations);

            return gameModel;
        }

        private static List<PathModel> LoadPaths(List<JsonPath> jsonPaths) {
            List<PathModel> pathModels = new List<PathModel>();

            foreach (JsonPath jsonPath in jsonPaths) {
                for (int x = 0; x < jsonPath.Width; x++) {
                    for (int y = 0; y < jsonPath.Height; y++) {
                        PathModel pathModel = new PathModel {
                            X = jsonPath.X + x,
                            Y = jsonPath.Y + y,
                            Colour = jsonPath.Colour,
                        };

                        pathModels.Add(pathModel);
                    }
                }
            }

            return pathModels;
        }

        private static List<LevelModel> LoadLevels(List<JsonLevel> jsonLevels) {
            List<LevelModel> levelModels = new List<LevelModel>();

            foreach (JsonLevel jsonLevel in jsonLevels) {
                LevelModel levelModel = new LevelModel {
                    X = jsonLevel.X,
                    Y = jsonLevel.Y,
                    Id = jsonLevel.Id,
                    Title = jsonLevel.Title,
                    Description = jsonLevel.Description,
                    Colour = jsonLevel.Colour,
                    Width = jsonLevel.Width,
                    Height = jsonLevel.Height,
                    StartX = jsonLevel.StartX,
                    StartY = jsonLevel.StartY,
                };

                levelModel.Tiles = LoadLevelTiles(jsonLevel.Tiles);
                levelModel.Goal = LoadLevelGoal(jsonLevel.Goal);

                levelModels.Add(levelModel);
            }

            return levelModels;
        }

        private static List<LevelTileModel> LoadLevelTiles(List<JsonTile> jsonLevelTiles) {
            List<LevelTileModel> levelTileModels = new List<LevelTileModel>();

            foreach (JsonTile jsonLevelTile in jsonLevelTiles) {
                if (jsonLevelTile.Character.Length != 1) {
                    throw new InvalidOperationException(
                        "Character length must be exactly 1.");
                }

                levelTileModels.Add(new LevelTileModel {
                    X = jsonLevelTile.X,
                    Y = jsonLevelTile.Y,
                    Character = jsonLevelTile.Character[0],
                });
            }

            return levelTileModels;
        }

        private static LevelGoalModel LoadLevelGoal(JsonLevelGoal jsonLevelGoal) {
            if (!Enum.TryParse(jsonLevelGoal.Type, out LevelGoalType levelGoalType)) {
                throw new InvalidOperationException(
                    $"Unrecognised {nameof(LevelGoalType)}: \"{jsonLevelGoal.Type}\".");
            }

            LevelGoalModel levelGoalModel = new LevelGoalModel {
                Type = levelGoalType,
                Word = jsonLevelGoal.Word,
            };

            return levelGoalModel;
        }

        public static List<DecorationModel> LoadDecorations(List<JsonDecoration> jsonDecorations) {
            List<DecorationModel> decorationModels = new List<DecorationModel>();

            foreach (JsonDecoration jsonDecoration in jsonDecorations) {
                DecorationModel decorationModel = new DecorationModel {
                    X = jsonDecoration.X,
                    Y = jsonDecoration.Y,
                    Width = jsonDecoration.Width,
                    Height = jsonDecoration.Height,
                    Material = jsonDecoration.Material,
                };

                decorationModels.Add(decorationModel);
            }

            return decorationModels;
        }
    }
}
