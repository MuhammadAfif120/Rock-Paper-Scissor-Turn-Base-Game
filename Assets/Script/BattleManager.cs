using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    // Mendeklarasi nilai enum agar dapat dipanggil menggunakan "state"
    [SerializeField] State state;

    [SerializeField] GameObject battleResult;
    [SerializeField] TMP_Text battleResultText;

    // Deklarasi agar BattleManager dapat mengakses script Player
    [SerializeField] Player player1;
    [SerializeField] Player enemyBot;
    
    

    // Membuat tipe data khusus
    // Dengan nilai default string
    // Tidak bisa digunakan untuk aritmatika
    // Hanya bisa digunakan untuk perbandingan (Equality)
    enum State
    {
        Preparation,
        Player1Select,
        Player2Select,
        Attacking,
        Damaging,
        Returning,
        BattleIsOver
    }
    // Update is called once per frame
    void Update()
    {
        // Switch case untuk program yang dijalankan
        switch (state)
        {
            case State.Preparation:            
                // Set setiap player untuk bersiap-siap
                player1.Prepare();
                enemyBot.Prepare();

                // set giliran player yang akan menyerang
                // Default awal player utama akan bergerak terlebih dahulu
                player1.SetPlay(true);
                enemyBot.SetPlay(false);

                // Akan berpindah ke state berikutnya jika seluruh pemain sudah set charakter
                state = State.Player1Select;
                break;

            case State.Player1Select:
                if (player1.SelectedCharacter != null)
                {
                    player1.SetPlay(false);
                    enemyBot.SetPlay(true);
                    state = State.Player2Select;
                }
                break;

            case State.Player2Select:
                if (enemyBot.SelectedCharacter != null)
                {
                    enemyBot.SetPlay(false);
                    player1.Attack();
                    enemyBot.Attack();
                    state = State.Attacking;
                    
                }
                break;

            case State.Attacking:
                if (player1.isAttacking() == false && enemyBot.isAttacking() == false)
                {
                    /*  Menghitung yang terkena damage
                        Menggunakan Tuple untuk mendapatkan hasil return berganda
                        Bisa memakai "(Player winner, Player loser) = CalculateBattle(player1, enemyBot)"
                        atau "CalculateBattle(player1, enemyBot, out Player winner, out Player loser)"
                    */
                    CalculateBattle(player1, enemyBot, out Player winner, out Player loser);
                    
                    if (loser == null)
                    {
                        player1.TakeDamage(enemyBot.SelectedCharacter.AttackPower);
                        enemyBot.TakeDamage(player1.SelectedCharacter.AttackPower);
                    }

                    else
                    {
                        /*
                            Loser menerima damage sebesar attack power dari winner
                            dijak dolan
                        */
                        loser.TakeDamage(winner.SelectedCharacter.AttackPower);
                    }
                    state = State.Damaging;
                }
                break;

            case State.Damaging:
                // Cek ketika keuda player sudah menyerang
                if (player1.ISDamaging() == false && enemyBot.ISDamaging() == false)
                {
                    //  Hapus karakter jika ada yang mati
                    //  Remove dibawah ini dijalankan ketika proses damaging selesai
                    if (player1.SelectedCharacter.CurrentHP == 0)
                    {
                        player1.Remove(player1.SelectedCharacter);
                    }

                    if (enemyBot.SelectedCharacter.CurrentHP == 0)
                    {
                        enemyBot.Remove(enemyBot.SelectedCharacter);
                    }


                    //  animasi return
                    if (player1.SelectedCharacter != null)
                    {
                       player1.Return(); 
                    }
                    if (enemyBot.SelectedCharacter != null)
                    {
                        enemyBot.Return();
                    }

                    state = State.Returning;
                }
                break;

            case State.Returning:
                if (player1.IsReturning() == false && enemyBot.IsReturning() == false)
                {
                    if (player1.CharacterList.Count == 0 && enemyBot.CharacterList.Count == 0)
                        {
                            battleResult.SetActive(true);
                            battleResultText.text = "Battle is Over!!!! \n Draw!";
                            state = State.BattleIsOver;
                        }

                    else if (player1.CharacterList.Count == 0)
                        {
                            battleResult.SetActive(true);
                            battleResultText.text = "Battle is Over!!!! \n Enemy Bot Win!";
                            state = State.BattleIsOver;
                        }

                    else if (enemyBot.CharacterList.Count == 0)
                        {
                            battleResult.SetActive(true);
                            battleResultText.text = "Battle is Over!!!! \n You Win!"; 
                            state = State.BattleIsOver;
                        }

                    else
                        {
                            state = State.Preparation;
                        }
                }
                break;

            case State.BattleIsOver:
                
                

                break;
        }
    }

    private void CalculateBattle(Player player1, Player enemyBot, out Player winner, out Player loser)
    {
        //  Membuat variabel temporay untuk menyimpan tipe dari karakter yang dipilih
        var type1 = player1.SelectedCharacter.Type;
        var type2 = enemyBot.SelectedCharacter.Type;

        //  Perulangan untuk menentukan siapa yang menang dan yang kalah
        if (type1 == CharacterType.Rock && type2 == CharacterType.Paper)
        {
            winner = enemyBot;
            loser = player1;
        }

        else if (type1 == CharacterType.Rock && type2 == CharacterType.Scissor)
        {
            winner = player1;
            loser = enemyBot;
        }

        else if (type1 == CharacterType.Paper && type2 == CharacterType.Rock)
        {
            winner = player1;
            loser = enemyBot;
        }

        else if (type1 == CharacterType.Paper && type2 == CharacterType.Scissor)
        {
            winner = enemyBot;
            loser = player1;
        }

        else if (type1 == CharacterType.Scissor && type2 == CharacterType.Rock)
        {
            winner = enemyBot;
            loser = player1;
        }

        else if (type1 == CharacterType.Scissor && type2 == CharacterType.Paper)
        {
            winner = player1;
            loser = enemyBot;
        }

        else
        {
            winner = null;
            loser = null;
        }
    }


    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
