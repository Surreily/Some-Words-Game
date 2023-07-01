using Surreily.SomeWords.Scripts.Materials;
using UnityEngine;

public class AnimatedTextureRenderer : MonoBehaviour
{
    private GameObject animatedTextureObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private GlobalTimer globalTimer;

    [SerializeField]
    public Material material;

    [SerializeField]
    public float width;

    [SerializeField]
    public float height;

    [SerializeField]
    public float z;

    public void Start() {
        globalTimer = GameObject.FindGameObjectWithTag("GameController").GetComponent<GlobalTimer>();
        globalTimer.FrameChanged += OnFrameChanged;

        animatedTextureObject = new GameObject();
        animatedTextureObject.transform.SetParent(transform, false);

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

        mesh.uv = GetTextureCoordinatesForFrame(globalTimer.Frame);

        meshFilter = animatedTextureObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer = animatedTextureObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    private void OnFrameChanged(object sender, int e) {
        SetFrame(e);
    }

    public void SetFrame(int frame) {
        meshFilter.mesh.uv = GetTextureCoordinatesForFrame(frame);
    }

    private Vector2[] GetTextureCoordinatesForFrame(int frame) {
        float step = 0.25f;

        return new Vector2[] {
            new Vector2(step * frame, 1),
            new Vector2(step * (frame + 1), 1),
            new Vector2(step * (frame + 1), 0),
            new Vector2(step * frame, 0),
        };
    }

    public void OnDestroy() {
        globalTimer.FrameChanged -= OnFrameChanged;
    }
}
