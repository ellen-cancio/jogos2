using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FPSAimController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public bool lockCursor = true;
    
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 50f;
    public float fireRate = 0.2f;
    public int maxAmmo = 30;
    public float reloadTime = 1.5f;
    public float maxShootDistance = 100f;
    
    public Image crosshair;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    
    public int scorePerHit = 10;
    public float gameDuration = 60f;
    public LayerMask targetLayer;
    
    private float xRotation = 0f;
    private int currentAmmo;
    private int currentScore;
    private float nextFireTime;
    private bool isReloading = false;
    private float gameTimer;
    private bool isGameActive = true;
    private Camera playerCamera;
    
    void Start()
    {
        currentAmmo = maxAmmo;
        currentScore = 0;
        gameTimer = gameDuration;
        playerCamera = GetComponentInChildren<Camera>();
        
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (restartButton != null) restartButton.gameObject.SetActive(false);
        
        UpdateUI();
    }
    
    void Update()
    {
        if (!isGameActive) return;
        
        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // Tempo
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            EndGame();
            return;
        }
        
        // Tiro e Recarga
        if (Input.GetButtonDown("Fire1") && !isReloading && currentAmmo > 0 && Time.time >= nextFireTime) Shoot();
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo) StartCoroutine(Reload());
        
        // Cursor Lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        UpdateUI();
    }
    
    void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        currentAmmo--;
        
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, maxShootDistance, targetLayer) ? hit.point : ray.GetPoint(maxShootDistance);
        
        Vector3 shootDirection = (targetPoint - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * bulletSpeed;
        Destroy(bullet, 5f);
    }
    
    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (ammoText != null) ammoText.text = isReloading ? "RELOADING..." : $"Ammo: {currentAmmo}/{maxAmmo}\nTime: {Mathf.CeilToInt(gameTimer)}s";
        if (scoreText != null) scoreText.text = $"Score: {currentScore}";
    }
    
    void EndGame()
    {
        isGameActive = false;
        if (gameOverText != null) { gameOverText.gameObject.SetActive(true); gameOverText.text = $"Game Over!\nFinal Score: {currentScore}"; }
        if (restartButton != null) restartButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void RestartGame()
    {
        currentScore = 0;
        currentAmmo = maxAmmo;
        gameTimer = gameDuration;
        isGameActive = true;
        isReloading = false;
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (restartButton != null) restartButton.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}