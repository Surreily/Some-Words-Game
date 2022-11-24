using System.Collections.Generic;
using UnityEngine;

public class TileCharacterRenderer : MonoBehaviour {
    private GameObject tileCharacter;

    private MeshRenderer meshRenderer;

    public MaterialStore MaterialStore { get; set; }

    public char Character { get; set; }

    public void Start() {
        tileCharacter = new GameObject();
        tileCharacter.transform.position = new Vector3(1f / 16f * 4.5f, 1f / 16f * 1.5f, 0f);
        tileCharacter.transform.SetParent(transform, false);

        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[] {
            new Vector3(-0.5f, 0.5f, 0f),
            new Vector3(0.5f, 0.5f, 0f),
            new Vector3(0.5f, -0.5f, 0f),
            new Vector3(-0.5f, -0.5f, 0f),
        };

        mesh.triangles = new int[] {
            0, 1, 2,
            0, 2, 3,
        };

        mesh.uv = new Vector2[] {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        MeshFilter meshFilter = tileCharacter.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer = tileCharacter.AddComponent<MeshRenderer>();
    }

    public void SetCharacter(char character) {
        meshRenderer.material = MaterialStore.GetRainbowFontMaterial(character);
    }

    #region Static

    private static readonly string supportedCharacters = "abcdefghijklmnopqrstuvwxyz";

    private static Dictionary<char, Vector2[]> characterUvs;

    [RuntimeInitializeOnLoadMethod]
    private static void SetUpCharacterUvs() {
        characterUvs = new Dictionary<char, Vector2[]>();

        float unit = 1f / 32f;

        for (int i = 0; i < supportedCharacters.Length; i++) {
            characterUvs.Add(
                supportedCharacters[i],
                new Vector2[] {
                    new Vector2(i * unit, 1f),
                    new Vector2((i + 1) * unit, 1f),
                    new Vector2((i + 1) * unit, 0f),
                    new Vector2(i * unit, 0f),
            });
        }
    }

    #endregion

}
