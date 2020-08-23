using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager instance;
    //UI's
    [Header("UI's")]
    public TMP_Text ammoText;
    public TMP_Text waveText;
    public TMP_Text zombieRemainingText;
    public Slider health;
    // Strings
    private string currentWaveText = "Wave: ";
    private string newWaveText = "New Wave Starting: ";

    private void Awake()
    {
        instance = this;
    }

    [PunRPC]
    public void ManageCurrentWaveText(int currentWave) => waveText.text = $"{currentWaveText}{currentWave}";

    [PunRPC]
    public void ManageTimerWaveText(int currentTime) => waveText.text = $"{newWaveText}{currentTime}";

    [PunRPC]
    public void ManageZombieRemaningText(int zombieRemaning) => zombieRemainingText.text = $"Zombies Remaning: {zombieRemaning}";

    public void ManageAmmoUI(int currentAmmo) => ammoText.text = $"{currentAmmo}/6";

    public void ManageHealthUI(float currentHealth)
    {
        health.value = currentHealth;
    }

}
