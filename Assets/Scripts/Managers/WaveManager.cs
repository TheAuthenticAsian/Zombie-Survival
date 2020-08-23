using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public Transform[] spawnPositions;
    public float timeBetweenZombiesSpawn = 2f;

    public int currentZombies;
    private void Awake()
    {
        instance = this;
    }

    public void StartWave(int waveNumber)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine("SpawnInZombies", waveNumber * 2);
    }

    public void ZombieAdded()
    {
        currentZombies++;
        GameManager.instance.UpdateZombieRemainingUI(currentZombies);
    }
    public void ZombieDied()
    {
        currentZombies--;
        GameManager.instance.UpdateZombieRemainingUI(currentZombies);
        if (currentZombies <= 0)
        {
            GameManager.instance.StartCoroutine("StartTimerForNextWave");
        }
    }

    IEnumerator SpawnInZombies(int amountOfZombies)
    {
        for (int i = 0; i < amountOfZombies; i++)
        {
            EnemyController zombie = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Zombie"), spawnPositions[Random.Range(0, spawnPositions.Length)].position, Quaternion.identity).GetComponent<EnemyController>();
            ZombieAdded();
            zombie.deathEvent.AddListener(ZombieDied);
            yield return new WaitForSeconds(timeBetweenZombiesSpawn);
        }
        
    }
}
