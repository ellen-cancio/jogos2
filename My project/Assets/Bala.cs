using UnityEngine;

public class Bala : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // O 0.05f faz a bala esperar uma fração de segundo antes de sumir.
        // Assim dá tempo do alvo perceber que apanhou!
        Destroy(gameObject, 0.05f); 
    }
}