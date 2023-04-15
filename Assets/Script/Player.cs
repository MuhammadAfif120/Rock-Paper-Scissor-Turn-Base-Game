using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] Character selectedCharacter;

    // Menyimpan List dari karakter
    [SerializeField] List<Character> characterList;

    // Mengambil posisi dari masing masing attakPosition player
    [SerializeField] Transform attPosition;
    [SerializeField] bool isBot;
    [SerializeField] UnityEvent onTakeDamage;

    public Character SelectedCharacter { get => selectedCharacter; }

    public List<Character> CharacterList { get => characterList;}

    private void Start() 
    {
        if (isBot)
        {
            foreach (var karakter in characterList)
            {
                karakter.Button.interactable = false;
            }    
        }
    }


    // Fungsi untuk set pilih karakter kembali ke null ketika program diulang
    // Terdapat di Battle Manager (dibalik layar)
    public void Prepare()
    {
        selectedCharacter = null;
    }

    // Fungsi untuk player dapat memilih karakter
    // Terdapat di Button
    public void SelectCharacter(Character character)
    {
        selectedCharacter = character; 
    }


    // Fungsi untuk setiap player melakukan set karater yang akan dimainkan
    public void SetPlay(bool value)
    {

        if (isBot)
        {

            List<Character> chanceBotCharacter = new List<Character>();
            foreach (var character in characterList)
            {
                /* 
                    pembuatan persentase bot memilih karakter yang darah masih banyak
                    dengan menggunakan float agar tidak menjai pembulatan kebawah ketika koma
                */
                int chance = Mathf.CeilToInt(((float) character.CurrentHP / (float) character.MaxHP) * 10);
                for (int i = 0; i < chance; i++)
                {
                    chanceBotCharacter.Add(character);    
                }
            }

            int index = Random.Range(0, chanceBotCharacter.Count);
            selectedCharacter = chanceBotCharacter[index]; 
        }

        else
        {
            //  Kode agar player dapat memilih karakter mana yang ingin dimainkan
            foreach (var character in characterList)
            {
                character.Button.interactable = value;
            }
        }

    }


    // Fungsi yang dijalankan ketika enum Attacking dijalankan
    /* Menggunakan plugins bernama DOTween untuk membuat pergerakan
       dari posisi idle ke posisi attack position 
       ada fungsi tambahan setEase untuk membuat pergerakan menjadi smooth
       dan juga terdapat penambahan animasi menggunakan setEase*/
    public void Attack()
    {
        selectedCharacter.transform
            .DOMove(attPosition.position, 1f);
    }       

    
    /* Fungsi yang digunakan untuk mengecek apakah karakter yang dipilih
            player sudah selesai menyerang.
       Menggunakan cara mengembalikan nilai DOTween yang berasal dari
            object ynag sedang di Tweening, dalam kasus ini adalah "selectedCharacter.transform".
       Terdapat tambahan code cek boolean pada fungsi .isTweening
            yang bersifat opsional untuk mengecek apakah object sedang
            di tweening, bool tersebut berfungsi pada ketika game dipause
            saat karakter sedang menyerang
    */
    
    public bool isAttacking()
    {
        if (selectedCharacter == null)
        {
            return false;
        }

        return DOTween.IsTweening(selectedCharacter.transform);
    }

    
    //  Fungsi untuk mengurangi jumlah HP yang terkena damage
    public void TakeDamage(int damageValue)
    {
        /*  
            Menggunakan nilai "negatif" karena HP yang akan dirubah
            berkurang dari HP saat ini 
        */
        selectedCharacter.ChangeHP(-damageValue);

        /* 
            Mengubah warna karakter yang terkena damage
            ditambahkan fungsi setLoops untuk perulangan pergantian warna
        */
        var SR = selectedCharacter.GetComponent<SpriteRenderer>();
        SR.DOColor(Color.red, 0.1f) .SetLoops(6, LoopType.Yoyo);

        //  Menampilkan suara ketika karakter terkena damage
        onTakeDamage.Invoke();
    }


    public bool ISDamaging()
    {
        if (selectedCharacter == null)
        {
            return false;
        }
        
        var SR = selectedCharacter.GetComponent<SpriteRenderer>();
        return DOTween.IsTweening(SR);
    }

    public void Remove(Character chara)
    {
        // Jika karakter tidak ada, maka skip lanjut ke enum state berikutnya
        if (characterList.Contains(chara) == false)
        {
            return;
        }

        //  Jika karakter yang ingin di jalankan di state return mati
        //  Maka karakter tidak dapat di return
        if (selectedCharacter == chara)
        {
            selectedCharacter = null;
        }

        // Jika karakter ada, lakukan dibawah ini
        chara.Button.interactable = false;
        chara.gameObject.SetActive(false);

        characterList.Remove(chara);
    }


    //  Fungsi untuk membuat karakter yang sudah menyerang kembali ke posisi awal
    internal void Return()
    {
        selectedCharacter.transform
            .DOMove(selectedCharacter.InitialPosition, 1f);
    }

    // Mengecek isReturn sudah selesai atau belum
    public bool IsReturning()
    {
        //  Jika karakter yang ingin di returning sudah mati
        //  akan menjalankan pengkondisian dibawah ini
        if (selectedCharacter == null)
        {
            return false;
        }

        //  Jika karakter belum mati, maka lakukan baris kode dibawah ini
        //  untuk mengembalikan karakter ke posisi awal
        return DOTween.IsTweening(selectedCharacter.transform);
    }
}
