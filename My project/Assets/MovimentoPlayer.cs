using UnityEngine;
public class MovimentoPlayer : MonoBehaviour {
    public float velocidade = 5f;
    public float sensibilidadeMouse = 2f;
    public Transform cameraJogador;
    private float rotacaoX = 0f;

    void Start() { Cursor.lockState = CursorLockMode.Locked; }
    void Update() {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        transform.position += (transform.right * moveX + transform.forward * moveZ) * velocidade * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;
        transform.Rotate(Vector3.up * mouseX);
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);
        cameraJogador.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
    }
}