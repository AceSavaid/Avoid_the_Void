using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] TMP_Text bulletCountText;


    [Header("Health")]
    [SerializeField] float maxHealth = 3;
    float currentHealth;
    [SerializeField] Slider healthBar;
    

    [Header("SoundEffects")]
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip jumpSound;

    Rigidbody2D rb;
    GameEnd gameEnd;


    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameEnd = FindObjectOfType<GameEnd>();

        //ui and stat setting 
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        currentBulletCount = bulletCount;
        bulletBar.maxValue = bulletCount;
        bulletBar.value = currentBulletCount;
        bulletCountText.text = currentBulletCount.ToString() + "/" + bulletCount.ToString();

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
        float xmovement = Mathf.Lerp(transform.position.x, transform.position.x+ movedir, Time.deltaTime * speed);
        transform.position = new Vector3(xmovement, transform.position.y, transform.position.z);
        //rb.velocity = new Vector2(movedir * speed, rb.velocity.y);

    }
    void Jump()
    {
        rb.velocity = new Vector2(0, jumpheight);
        PlaySoundEffect(jumpSound);
    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet, this.transform);
        b.GetComponent<Rigidbody2D>().velocity = Vector2.right *10;
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
        currentHealth += value;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
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
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 8) //ground or breakable wall
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
