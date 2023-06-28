using UnityEngine;

namespace Surreily.SomeWords.Scripts.Renderers {
    public class TileRenderer : MonoBehaviour {
        public Material Material { get; set; }

        public void Start() {
            Mesh mesh = new Mesh();

            mesh.vertices = new Vector3[] {
                new Vector3(-0.5f, 0.5f, 0f),
                new Vector3(0.5f, 0.5f, 0f),
                new Vector3(0.5f, -0.5f, 0f),
                new Vector3(-0.5f, -0.5f, 0f),
            };

            mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

            mesh.uv = new Vector2[] {
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f),
            };

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = Material;
        }
    }
}
