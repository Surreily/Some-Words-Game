using UnityEngine;

public class TextureRenderer : MonoBehaviour {
    [SerializeField]
    public Material material;

    [SerializeField]
    public float width;

    [SerializeField]
    public float height;

    [SerializeField]
    public float z;

    public void Start() {
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[] {
            new Vector3(-width / 2f, height / 2f, z),
            new Vector3(width / 2f, height / 2f, z),
            new Vector3(width / 2f, -height / 2f, z),
            new Vector3(-width / 2f, -height / 2f, z),
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

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }
}
