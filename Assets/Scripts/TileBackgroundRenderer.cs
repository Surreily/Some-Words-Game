using UnityEngine;

public class TileBackgroundRenderer : MonoBehaviour {
    private GameObject tileBackground;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Material material;

    public Vector3 Position { get; set; } = Vector3.zero;
    public float Width { get; set; } = 1f;
    public float Height { get; set; } = 1f;
    
    public Material Material {
        get { return material; }
        set {
            if (material != value) {
                material = value;

                if (meshRenderer != null) {
                    meshRenderer.material = value;
                }
            }
        }
    }

    public void Start() {
        tileBackground = new GameObject();
        tileBackground.transform.position = Position;
        tileBackground.transform.SetParent(transform, false);

        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[] {
            new Vector3(-Width / 2f, Height / 2f, 0f),
            new Vector3(Width / 2f, Height / 2f, 0f),
            new Vector3(Width / 2f, -Height / 2f, 0f),
            new Vector3(-Width / 2f, -Height / 2f, 0f),
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

        meshFilter = tileBackground.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer = tileBackground.AddComponent<MeshRenderer>();
        meshRenderer.material = Material;
    }
}
