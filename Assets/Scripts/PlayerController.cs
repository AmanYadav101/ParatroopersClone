using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action<float> OnRotatePlayer;
    public static event Action<Vector2> OnShoot;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed =10f;
    public float fireRate = 0.5f;
    private float _nextFireTime = 0f;
    
    public float rotationSpeed = 100f;
    public float maxRotationAngle = 45f;
    public float sensitivity = 2f;
    private float _currentRotation = 0f;

    private void OnEnable()
    {
        OnRotatePlayer += RotateTurret;
    }

    private void OnDisable()
    {
        OnRotatePlayer -= RotateTurret;
    }

    private void Update()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        if (rotationInput != 0)
        {
            RotatePlayer(rotationInput);
        }

        if (Input.GetKey(KeyCode.Space)&&Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + fireRate;
        }
    }

    private void RotateTurret(float rotationInput)
    {
        float rotationAmount = -rotationInput * rotationSpeed * sensitivity * Time.deltaTime;
        float newRotation = _currentRotation + rotationAmount;

        newRotation = Mathf.Clamp(newRotation, -maxRotationAngle, maxRotationAngle);


        transform.rotation = Quaternion.Euler(0, 0, newRotation);
        _currentRotation = newRotation;
    }

    private void Shoot()
    {
        OnShoot?.Invoke(firePoint.position);
        if (bulletPrefab && firePoint )
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.velocity = firePoint.up * bulletSpeed;
            }
        }
    }


    private static void RotatePlayer(float rotationInput)
    {
        OnRotatePlayer?.Invoke(rotationInput);
    }
}