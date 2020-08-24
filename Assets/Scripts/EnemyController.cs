using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Pathfinding;

public class EnemyController : Actor
{
    int followingPlayerID;
    Vector2 playerLocation;

    AIDestinationSetter destinationSetter;
    AIPath aiPath;

    new private void Awake()
    {
        base.Awake();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
    }

    new private void Start()
    {
        base.Start();
        deathEvent.AddListener(Death);
        aiPath.maxSpeed = speed;
    }

    private void Update()
    {
        if (GameManager.instance.availablePlayers.Count == 0) return;
        if (followingPlayerID == 0) followingPlayerID = GameManager.instance.availablePlayers[Random.Range(0, GameManager.instance.availablePlayers.Count)];
        foreach(int player in GameManager.instance.availablePlayers)
        {
            if((PhotonView.Find(player).gameObject.transform.position - transform.position).magnitude <= 3f)
            {
                followingPlayerID = player;
            }
        }
        playerLocation = PhotonView.Find(followingPlayerID).gameObject.transform.position;
        destinationSetter.target = PhotonView.Find(followingPlayerID).gameObject.transform;
        LookAt(playerLocation);
    }
    private void Death() => Destroy(gameObject);
}
