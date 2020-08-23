using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks    
{
    public static GameManager instance;
    public int currentWave = 0;

    public List<int> availablePlayers = new List<int>();

    [PunRPC]
    public void AddPlayerToList(int player)
    {
        availablePlayers.Add(player);
    }

    private void Awake()
    {
        instance = this;
       
    }
    private void Start()
    {
        UIManager.instance.photonView.RPC("ManageCurrentWaveText", RpcTarget.All, currentWave);
        StartCoroutine("StartTimerForNextWave");
    }
    public IEnumerator StartTimerForNextWave()
    {
        int currentTime = 10;

        while (currentTime != 0)
        {
            UIManager.instance.photonView.RPC("ManageTimerWaveText", RpcTarget.All, currentTime);
            currentTime--;
            yield return new WaitForSeconds(1);
        }
        GameManager.instance.photonView.RPC("StartNextWave", RpcTarget.All);
    }

    public void StartNextWave()
    {
        currentWave++;
        print(currentWave);
        UIManager.instance.photonView.RPC("ManageCurrentWaveText", RpcTarget.All, currentWave);
        WaveManager.instance.StartWave(currentWave);
    }

    // UI
    public void UpdateZombieRemainingUI(int currentZombies)
    {
        UIManager.instance.photonView.RPC("ManageZombieRemaningText", RpcTarget.All, currentZombies);
    }

}
