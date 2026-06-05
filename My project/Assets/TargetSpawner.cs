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
        public bool moveHorizontal = false;
        public bool moveVertical = false;
        public int health = 1;
        public int pointsValue = 10;
    }

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    private List<GameObject> spawnedTargets = new List<GameObject>();

    void Start() => SpawnAllTargets();

    void Update()
    {
        spawnedTargets.RemoveAll(t => t == null);
        foreach (var point in spawnPoints)
        {
            int count = 0;
            foreach (var target in spawnedTargets)
                if (target != null && target.GetComponent<Target>().spawnPoint == point) count++;
            
            if (count < point.quantity) SpawnTarget(point);
        }
    }

    void SpawnAllTargets()
    {
        foreach (var point in spawnPoints)
            for (int i = 0; i < point.quantity; i++) SpawnTarget(point);
    }

    void SpawnTarget(SpawnPoint point)
    {
        if (point.position == null || point.targetPrefab == null) return;
        GameObject target = Instantiate(point.targetPrefab, point.position.position, Quaternion.identity);
        target.transform.localScale = point.scale;
        
        Target tScript = target.GetComponent<Target>();
        tScript.spawnPoint = point;
        tScript.moveHorizontal = point.moveHorizontal;
        tScript.moveVertical = point.moveVertical;
        tScript.health = point.health;
        tScript.pointsValue = point.pointsValue;
        
        spawnedTargets.Add(target);
    }
}