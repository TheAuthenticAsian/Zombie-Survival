using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerController : Actor
{
    private CinemachineVirtualCamera cameraVC;
    private Animator animator;

    private Vector2 input;
    public bool isDead;

    public Gun pistol;
    public Transform gunPos;

    new private void Awake()
    {
        base.Awake();

        cameraVC = FindObjectOfType<CinemachineVirtualCamera>();
        animator = GetComponent<Animator>();
        pistol = GetComponent<Gun>();

        GameManager.instance.photonView.RPC("AddPlayerToList", RpcTarget.All, photonView.ViewID);
    }


    new private void Start()
    {
        if (!photonView.IsMine) return;
        base.Start();
        SetFollow(transform);
        deathEvent.AddListener(OnDeath);
        tookDamage += TookDamage;
    }

    private void TookDamage()
    {
        UIManager.instance.ManageHealthUI(currentHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }


    private void OnDeath()
    {
        animator.SetBool("IsDead", true);
        isDead = true;
        GameManager.instance.photonView.RPC("PlayerDied",RpcTarget.All,photonView.ViewID);
        if (GameManager.instance.alivePlayers.Count == 0)
        {
            Application.Quit();
            return;
        }
        SetFollow(PhotonView.Find(GameManager.instance.alivePlayers[Random.Range(0, GameManager.instance.alivePlayers.Count)]).gameObject.transform);
    }

    private void Update()
    {
        if (!photonView.IsMine || isDead) return;

        LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(pistol.Reload(() => animator.SetBool("Reloading", false)));
            animator.SetBool("Reloading", true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            pistol.Shoot((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2) transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine || isDead) return;
        Move(input);
    }

    public void SetFollow(Transform player) => cameraVC.Follow = player;
}
