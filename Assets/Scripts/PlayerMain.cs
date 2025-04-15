using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] float speed = 4;
    [SerializeField] float jumpHeight = 5;

    [Header("Bullet Stats")]
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletCount = 5;
    int currentBulletCount;
    [SerializeField] Slider bulletBar;
    [SerializeField] TMP_Text bulletCountText;

    [Header("Health")]
    [SerializeField] float maxHealth = 3;
    float currentHealth;
    [SerializeField] Slider healthBar;

    [Header("Sound Effects")]
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip jumpSound;

    Rigidbody2D rb;
    GameEnd gameEnd;
    bool isGrounded;

    PlayerControls controls;
    Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => { if (isGrounded) Jump(); };
        controls.Player.Shoot.performed += ctx => { if (currentBulletCount > 0) Shoot(); };
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Start()
    {
        gameEnd = FindObjectOfType<GameEnd>();

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        currentBulletCount = bulletCount;
        bulletBar.maxValue = bulletCount;
        bulletBar.value = currentBulletCount;
        bulletCountText.text = currentBulletCount.ToString() + "/" + bulletCount.ToString();
    }

    void FixedUpdate()
    {
        Move();
        UpdateUI();
    }

    void Move()
    {
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        PlaySoundEffect(jumpSound);
    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<Rigidbody2D>().velocity = Vector2.right * 10;
        currentBulletCount--;
        PlaySoundEffect(shootSound);
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
        currentHealth = Mathf.Min(currentHealth + value, maxHealth);
    }

    public void WinGame()
    {
        PlaySoundEffect(winSound);
        gameEnd.EndGame(true);
    }

    public void KillPlayer()
    {
        PlaySoundEffect(deathSound);
        gameEnd.EndGame(false);
    }

    public void UpgradeSpeed(float value)
    {
        speed += value;
    }

    public void IncreaseBullet()
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
        bulletCountText.text = currentBulletCount.ToString() + "/" + bulletCount.ToString();
    }

    void PlaySoundEffect(AudioClip sound)
    {
        if (sound)
        {
            AudioSource.PlayClipAtPoint(sound, transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 8)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            KillPlayer();
        }
        if (collision.gameObject.layer == 11)
        {
            WinGame();
        }
        if (collision.gameObject.layer == 12)
        {
            UpgradeSpeed(collision.gameObject.GetComponent<SpeedUpgrades>().GetUpgrade());
        }
    }
}
