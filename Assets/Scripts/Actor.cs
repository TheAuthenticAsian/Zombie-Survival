using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class Actor : MonoBehaviourPunCallbacks
{
    public float speed = 5;
    public float maxHealth = 100;
    public UnityEvent deathEvent;
    public UnityAction tookDamage;

    protected float currentHealth;
    protected Rigidbody2D rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damage) => photonView.RPC("TakeDamage", RpcTarget.All, damage);

    [PunRPC]
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        tookDamage?.Invoke();
        if (currentHealth <= 0)
        {
            deathEvent?.Invoke();
        }
    }

    public void LookAt(Vector2 points)
    {
        Vector2 direction = points - (Vector2) transform.position;

        if (direction.magnitude < .5) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Move(Vector2 input)
    {
        rb.MovePosition(rb.position + input.normalized * speed * Time.fixedDeltaTime);
    }

}
