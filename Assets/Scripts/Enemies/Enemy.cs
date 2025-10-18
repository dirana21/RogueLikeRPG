using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 100;
    private int currentHealth;
    private Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        moveDirection = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Простой возврат
        if (transform.position.x > 3) moveDirection = Vector2.left;
        if (transform.position.x < -3) moveDirection = Vector2.right;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
