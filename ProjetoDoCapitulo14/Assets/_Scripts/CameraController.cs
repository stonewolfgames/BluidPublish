using UnityEngine;

// Classe responsável por controlar o movimento da câmera
public class CameraController : MonoBehaviour
{
    // Referência ao Transform do personagem (posição dele no mundo)
    public Transform player;

    // Limites horizontais da câmera
    public float minX; // até onde a câmera pode ir para a esquerda
    public float maxX; // até onde a câmera pode ir para a direita

    // Controla a suavidade do movimento da câmera
    public float smoothSpeed = 5f;

    // LateUpdate é usado para câmera pois é chamado após todos os Updates
    // Isso garante que o personagem já se moveu antes da câmera atualizar
    void LateUpdate()
    {
        // Se não houver referência ao jogador, não faz nada
        if (player == null) return;

        // Guarda a posição atual da câmera
        // Vamos modificar apenas o eixo X, mantendo Y e Z iguais
        Vector3 targetPosition = transform.position;

        // Captura a posição X atual do jogador
        // Esse é o valor que a câmera tentará seguir
        float targetX = player.position.x;

        /*
         * CLAMP (LIMITADOR DE VALOR)
         * 
         * Mathf.Clamp funciona como um "bloqueio" de valores.
         * Ele impede que targetX ultrapasse os limites definidos.
         * 
         * Sintaxe:
         * Mathf.Clamp(valor, minimo, maximo)
         * 
         * Comportamento:
         * - Se o jogador estiver dentro dos limites ? câmera segue normalmente
         * - Se passar do limite esquerdo ? câmera trava em minX
         * - Se passar do limite direito ? câmera trava em maxX
         * 
         * Exemplo prático:
         * minX = 0 | maxX = 10
         * jogador em X = -5 ? câmera fica em 0
         * jogador em X = 15 ? câmera fica em 10
         * jogador em X = 5 ? câmera acompanha normalmente
         */
        targetX = Mathf.Clamp(targetX, minX, maxX);

        // Atualiza apenas o eixo X da posição alvo da câmera
        // Y e Z permanecem os mesmos (importante para jogos 2D ou side-scroll)
        targetPosition.x = targetX;

        /*
         * LERP (INTERPOLAÇÃO SUAVE)
         * 
         * Vector3.Lerp move gradualmente de uma posição para outra.
         * Isso evita que a câmera "teleporte" diretamente para o jogador.
         * 
         * Parâmetros:
         * - posição atual
         * - posição alvo
         * - velocidade ajustada pelo tempo (Time.deltaTime)
         * 
         * Resultado:
         * movimento suave e mais natural
         */
        transform.position = Vector3.Lerp(
            transform.position,   // posição atual da câmera
            targetPosition,       // posição desejada
            smoothSpeed * Time.deltaTime // fator de suavização
        );
    }
}