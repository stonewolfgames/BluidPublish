using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Classe responsável pelo controle do inimigo Bee.
// Controla movimento, comportamento de ataque, vulnerabilidade,
// recebimento de dano e retorno para a tela inicial após a derrota.
public class BeeController : MonoBehaviour
{

    // =========================
    // CONFIGURAÇÕES DE MOVIMENTO
    // =========================

    // Velocidade de deslocamento horizontal do inimigo.
    // Controla a velocidade com que a abelha se move para esquerda ou direita.
    public float speed = 2f;

    // Define a altura do movimento vertical.
    // Quanto maior o valor, maior será a distância que o inimigo sobe e desce.
    public float amplitude = 1f;

    // Define a velocidade da oscilação vertical.
    // Controla quantas vezes a onda senoidal se repete.
    public float frequency = 2f;

    // Tempo que o inimigo permanece parado e vulnerável.
    public float waitTime = 3f;


    // Limites horizontais do movimento do inimigo.
    // Quando atingir esses valores, ele muda para o estado vulnerável.
    public float leftLimit = -8f;
    public float rightLimit = 8f;


    // Referência ao componente Rigidbody2D.
    // Será utilizado para controlar o movimento através da física da Unity.
    private Rigidbody2D rb;

    // Referência ao componente responsável pelo desenho do personagem.
    // Será utilizado para alterar a cor quando receber dano.
    private SpriteRenderer sr;

    // Referência ao componente de áudio.
    // Utilizado para reproduzir o som ao receber dano.
    private AudioSource audioSource;


    // Guarda a posição inicial do inimigo no eixo Y.
    // Serve como referência para o movimento ondulado.
    private float baseY;

    // Controla o tempo utilizado no cálculo da função seno.
    private float timeCounter;



    // =========================
    // CONTROLE DE ESTADO
    // =========================

    // Define se o inimigo está se movimentando.
    // true = inimigo voando
    // false = inimigo parado e vulnerável
    private bool moving = true;


    // Controla o tempo restante do estado parado.
    private float waitCounter;


    // Define a direção do movimento horizontal.
    // 1 = direita
    // -1 = esquerda
    private int direction = 1;


    // Quantidade de vida do inimigo.
    // Cada ataque recebido reduz esse valor.
    private int life = 5;



    // Executado uma única vez quando o objeto é iniciado.
    void Start()
    {

        // Busca o componente Rigidbody2D presente no próprio objeto.
        rb = GetComponent<Rigidbody2D>();

        // Busca o componente AudioSource para tocar o som de dano.
        audioSource = GetComponent<AudioSource>();

        // Busca o SpriteRenderer para alterar a cor do inimigo.
        sr = rb.GetComponent<SpriteRenderer>();


        // Salva a posição inicial vertical.
        // O movimento da onda será calculado baseado nesse ponto.
        baseY = rb.position.y;
    }



    // Executado em intervalos fixos de tempo.
    // Ideal para trabalhar com física utilizando Rigidbody.
    void FixedUpdate()
    {

        // Caso o inimigo esteja em movimento,
        // executa o padrão de voo.
        if (moving)
        {
            MovePattern();
        }

        // Caso contrário, executa o estado vulnerável.
        else
        {
            WaitState();
        }
    }



    // =========================
    // MOVIMENTO SENOIDAL
    // =========================

    void MovePattern()
    {

        // Incrementa o contador de tempo.
        // Ele será usado para calcular a onda senoidal.
        timeCounter += Time.fixedDeltaTime;


        // Movimento horizontal constante.
        // A direção define se vai para direita ou esquerda.
        float xMovement = direction * speed;



        // Calcula o movimento vertical utilizando seno.
        // Mathf.Sin gera valores entre -1 e 1,
        // criando um movimento suave de subida e descida.
        float yOffset = Mathf.Sin(timeCounter * frequency) * amplitude;


        // Define a nova posição vertical.
        float newY = baseY + yOffset;



        // Aplica velocidade no Rigidbody2D.
        //
        // X:
        // Movimento horizontal da abelha.
        //
        // Y:
        // Calcula a velocidade necessária para alcançar
        // a nova posição vertical.
        rb.linearVelocity = new Vector2(
            xMovement,
            (newY - rb.position.y) / Time.fixedDeltaTime
        );



        // Verifica se o inimigo chegou ao limite da tela.
        if (rb.position.x > rightLimit || rb.position.x < leftLimit)
        {

            // Ao chegar ao limite,
            // muda para o estado parado e vulnerável.
            EnterWaitState();
        }
    }



    // =========================
    // ENTRA NO ESTADO DE PAUSA
    // =========================

    void EnterWaitState()
    {

        // Desativa o movimento do inimigo.
        moving = false;



        // Verifica o lado em que o inimigo está.
        // Isso faz ele virar para ficar voltado para o jogador.
        if (transform.position.x > 0)
        {
            transform.eulerAngles = new Vector2(0f, 180f);

            transform.position = new Vector2(
                transform.position.x - 1.0f,
                transform.position.y
            );
        }
        else
        {
            transform.eulerAngles = new Vector2(0f, 0f);

            transform.position = new Vector2(
                transform.position.x + 1.0f,
                transform.position.y
            );
        }



        // Define o tempo que ficará vulnerável.
        waitCounter = waitTime;


        // Remove a velocidade para ficar completamente parado.
        rb.linearVelocity = Vector2.zero;
    }



    // =========================
    // ESTADO PARADO (VULNERÁVEL)
    // =========================

    void WaitState()
    {

        // Diminui o tempo restante do estado vulnerável.
        waitCounter -= Time.fixedDeltaTime;



        // Quando o tempo acabar,
        // o inimigo volta a se movimentar.
        if (waitCounter <= 0f)
        {

            // Inverte a direção do movimento.
            direction *= -1;


            // Atualiza a nova altura base.
            // Evita que pequenos erros acumulem durante o movimento.
            baseY = rb.position.y;


            // Reinicia o cálculo da senoide.
            timeCounter = 0f;


            // Retorna ao estado de movimento.
            moving = true;
        }
    }



    // =========================
    // DETECÇÃO DE DANO
    // =========================

    private void OnTriggerEnter2D(Collider2D other)
    {

        // Verifica se o objeto que colidiu possui a tag PlayerAttack.
        // Somente ataques do jogador podem causar dano.
        if (other.CompareTag("PlayerAttack"))
        {

            // Executa a rotina de dano.
            StartCoroutine(Damage());
        }
    }



    // Corroutine utilizada para criar um efeito visual de dano.
    IEnumerator Damage()
    {

        // Reproduz o som de impacto.
        audioSource.Play();


        // Altera temporariamente a cor do inimigo para vermelho.
        sr.color = Color.red;


        // Aguarda 0,3 segundos.
        yield return new WaitForSeconds(0.3f);


        // Retorna a cor original.
        sr.color = Color.white;



        // Remove um ponto de vida do inimigo.
        life--;



        // Verifica se o inimigo foi derrotado.
        if (life < 0)
        {

            // Carrega novamente a cena inicial.
            // Representa o fim da batalha após derrotar o inimigo.
            SceneManager.LoadScene("StartScene");
        }
    }
}