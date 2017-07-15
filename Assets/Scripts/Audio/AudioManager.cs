using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{    
    [SerializeField]
    private AudioClip[] clips;

    public void CheckAudio()
    {
        if (!GameInfo.Instance.AudioEnabled)
        {
            enabled = false;
            return;
        }
    }

    public void PlayAudio(string name)
    {
        return;

        if (!enabled)
            return;
        
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].name == name)
            {
                AudioSource source = GetComponent<AudioSource>();
                source.PlayOneShot(clips[i], 1);
                return;
            }
        }
        throw new System.Exception("There is no audio called " + name);
    }
}
