using System;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    private int frame;
    private float nextFrameTime;

    [SerializeField]
    public float frameDuration;

    public event EventHandler<int> FrameChanged;

    public int Frame => frame;

    // Start is called before the first frame update
    void Start()
    {
        nextFrameTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFrameTime) {
            FrameChanged?.Invoke(this, frame);

            frame = (frame + 1) % 4;
            nextFrameTime += frameDuration;
        }
    }
}
