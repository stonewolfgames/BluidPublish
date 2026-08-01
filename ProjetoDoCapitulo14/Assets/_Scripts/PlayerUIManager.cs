using UnityEngine;
using UnityEngine.UI; // Permite trabalhar com elementos de UI, como Text

// Classe responsável por controlar e atualizar a interface do jogador
public class PlayerUIManager : MonoBehaviour
{
    // Referência ao texto que exibirá a vida do jogador
    [SerializeField] private Text playerTextHealth;

    // Referência ao texto que exibirá a quantidade de itens
    [SerializeField] private Text playerTextItem;

    // Armazena o valor atual da vida do jogador
    public int playerHealth;

    // Armazena a quantidade atual de itens coletados
    public int playerItem;

    // Awake é chamado assim que o objeto é carregado na cena
    private void Awake()
    {
        // Define valores iniciais do jogador
        playerHealth = 100;
        playerItem = 0;

        // Atualiza imediatamente a interface com os valores iniciais
        // Isso evita que a UI comece vazia ou com valores incorretos
        ShowValuesUI(playerHealth, playerItem);
    }

    // Método responsável por atualizar os textos da interface
    // Recebe os valores atuais como parâmetro
    public void ShowValuesUI(int health, int item)
    {
        // Atualiza o texto da vida
        // Converte o número para string e adiciona o símbolo de porcentagem
        playerTextHealth.text = health.ToString() + " %";

        // Atualiza o texto dos itens
        // Apenas converte o valor numérico para string
        playerTextItem.text = item.ToString();
    }
}