using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    private AudioSource source;
    
    [SerializeField]
    private AudioClip[] clips;

	void Start ()
    {
        source = GetComponent<AudioSource>();
	}
	
	void Update ()
    {

	}

    public void PlayAudio(string name)
    {
        if (!enabled)
            return;

        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].name == name)
            {
                source.PlayOneShot(clips[i], 1);
                return;
            }
        }
        throw new System.Exception("There is no audio called " + name);
    }
}
