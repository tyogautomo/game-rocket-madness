using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour {

    AudioSource audioSource;
    [SerializeField] AudioClip soundStartJingle;
    [SerializeField] AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic() {
        audioSource.clip = soundStartJingle;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.loop = true;
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

}
