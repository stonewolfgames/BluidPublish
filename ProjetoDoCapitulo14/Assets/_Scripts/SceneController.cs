// Importa a biblioteca principal do Unity, que contém as classes
// básicas utilizadas no desenvolvimento de jogos, como MonoBehaviour.
using UnityEngine;

// Importa a biblioteca responsável pelo gerenciamento de cenas,
// permitindo carregar, descarregar e consultar cenas do projeto.
using UnityEngine.SceneManagement;

// Define a classe SceneController, que herda de MonoBehaviour,
// permitindo que ela seja utilizada como um componente do Unity.
public class SceneController : MonoBehaviour
{
    // Método público que poderá ser chamado por um botão da interface.
    // Sua função é carregar a cena de gameplay.
    public void LoadGameScene()
    {
        // Carrega a cena chamada "GameScene".
        // Ao ser executado, o Unity descarrega a cena atual
        // e abre a nova cena especificada.
        SceneManager.LoadScene("GameScene");
    }
}