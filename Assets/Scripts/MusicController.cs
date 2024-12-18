using UnityEngine;

public class MusicController : MonoBehaviour
{
    // classe responsavel por controlar qualquer tipo de audio
    private AudioSource AudioSource;

    // AudioClip � o arquivo de audio que ser� executado
    public AudioClip levelMusic;


    void Start()
    {
        AudioSource = GetComponent<AudioSource>();

        // Ao iniciar o MusicController, inicia a musica da fase
        PlayMusic(levelMusic);
    }

    public void PlayMusic(AudioClip music)
    {
        // Define o som que ir� ser reproduzido
        AudioSource.clip = music;

        // Reproduz o som
        AudioSource.Play();
    }
}
