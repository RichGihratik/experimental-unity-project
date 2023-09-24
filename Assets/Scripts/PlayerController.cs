using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2f;

    [SerializeField]
    private Transform laser;

    [SerializeField]
    private float fireCooldown = 0.3f;

    [SerializeField]
    private float laserDistance = 0.2f;

    private float timeTilNextFire = 0f;

    private float currentSpeed = 0f;

    private Vector2 lastMovement = new Vector2();
    private Vector2 currentMoveVector = new Vector2();

    void Update()
    {
        Rotation();
        Movement();
        // Возможно, лучше это вынести в корутину
        ShootCooldownCalculation();
    }

    void ShootCooldownCalculation() 
    {
        timeTilNextFire = Mathf.Max(timeTilNextFire - Time.deltaTime, 0f);
    }

    void Rotation() 
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();   
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        float dx = this.transform.position.x - worldPos.x;
        float dy = this.transform.position.y - worldPos.y;

        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        this.transform.rotation = rot;
    }

    void Movement() 
    {
        if (currentMoveVector.magnitude > 0)
        {
            currentSpeed = playerSpeed;
            this.transform.Translate(currentMoveVector * Time.deltaTime * playerSpeed, Space.World);
            lastMovement = currentMoveVector;
        }
        else 
        {
            this.transform.Translate(lastMovement * Time.deltaTime * currentSpeed, Space.World);
            currentSpeed *= 0.9f;
        }
    }

    void OnMove(InputValue value) 
    {
        currentMoveVector = value.Get<Vector2>();
    }

    void ShootLaser()
    {
        float posX = this.transform.position.x + (Mathf.Cos ((transform.localEulerAngles.z - 90) * Mathf.Deg2Rad) * -laserDistance);
        float posY = this.transform.position.y + (Mathf.Sin ((transform.localEulerAngles.z - 90) * Mathf.Deg2Rad) * -laserDistance);
        Instantiate(laser, new Vector3(posX, posY, 0), this.transform.rotation);
    }

    void OnShoot() 
    {
        if(timeTilNextFire == 0)
        {
            timeTilNextFire = fireCooldown;
            ShootLaser();
        }
    }
}
