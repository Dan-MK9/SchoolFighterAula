using System;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private bool facingRight;
    private bool previousDirectionRight;

    private bool IsDead;

    private Transform target;

    private float enemySpeed = 0.3f;
    private float currentSpeed;

    private float verticalForce, horizontalForce;

    private bool IsWalking = false;

    private float walkTimer;

    public int maxHealth;
    public int currentHealth;

    private float staggerTime = 0.6f;
    private bool IsTalkingDamage = false;
    private float damageTimer;

    private float attackRate = 1f;
    private float nextAttack;

    public Sprite enemyImage;

    // Variavel para armazenar o projetil
    public GameObject projectile;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buscar o Player e armazenar sua posição
        target = FindAnyObjectByType<PlayerController>().transform;

        // Inicializar a velocidade do inimigo
        currentSpeed = enemySpeed;

        // Inicializar a vida do inimigo
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Verificar se o player está para esquerda ou para direira
        // e com isso eterminar o lado que o inimigo ficará virado
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        // Se faceRight for true, vamos virar o inimigo em 180 graus no eixo Y,
        // se não, vamos virar o inimigo para a esquerda

        // Se o player à direita e a direção anterior não era direita (estava olhando para esquerda)
        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        // Se o player não esta à direita e a direção anterior era direita (estava olhando para esquerda)
        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        // Iniciar o timer do caminhar do inimigo
        walkTimer += Time.deltaTime;

        // Gerenciar a animação do inimigo
        if (horizontalForce == 0 && verticalForce == 0)
        {
            IsWalking = false;
        }
        else
        {
            IsWalking = true;
        }

        if (IsTalkingDamage && !IsDead)
        {
            damageTimer += Time.deltaTime;

            ZeroSpeed();

            if (damageTimer >= staggerTime)
            {
                IsTalkingDamage = false;
                damageTimer = 0;

                ResetSpeed();
            }
        }

        // Atualiza o animator
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsWalking", IsWalking);
    }

    private void ResetSpeed()
    {
        currentSpeed = enemySpeed;
    }

    private void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        IsTalkingDamage = true;

        currentHealth -= damage;

        animator.SetTrigger("HitDamage");

        FindFirstObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

        if (currentHealth <= 0)
        {
            IsDead = true;

            //corrige o bug do inimigo deslizar apos morto
            rb.linearVelocity = Vector2.zero;

            animator.SetTrigger("Dead");
        }
    }

    public void FixedUpdate()
    {
        if ( IsDead)
        {
            Vector3 targetDistance = target.position - this.transform.position;

            if (walkTimer >= UnityEngine.Random.Range(2.5f, 3.5f))
            {
                verticalForce = targetDistance.y / Math.Abs(targetDistance.y);
                horizontalForce = targetDistance.x / Math.Abs(targetDistance.x);

                walkTimer = 0;
            }

            if (MathF.Abs(targetDistance.x) < 1f)
            {
                horizontalForce = 0;
            }

            if (MathF.Abs(targetDistance.y) < 0.5f)
            {
                verticalForce = 0;
            }

            if (!IsTalkingDamage)
            {
                rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);
            }

            // Logica do ataque
            if (Mathf.Abs(targetDistance.x) < 1.3f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
            {
                // Ataque do inimigo
                animator.SetTrigger("Attack");
                ZeroSpeed();

                nextAttack = Time.time + attackRate;
            }
        }
    }
    public void Shoot()
    {
        // Define a posição do spawn do projetil
        Vector2 spawnPosition = new Vector2(this.transform.position.x, this.transform.position.y + 0.2f);

        // spawnar o projetil na posção definida
        GameObject shotObject = Instantiate(projectile, spawnPosition, Quaternion.identity);

        // ativar o projetil
        shotObject.SetActive(true);

        var shotPhysics = shotObject.GetComponent<Rigidbody2D>();

        if (facingRight)
        {
            //Aplica força no projetil para ele se desolocar para a direita
            shotPhysics.AddForceX(80f);
        }
        else
        {
            // aplica força no projetil para ele se desolocar para a esquerda
            shotPhysics.AddForceX(-80f);
        }
    }

    internal void TakeDemage(int demage)
    {
        throw new NotImplementedException();
    }
}


