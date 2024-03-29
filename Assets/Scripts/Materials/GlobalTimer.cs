using System;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class GlobalTimer : MonoBehaviour {
        private int frame;
        private float nextFrameTime;

        [SerializeField]
        public float frameDuration;

        public event EventHandler<int> FrameChanged;

        public int Frame => frame;

        void Start() {
            nextFrameTime = 0f;
        }

        void Update() {
            if (Time.time > nextFrameTime) {
                FrameChanged?.Invoke(this, frame);

                frame = (frame + 1) % 16;
                nextFrameTime += frameDuration;
            }
        }
    }
}