using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMain : MonoBehaviour
{
    [Header ("Movement Stats")]
    [SerializeField] float speed = 4;
    [SerializeField] KeyCode jumpButton = KeyCode.W;
    [SerializeField] float jumpheight = 5;

    [Header("Bullet Stats")]
    [SerializeField] KeyCode shootButton = KeyCode.Space;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletCount = 5;
    int currentBulletCount;
    [SerializeField] Slider bulletBar;

    [Header("Health")]
    [SerializeField] float maxHealth = 3;
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

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        currentBulletCount = bulletCount;
        bulletBar.maxValue = bulletCount;
        bulletBar.value = currentBulletCount;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(jumpButton) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(shootButton) && currentBulletCount >0)
        {
            Shoot();
        }

        UpdateUI();
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
        b.GetComponent<Rigidbody2D>().velocity = Vector2.right *10;
        currentBulletCount--;
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

    public void InceaseBullet()
    {
        if (currentBulletCount < bulletCount)
        {
            currentBulletCount++;
        }
    }

    void UpdateUI()
    {
        healthBar.value = currentHealth;

        bulletBar.value = currentBulletCount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //death
        if (collision.gameObject.layer == 10)
        {
            KillPlayer();
        }
        //win
        if (collision.gameObject.layer == 11)
        {
            WinGame();
        }
        //upgrades
        if (collision.gameObject.layer == 12)
        {
            UpgradeSpeed(collision.gameObject.GetComponent<SpeedUpgrades>().GetUpgrade());
        }
    }
    
}
