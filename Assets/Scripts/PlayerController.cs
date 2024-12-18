using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;

    public float playerSpeed = 1f;
    public float currentSpeed;

    public Vector2 playDirection;

    private bool IsWalking;

    private Animator playerAnimator;

    private bool playerFacingRight = true;

    public int punchCount;

    private float timeCross = 1f;

    private bool comboControl;

    private bool IsDead;

    public int maxHealth = 50;
    public int currentHealth;
    public Sprite playerImage;

    // SFX do player
    private AudioSource playerAudioSource;

    public AudioClip jabSound;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        playerAnimator = GetComponent<Animator>();

        currentHealth = maxHealth;

        currentSpeed = playerSpeed;

        // Inicia o componente AudioSource do Player
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        UpdateAnimator();


        if (Input.GetKeyDown(KeyCode.X))
        {
            if (IsWalking == false && !comboControl)
            
                StartCoroutine(CrossController());
                if(punchCount < 2)
                {
                    PlayerJab();
                    punchCount++;
                }
                else if (punchCount >= 2) {
                    PlayerCroos();
                    punchCount = 0;
                }
                StopCoroutine(CrossController());
            
        }
    }

    private void FixedUpdate()
    {
        if (playDirection.x != 0 || playDirection.y != 0)
        {
            IsWalking = true;
        }
        else 
        {
            IsWalking = false;
        }
        
        //playerRigidBody.MovePosition(playerRigidBody.position + playerSpeed * Time.fixedDeltaTime * playDirection);
        playerRigidBody.MovePosition(playerRigidBody.position + currentSpeed * Time.fixedDeltaTime * playDirection);

    }

    void PlayerMove()
    {
        playDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (playDirection.x < 0 && playerFacingRight)
        {
            Flip();
        }

        else if (playDirection.x > 0 && !playerFacingRight)
        {
            Flip();
        }
    }

    void UpdateAnimator()
    {
        playerAnimator.SetBool("IsWalking", IsWalking);

    }

    void Flip()
    {
        playerFacingRight = !playerFacingRight;

        transform.Rotate(0, 180, 0);
    }

    void PlayerJab()
    {
        playerAnimator.SetTrigger("IsJab");

        // Definir o SFX á ser reproduzido
        playerAudioSource.clip = jabSound;

        //Executar o SFX
        playerAudioSource.Play();
    } 

    void PlayerCroos()
    {
        playerAnimator.SetTrigger("IsCross");

        // Definir o SFX á ser reproduzido
        playerAudioSource.clip = jabSound;

        //Executar o SFX
        playerAudioSource.Play();
    }

    IEnumerator CrossController()
    {
        comboControl = true;
        yield return new WaitForSeconds(timeCross);
        punchCount = 0;
        comboControl = false;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed ()
    {
        currentSpeed = playerSpeed;         
    }

    public void TakeDamage(int damage)
    {
        if (!IsDead)
        {
            currentHealth -= damage;
            FindAnyObjectByType<UIManager>().UpdatePlayerHealth(currentHealth);
        }
    }
}
