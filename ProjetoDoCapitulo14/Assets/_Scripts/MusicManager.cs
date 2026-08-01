using UnityEngine;

// Classe responsável por controlar a música do jogo.
// Herda de MonoBehaviour para poder ser adicionada a um GameObject da Unity.
public class MusicManager : MonoBehaviour
{
    // Variável estática que armazena a única instância do MusicManager existente no jogo.
    // O uso de "static" permite que todos os objetos tenham acesso à mesma referência.
    private static MusicManager instance;

    // O método Awake() é chamado automaticamente pela Unity
    // assim que o objeto é carregado na cena.
    private void Awake()
    {
        // Verifica se ainda não existe um MusicManager ativo.
        if (instance == null)
        {
            // Guarda a referência do objeto atual como a instância principal.
            instance = this;

            // Impede que este objeto seja destruído quando uma nova cena for carregada.
            // Dessa forma, o Audio Source continuará reproduzindo a música.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Caso já exista outro MusicManager ativo,
            // este objeto será destruído para evitar duplicação.
            //
            // Isso impede que duas músicas sejam reproduzidas ao mesmo tempo
            // quando o jogador troca de cenas.
            Destroy(gameObject);
        }
    }
}