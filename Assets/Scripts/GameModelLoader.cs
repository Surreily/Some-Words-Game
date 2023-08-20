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
                            State = GetPathState(jsonPath.State),
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
                    Id = jsonLevel.Id,
                    X = jsonLevel.X,
                    Y = jsonLevel.Y,
                    Title = jsonLevel.Title,
                    Description = jsonLevel.Description,
                    State = GetLevelState(jsonLevel.State),
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
            LevelGoalModel levelGoalModel = new LevelGoalModel {
                Type = GetLevelGoalType(jsonLevelGoal.Type),
                Word = jsonLevelGoal.Word,
            };

            return levelGoalModel;
        }

        private static List<DecorationModel> LoadDecorations(List<JsonDecoration> jsonDecorations) {
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

        private static PathState GetPathState(string value) {
            if (string.IsNullOrEmpty(value)) {
                return PathState.Closed;
            }

            if (Enum.TryParse(value, out PathState pathState)) {
                return pathState;
            }

            throw new InvalidOperationException(
                $"Unrecognised {nameof(PathState)}: \"{value}\".");
        }

        private static LevelState GetLevelState(string value) {
            if (string.IsNullOrEmpty(value)) {
                return LevelState.Closed;
            }

            if (Enum.TryParse(value, out LevelState levelState)) {
                return levelState;
            }

            throw new InvalidOperationException(
                $"Unrecognised {nameof(LevelState)}: \"{value}\".");
        }

        private static LevelGoalType GetLevelGoalType(string value) {
            if (Enum.TryParse(value, out LevelGoalType levelGoalType)) {
                return levelGoalType;
            }

            throw new InvalidOperationException(
                $"Unrecognised {nameof(LevelGoalType)}: \"{value}\".");
        }

        private static Direction GetDirection(string value) {
            if (Enum.TryParse(value, out Direction direction)) {
                return direction;
            }

            throw new InvalidOperationException(
                $"Unrecognised {nameof(Direction)}: \"{value}\".");
        }
    }
}
