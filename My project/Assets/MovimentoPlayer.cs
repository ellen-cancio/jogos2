using UnityEngine;

public class MovimentoPlayer : MonoBehaviour 
{
    public float velocidade = 5f;
    public float sensibilidadeMouse = 2f;
    public Transform cameraJogador;
    
    private float rotacaoX = 0f;
    private Rigidbody rb;
    private Vector3 direcaoMovimento;

    void Start() 
    { 
        Cursor.lockState = CursorLockMode.Locked; 
        rb = GetComponent<Rigidbody>(); 
    }

    void Update() 
    {
        // Zera o movimento no início de todo frame
        float moveX = 0f;
        float moveZ = 0f;

        // Lê as teclas (WASD ou Setinhas)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveZ = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        
        // Salva a direção para a física usar depois
        direcaoMovimento = (transform.right * moveX + transform.forward * moveZ).normalized;

        // Controle da câmera pelo mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;
        
        transform.Rotate(Vector3.up * mouseX);
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);
        cameraJogador.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
    }

    void FixedUpdate()
    {
        // Aplica a força física para andar e BATER nas paredes
        // Mantemos o rb.velocity.y para a gravidade continuar puxando ele pra baixo!
        rb.velocity = new Vector3(direcaoMovimento.x * velocidade, rb.velocity.y, direcaoMovimento.z * velocidade);
    }
}