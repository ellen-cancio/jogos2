using UnityEngine;

public class Target : MonoBehaviour
{
    [HideInInspector] public TargetSpawner.SpawnPoint spawnPoint;
    [HideInInspector] public bool moveHorizontal;
    [HideInInspector] public bool moveVertical;
    [HideInInspector] public int health;
    [HideInInspector] public int pointsValue;

    private float directionX = 1f;
    private float moveSpeed = 3f;
    private float moveRange = 5f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 newPos = transform.position;
        if (moveHorizontal)
        {
            newPos.x += directionX * moveSpeed * Time.deltaTime;
            if (Mathf.Abs(newPos.x - startPosition.x) >= moveRange) directionX *= -1;
        }
        transform.position = newPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) 
        {
            health--;
            if (health <= 0)
            {
                FindObjectOfType<FPSAimController>().AddScore(pointsValue);
                Destroy(gameObject);
            }
            Destroy(other.gameObject);
        }
    }
}