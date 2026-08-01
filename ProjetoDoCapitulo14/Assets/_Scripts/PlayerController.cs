using UnityEngine; // Biblioteca base do Unity
using UnityEngine.InputSystem; // Sistema de entrada moderno

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]

    private Rigidbody2D rigidBody; // Componente responsável pela física do personagem
    private Animator anim; // Controla as animações do personagem
    private AudioSource audioSource;

    private Vector2 moveInput; // Armazena o input de movimento (horizontal e vertical)

    public GameObject playerSprite; // Referência ao objeto visual do player

    public float speed = 5f; // Velocidade de movimento horizontal
    public float jumpForce = 8f; // Força aplicada no pulo

    public float sensorRadius = 0.15f; // Raio do sensor que detecta o chão

    // =========================
    // Configurações de ataque
    // =========================
    [Header("Attack Settings")]
    public GameObject playerAttackPrefab; // Prefab do ataque
    public Transform playerPointAttack; // Ponto onde o ataque será criado
    public float attackDuration = 0.3f; // Tempo antes do ataque ser destruído

    private void Start()
    {
        // Obtém o Rigidbody2D do objeto
        rigidBody = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        // Obtém o Animator a partir do sprite do player
        anim = playerSprite.GetComponent<Animator>();
    }

    // =========================
    // INPUT DE MOVIMENTO
    // =========================
    public void OnMove(InputAction.CallbackContext context)
    {
        // Lê o input do jogador (teclado, controle, etc)
        moveInput = context.ReadValue<Vector2>();
    }

    // =========================
    // INPUT DE PULO
    // =========================
    public void OnJump(InputAction.CallbackContext context)
    {
        // Executa apenas quando o botão é pressionado e o player está no chão
        if (context.started && OnGround())
        {
            audioSource.Play();

            // Mantém a velocidade horizontal e aplica força vertical
            rigidBody.linearVelocity = new Vector2(
                rigidBody.linearVelocity.x,
                jumpForce
            );
        }
    }

    void Update()
    {
        // =========================
        // ATAQUE
        // =========================
        if (Input.GetButtonDown("Fire1"))
        {
            ExecuteAttack();
        }

        // =========================
        // DIREÇÃO DO PERSONAGEM
        // =========================
        if (moveInput.x > 0.0f)
        {
            // Olhando para a direita
            transform.eulerAngles = new Vector2(0.0f, 0.0f);
        }
        else if (moveInput.x < 0.0f)
        {
            // Olhando para a esquerda
            transform.eulerAngles = new Vector2(0.0f, 180.0f);
        }

        // Atualiza animações
        Animations();
    }

    private void FixedUpdate()
    {
        // Movimento horizontal baseado em física
        // FixedUpdate é usado para garantir consistência na simulação física
        rigidBody.linearVelocity = new Vector2(
            moveInput.x * speed,
            rigidBody.linearVelocity.y
        );
    }

    // =========================
    // DETECÇÃO DE CHÃO
    // =========================
    bool OnGround()
    {
        // Cria um círculo na posição do player e verifica colisão com a layer "Ground"
        return Physics2D.OverlapCircle(
            transform.position,
            sensorRadius,
            1 << LayerMask.NameToLayer("Ground")
        );
    }

    // =========================
    // VISUALIZAÇÃO DO SENSOR
    // =========================
    private void OnDrawGizmos()
    {
        // Define cor do gizmo
        Gizmos.color = Color.yellow;

        // Desenha o sensor de chão na cena
        Gizmos.DrawSphere(transform.position, sensorRadius);
    }

    // =========================
    // ANIMAÇÕES
    // =========================
    void Animations()
    {
        // Controla animação de movimento (valor absoluto da velocidade)
        anim.SetFloat("pMove", Mathf.Abs(moveInput.x));

        // Controla animação de pulo (true = no chão)
        anim.SetBool("pJump", OnGround());
    }

    // =========================
    // ATAQUE
    // =========================
    void ExecuteAttack()
    {
        // Instancia o ataque na posição do ponto de ataque
        GameObject attack = Instantiate(
            playerAttackPrefab,
            playerPointAttack.position,
            playerPointAttack.rotation
        );

        // Define o ataque como filho do ponto de ataque
        attack.transform.SetParent(playerPointAttack);

        // Destroi o ataque após um tempo
        Destroy(attack, attackDuration);
    }
}