using UnityEngine;
public class Alvo : MonoBehaviour {
    public GameManager gameManager;
    public int pontos = 10;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Bala>() != null) {
            gameManager.AdicionarScore(pontos);
            Destroy(gameObject);
        }
    }
}