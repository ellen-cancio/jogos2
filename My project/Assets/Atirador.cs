using UnityEngine;
using System.Collections;

public class Atirador : MonoBehaviour
{
    [Header("Configurações do Tiro")]
    public GameObject balaPrefab;
    public Transform pontoDeTiro;
    public float forcaDoTiro = 30f;

    [Header("Configurações de Recarga")]
    public float tempoDeRecarga = 2.0f;
    private bool estaRecarregando = false;

    private GameManager gameManager; // Guarda a referência do GameManager

    void Start()
    {
        // Encontra o GameManager que está na cena automaticamente
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (estaRecarregando || gameManager == null) return;

        // Atira ao clicar com o botão esquerdo
        if (Input.GetButtonDown("Fire1"))
        {
            // Pergunta para o GameManager se pode atirar
            if (gameManager.PodeAtirar())
            {
                Atirar();
            }
            else
            {
                Debug.Log("Sem munição! Aperte R para recarregar.");
            }
        }

        // Recarrega ao apertar R
        if (Input.GetKeyDown(KeyCode.R) && gameManager.PrecisaRecarregar())
        {
            StartCoroutine(RecarregarCoroutine());
        }
    }

    void Atirar()
    {
        // Manda o GameManager gastar uma bala
        gameManager.GastarMunicao();

        // Código físico do tiro
        GameObject novaBala = Instantiate(balaPrefab, pontoDeTiro.position, pontoDeTiro.rotation);
        Rigidbody rb = novaBala.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(pontoDeTiro.forward * forcaDoTiro, ForceMode.Impulse);
        }
    }

    IEnumerator RecarregarCoroutine()
    {
        estaRecarregando = true;

        // Pede para o GameManager escrever "Recarregando..." na tela
        gameManager.MostrarTextoRecarregando();

        yield return new WaitForSeconds(tempoDeRecarga);

        // Termina o tempo e manda o GameManager resetar as balas
        gameManager.RecarregarMunicao();
        estaRecarregando = false;
    }
}