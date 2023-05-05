using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;         // Prefab del proyectil
    public Transform projectileSpawnPoint;      // Punto de spawn del proyectil
    public float fireRate = 0.5f;                // Velocidad de disparo
    public float projectileSpeed = 10f;
    public float reloadTime = 2f;                // Tiempo de recarga
    public int maxAmmo = 10;                     // Cantidad máxima de munición
    public float zoomFOV = 20f;                  // FOV del zoom
    public float zoomSpeed = 5f;                 // Velocidad del zoom
    public KeyCode fireKey = KeyCode.Mouse0;     // Tecla de disparo
    public KeyCode zoomKey = KeyCode.Mouse1;     // Tecla de zoom

    private float fireTimer;
    private float reloadTimer;
    private int currentAmmo;
    private bool isZoomed = false;
    private Camera mainCamera;
    private float defaultFOV;

    void Start()
    {
        fireTimer = fireRate;
        reloadTimer = 0f;
        currentAmmo = maxAmmo;
        mainCamera = Camera.main;
        defaultFOV = mainCamera.fieldOfView;
    }

    void Update()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            isZoomed = true;
        }
        else if (Input.GetKeyUp(zoomKey))
        {
            isZoomed = false;
        }

        if (isZoomed)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, defaultFOV, Time.deltaTime * zoomSpeed);
        }

        fireTimer += Time.deltaTime;
        if (Input.GetKey(fireKey) && fireTimer >= fireRate && currentAmmo > 0 && reloadTimer <= 0f)
        {
            Fire();
            fireTimer = 0f;
            currentAmmo--;
            if (currentAmmo <= 0)
            {
                reloadTimer = reloadTime;
            }
        }

        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                currentAmmo = maxAmmo;
            }
        }
    }

    void Fire()
    {
    GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
    projectileRigidbody.velocity = projectile.transform.forward * projectileSpeed; // velocidad del proyectil
    projectileRigidbody.rotation = Quaternion.LookRotation(projectileRigidbody.velocity); // dirección del proyectil
}
}

