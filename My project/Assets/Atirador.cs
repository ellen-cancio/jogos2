using UnityEngine;
public class Atirador : MonoBehaviour {
    public GameObject prefabDaBala;
    public Transform firePoint;
    public float velocidadeDaBala = 40f;
    public Camera cameraJogador;
    public GameManager gameManager;

    void Update() {
        if (Input.GetButtonDown("Fire1") && gameManager.municao > 0) {
            AtirarComMira(); gameManager.GastarMunicao();
        }
    }
    void AtirarComMira() {
        Ray raio = cameraJogador.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 destino = Physics.Raycast(raio, out RaycastHit hit) ? hit.point : raio.GetPoint(1000);
        
        GameObject bala = Instantiate(prefabDaBala, firePoint.position, firePoint.rotation);
        Vector3 direcao = (destino - firePoint.position).normalized;
        bala.GetComponent<Rigidbody>().velocity = direcao * velocidadeDaBala;
    }
}