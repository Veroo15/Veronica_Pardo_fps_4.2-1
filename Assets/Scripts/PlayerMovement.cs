using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Velocidad normal de movimiento
    public float sprintSpeed = 10f;     // Velocidad de sprint
    public float sprintDuration = 2f;   // Duración máxima del sprint
    public float sprintCooldown = 3f;   // Tiempo de espera para volver a sprintear
    public KeyCode sprintKey = KeyCode.LeftShift;  // Tecla de sprint

    private Rigidbody rb;
    private float sprintTimer;
    private bool isSprinting = false;
    private float sprintTime = 0f;
    private Transform cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Desactivamos la rotación para evitar problemas con la física
        sprintTimer = sprintCooldown;
        cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        // Comprobamos si el jugador puede sprintear de nuevo
        if (sprintTimer < sprintCooldown)
        {
            sprintTimer += Time.fixedDeltaTime;
        }

        // Detectamos la entrada del usuario para el movimiento
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Obtenemos la dirección de movimiento en base a la cámara
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = (verticalInput * camForward + horizontalInput * cam.right).normalized;

        // Si el usuario presiona la tecla de sprint y puede hacerlo, aumentamos la velocidad de movimiento
        if (Input.GetKey(sprintKey) && sprintTimer >= sprintCooldown && !isSprinting)
        {
            movement = movement.normalized * sprintSpeed;
            isSprinting = true;
            sprintTime = 0f;
            sprintTimer = 0f;
        }
        else if (isSprinting && sprintTime < sprintDuration)
        {
            // Si el jugador sigue sprinteando, aumentamos la velocidad de movimiento y el tiempo de sprint
            movement = movement.normalized * sprintSpeed;
            sprintTime += Time.fixedDeltaTime;
        }
        else
        {
            // Si el tiempo de sprint ha terminado o el jugador ha dejado de sprintear, volvemos a la velocidad normal de movimiento
            movement = movement.normalized * moveSpeed;
            isSprinting = false;
        }

        // Aplicamos la fuerza de movimiento al rigidbody
        rb.AddForce(movement * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
