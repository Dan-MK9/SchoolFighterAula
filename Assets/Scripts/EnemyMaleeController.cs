using UnityEngine;

public class EnemyMaleeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // Variavel que indica se o iimigo est� vivo
    public bool IsDead;

    // Variaveis para controlar o lado que o inimigo est� virado
    public bool facingRight;
    public bool previousDirectionRight;

    // Variavel para armazenar posi��o do player
    private Transform target;

    // Variaveis para movimenta��o do inimigo
    private float enemySpeed = 0.3f;
    private float currentSpeed;

    private bool IsWalking;

    private float horizontalForce;
    private float verticalForce;

    private float walkTimer;

    private float attackRate = 1f;
    private float nextAttack;

    // Variaveis para mecanica de dano
    public int maxHealth;
    public int currentHealth;
    public Sprite enemyImage;

    public float staggerTime = 0.5f;
    private float demageTimer;
    public bool isTakingDemage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buscar o Player e armazenar sua posi��o
        target = FindAnyObjectByType<PlayerController>().transform;

        // Inicializar a velocidade do inimigo
        currentSpeed = enemySpeed;

        // Inicializar a vida do inimigo
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Verificar se o player est� para esquerda ou para direira
        // e com isso eterminar o lado que o inimigo ficar� virado
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        // Se faceRight for true, vamos virar o inimigo em 180 graus no eixo Y,
        // se n�o, vamos virar o inimigo para a esquerda

        // Se o player � direita e a dire��o anterior n�o era direita (estava olhando para esquerda)
        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        // Se o player n�o esta � direita e a dire��o anterior era direita (estava olhando para esquerda)
        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        // Iniciar o timer do caminhar do inimigo
        walkTimer += Time.deltaTime;

        // Gerenciar a anima��o do inimigo
        if(horizontalForce == 0 && verticalForce == 0)
        {
            IsWalking = false;
        }
        else
        {
            IsWalking = true;
        }

        if (isTakingDemage && !IsDead)
        {
            demageTimer += Time.deltaTime;

            ZeroSpeed();

            if (demageTimer >= staggerTime)
            {
                isTakingDemage = false;
                demageTimer = 0;

                ResetSpeed();
            }
        }

        // Atualiza o animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (!IsDead)
        {
            // Movimenta��o

            // Variavel para armazenar a distancia entre o inimigo e o player
            Vector3 targetDistance = target.position - this.transform.position;

            // Determina se a for�a horizontal deve ser negativa ou positiva
            // 5 / 5 = 1
            // -5 / 5 = -1
            horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            // Entre 1 e 2 segundos, ser� feita uma defini��o de dire��o vertical
            if (walkTimer >= Random.Range(1f, 2f))
            {
                verticalForce = Random.Range(-1, 2);

                // Zera o timer de movimenta��o para andar verticalmente novamente daqui a +- 1 segundo
                walkTimer = 0;
            }

            //Caso esteja perto do player, parar a movimenta��o
            if (Mathf.Abs(targetDistance.x) < 0.2f)
            {
                horizontalForce = 0;
            }

            // Aplica a velocidade do inimigo fazendo o movimentar
            rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);

            // Ataque
            // Se estiver pr�ximo do player e o timer do jogo for maior do que o valor de nextAttack
            if (Mathf.Abs(targetDistance.x) < 0.2f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
            {
                animator.SetTrigger("Attack");

                ZeroSpeed();

                nextAttack = Time.time + attackRate;
            }
        }

    }

    void UpdateAnimator()
    {
        animator.SetBool("IsWalking", IsWalking);
    }

    public void TakeDemage(int demage)
    {
        if (!IsDead)
        {
            isTakingDemage = true;

            currentHealth -= demage;

            animator.SetTrigger("HitDamange");

            FindAnyObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

            if (currentHealth < 0)
            {
                IsDead = true;

                ZeroSpeed();

                animator.SetTrigger("Dead");
            }
        }
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = enemySpeed;
    }

    public void OnDisable()
    {
        this.gameObject.SetActive(false);
    }
}
