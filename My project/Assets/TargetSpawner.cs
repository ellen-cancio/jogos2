using UnityEngine;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform position;       
        public GameObject targetPrefab;  
        public int quantity = 1;         
        public Vector3 scale = Vector3.one; 
        public Vector3 rotation = Vector3.zero; 

        // Movimento
        public bool moveHorizontal = false;
        public bool moveVertical = false;
        public float moveSpeed = 3f;
        public float moveRange = 5f;

        public int health = 1;
        public int pointsValue = 10;
    }

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    private List<GameObject> spawnedTargets = new List<GameObject>();

    // Referência ao GameManager e controle para o fim de jogo
    private GameManager gameManager;
    private bool jogoTerminou = false;

    void Start()
    {
        // Encontra o GameManager na cena
        gameManager = FindObjectOfType<GameManager>();

        // Cria os alvos no início
        SpawnAllTargets();
    }

    void Update()
    {
        // Limpa os alvos destruídos da lista
        spawnedTargets.RemoveAll(t => t == null);

        // SE TODOS OS ALVOS MORRERAM:
        // Checa se a lista está vazia, se o jogo já não acabou antes e se pelo menos um alvo foi criado
        if (spawnedTargets.Count == 0 && !jogoTerminou)
        {
            jogoTerminou = true; // Evita que este "if" fique rodando em loop infinito
            FinalizarPartida();
        }
    }

    void FinalizarPartida()
{
    Debug.Log("Todos os alvos foram destruídos! Fim de Jogo.");

    if (gameManager != null)
    {
        // Chama a função pública do GameManager para ativar a tela
        gameManager.FimDeJogo(); 
    }
}

    void SpawnAllTargets()
    {
        foreach (SpawnPoint point in spawnPoints)
        {
            for (int i = 0; i < point.quantity; i++)
            {
                SpawnTarget(point);
            }
        }
    }

    void SpawnTarget(SpawnPoint point)
    {
        if (point.position == null || point.targetPrefab == null) return;

        GameObject target = Instantiate(point.targetPrefab, point.position.position, Quaternion.Euler(point.rotation));
        target.transform.localScale = point.scale;

        Alvo targetScript = target.GetComponent<Alvo>();
        if (targetScript != null)
        {
            targetScript.spawnPoint = point;
            targetScript.moveHorizontal = point.moveHorizontal;
            targetScript.moveVertical = point.moveVertical;
            targetScript.moveSpeed = point.moveSpeed;
            targetScript.moveRange = point.moveRange;
            targetScript.health = point.health;
            targetScript.pointsValue = point.pointsValue;
        }

        spawnedTargets.Add(target);
    }
}