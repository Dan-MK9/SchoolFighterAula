using UnityEngine;

public class MusicController : MonoBehaviour
{
    // classe responsavel por controlar qualquer tipo de audio
    private AudioSource AudioSource;

    // AudioClip é o arquivo de audio que será executado
    public AudioClip levelMusic;


    void Start()
    {
        AudioSource = GetComponent<AudioSource>();

        // Ao iniciar o MusicController, inicia a musica da fase
        PlayMusic(levelMusic);
    }

    public void PlayMusic(AudioClip music)
    {
        // Define o som que irá ser reproduzido
        AudioSource.clip = music;

        // Reproduz o som
        AudioSource.Play();
    }
}
