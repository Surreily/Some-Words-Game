using UnityEngine;

public class PlaySoundAction : IAction {
    private readonly AudioSource audioSource;
    private readonly AudioClip audioClip;

    public PlaySoundAction(AudioSource audioSource, AudioClip audioClip) {
        this.audioSource = audioSource;
        this.audioClip = audioClip;
    }

    public void Do() {
        audioSource.PlayOneShot(audioClip);
    }

    public void Undo() {
        // TODO: Play "undo" audio clip?
    }
}