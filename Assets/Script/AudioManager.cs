using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    //  Membuat static instance, yang artinya setiap gameObject yang jenisnya
    //  AudioManager akan punya variabel yang sama
    //  Agar tidak terjadi duplikat audio manager ketika perpindahan scene
    static AudioSource bgmInstance;
    static AudioSource sfxInstance;
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;

    public bool IsMute{ get => bgm.mute; }
    public float BgmVolume { get => bgm.volume; }
    public float SfxVolume { get => sfx.volume; }



    private void Start() {
        //  Check instance
        //  Jika ada destroy yang baru. Seperti LIFO
        if (bgmInstance != null)
        {
            Destroy(this.bgm.gameObject);

             //  Mengoper sfxInstance dan bgmInstane menjadi sfx dan bgm, agar tidak di destroy
            bgm = bgmInstance;
        }
        else
        {
            //  Jika bgmInstance null maka ambil bgm yang ada di game object
            bgmInstance = bgm;

            //  Membuat bgm dan sfx tidak memiliki Parent game object
            bgm.transform.SetParent(null);

            //  Membuat BGM lama agar tidak dihancurkan ketika berganti scene
            DontDestroyOnLoad(bgm.gameObject);

        }

        //  Check instance
        //  Jika ada destroy yang baru. Seperti LIFO
        if (sfxInstance != null)
        {
            Destroy(this.sfx.gameObject);

            //  Mengoper sfxInstance dan bgmInstane menjadi sfx dan bgm, agar tidak di destroy
            sfx = sfxInstance;
        }

        else
        {
            //  Jika sfxInstance null maka ambil sfx yang ada di game object
            sfxInstance = sfx;

            //  Membuat bgm dan sfx tidak memiliki Parent game object
            sfx.transform.SetParent(null);

            //  Membuat BGM lama agar tidak dihancurkan ketika berganti scene
            DontDestroyOnLoad(sfx.gameObject);
        }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        /*  Pengkondisian ini hanya berlaku ketika 2 karakter yang sama terkena damage
            Ketika karakter 1 terkena damage dan meminta suara ke AudioManager
            AudioManager akan mengecek apabila terdapat 2 karakter yang sama yang terkena damage
            jika benar, maka permintaan pertama akan distop, dan diperbarui permintaan
            suara oleh karakter yang sama yang lainnya.
        */

        if (bgm.isPlaying)
        {
            bgm.Stop();
        }

        bgm.clip = clip;
        bgm.loop = loop;
        bgm.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        /*  Pengkondisian ini hanya berlaku ketika 2 karakter yang sama terkena damage
            Ketika karakter 1 terkena damage dan meminta suara ke AudioManager
            AudioManager akan mengecek apabila terdapat 2 karakter yang sama yang terkena damage
            jika benar, maka permintaan pertama akan distop, dan diperbarui permintaan
            suara oleh karakter yang sama yang lainnya.
        */
        
        if (sfx.isPlaying)
        {
            sfx.Stop();
        }

        sfx.clip = clip; 
        sfx.Play();
    }

    public void setMute(bool value)
    {
        bgm.mute = value;
        sfx.mute = value;
    }

    //  Fungsi untuk set playerpref BGM di options settings
    public void setBgmVolume(float value)
    {
        bgm.volume = value;

    }

    //  Fungsi untuk set playerpref Sfx di options settings
    public void setSfxVolume(float value)
    {
        sfx.volume = value;

    }
}
