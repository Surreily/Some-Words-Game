using UnityEngine;

public class TileBackgroundRenderer : MonoBehaviour {
    private GameObject tileBackground;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    [SerializeField]
    public Material material;

    public void Start() {
        tileBackground = new GameObject();
        tileBackground.transform.SetParent(transform, false);

        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[] {
            new Vector3(-0.45f, 0.45f, 10f),
            new Vector3(0.45f, 0.45f, 10f),
            new Vector3(0.45f, -0.45f, 10f),
            new Vector3(-0.45f, -0.45f, 10f),
        };

        mesh.triangles = new int[] {
            0, 1, 2,
            0, 2, 3,
        };

        mesh.uv = GetTextureCoordinatesForAnimationFrame(0);

        meshFilter = tileBackground.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer = tileBackground.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    public void SetAnimationFrame(int frame) {
        meshFilter.mesh.uv = GetTextureCoordinatesForAnimationFrame(frame);
    }

    private Vector2[] GetTextureCoordinatesForAnimationFrame(int frame) {
        float step = 0.25f;

        return new Vector2[] {
            new Vector2(step * frame, 1),
            new Vector2(step * (frame + 1), 1),
            new Vector2(step * (frame + 1), 0),
            new Vector2(step * frame, 0),
        };
    }
}
