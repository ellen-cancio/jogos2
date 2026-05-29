using UnityEngine;
public class Bala : MonoBehaviour {
    void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}