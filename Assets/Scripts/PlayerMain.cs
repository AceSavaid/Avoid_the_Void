using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] KeyCode jumpButton = KeyCode.W;
    [SerializeField] float jumpheight = 5;

    [SerializeField] KeyCode shootButton = KeyCode.Space;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletCount = 3;
    int currentBulletCount;
    [SerializeField] Slider bulletBar;

    [SerializeField] float maxHealth = 5;
    float currentHealth;
    [SerializeField] Slider healthBar;

    Rigidbody2D rb;
    GameEnd gameEnd;

    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameEnd = FindObjectOfType<GameEnd>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(jumpButton))
        {

        }

        if (Input.GetKeyDown(shootButton))
        {
            Shoot();
        }
    }

    void Move()
    {
        float movedir = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movedir * speed, rb.velocity.y);

    }
    void Jump()
    {
        rb.velocity = new Vector2(0, jumpheight);
    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet, this.transform);
        b.GetComponent<Rigidbody2D>().velocity = Vector2.right;
    }

    public void HurtPlayer(float value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    public void HealPlayer(float value)
    {
        currentHealth += value;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void WinGame()
    {
        gameEnd.EndGame(true);
    }

    public void KillPlayer()
    {
        gameEnd.EndGame(false);
    }

    public void UpgradeSpeed(float value)
    {
        speed += value;
    }

    public void DownGradeSpeed(float value)
    {
        speed -= value;

    }
}
