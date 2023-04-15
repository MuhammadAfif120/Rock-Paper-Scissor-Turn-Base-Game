using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    // Deklarasi string name ke dalam unity editor
    // penambahan kata "new" untuk membedakan antara "name" player dan "name" yang ada di monobehavior
    [SerializeField] new string name;

    [SerializeField] CharacterType type;
    [SerializeField] int currentHP;
    [SerializeField] int maxHP;
    [SerializeField] int attackPower;
    [SerializeField] int evenAttPower;
    [SerializeField] TMP_Text overHeadText;
    [SerializeField] Image avatar;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text hpText;
    [SerializeField] Button button;

    // Untuk menyimpan nilai awal posisi dari karakter
    // agar dapat dijalankan ketika ingin menuju enum returning
    private Vector3 initialPosition;

    public Button Button { get => button; }
    public CharacterType Type { get => type;}
    public int AttackPower { get => attackPower; }
    public int CurrentHP { get => currentHP;}
    public Vector3 InitialPosition { get => initialPosition;}
    public int MaxHP { get => maxHP;}
    public int EvenAttPower { get => evenAttPower; }

    private void Start() {
        initialPosition = this.transform.position;
        overHeadText.text = name;
        nameText.text = name;
        typeText.text = type.ToString(); // ditambahkan ".ToString()" untuk mengambil isi dari enum CharacterType
        UpdateHpUI();
        button.interactable = false; // setting button awal disable, dapat diaktifkan di battle manager

    }


    public void ChangeHP (int amount)
    {
        currentHP += amount;

        //  Untuk membatasi nilai HP max dan min
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHpUI();
    }


    private void UpdateHpUI()
    {
        healthBar.fillAmount = (float) currentHP / (float) maxHP; // setting bar HP warna hijau, ditambah "(float)" karena nilai default int
        hpText.text = (float) currentHP + "/" + maxHP;
    }
}
