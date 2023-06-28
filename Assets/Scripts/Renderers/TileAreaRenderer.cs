using UnityEngine;

namespace Surreily.SomeWords.Scripts.Renderers {
    public class TileAreaRenderer : MonoBehaviour {
        public int Width { get; set; }
        public int Height { get; set; }
        public Material Material { get; set; }

        public void Start() {
            Mesh mesh = new Mesh();

            int quadCount = Width * Height;

            Vector3[] vertices = new Vector3[quadCount * 4];
            int[] triangles = new int[quadCount * 6];
            Vector2[] uv = new Vector2[quadCount * 4];

            int quadIndex = 0;

            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    vertices[quadIndex * 4] = new Vector3(x - 0.5f, y + 0.5f, 0f);
                    vertices[(quadIndex * 4) + 1] = new Vector3(x + 0.5f, y + 0.5f, 0f);
                    vertices[(quadIndex * 4) + 2] = new Vector3(x + 0.5f, y - 0.5f, 0f);
                    vertices[(quadIndex * 4) + 3] = new Vector3(x - 0.5f, y - 0.5f, 0f);

                    triangles[quadIndex * 6] = quadIndex * 4;
                    triangles[(quadIndex * 6) + 1] = (quadIndex * 4) + 1;
                    triangles[(quadIndex * 6) + 2] = (quadIndex * 4) + 2;
                    triangles[(quadIndex * 6) + 3] = quadIndex * 4;
                    triangles[(quadIndex * 6) + 4] = (quadIndex * 4) + 2;
                    triangles[(quadIndex * 6) + 5] = (quadIndex * 4) + 3;

                    uv[quadIndex * 4] = new Vector2(0f, 1f);
                    uv[(quadIndex * 4) + 1] = new Vector2(1f, 1f);
                    uv[(quadIndex * 4) + 2] = new Vector2(1f, 0f);
                    uv[(quadIndex * 4) + 3] = new Vector2(0f, 0f);

                    quadIndex++;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = Material;
        }
    }
}
