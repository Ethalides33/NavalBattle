using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{ 
    [SerializeField] private float forwardSpeed = 20f;  // Speed of the boat
    [SerializeField] private float speed_lim = 30f;
    [SerializeField] private float turnSpeed = 50f;     // Turning speed of the boat
    public Transform frontShootPoint;

    public Transform cannon;
    public GameObject spherePrefab;   // Prefab of the sphere to shoot
    public float shootingForce = 750; // Force applied to the spheres when shooting
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Move forward
        float moveInput = Input.GetAxis("Vertical");
        Vector3 forwardMovement = -forwardSpeed * moveInput * transform.right;
        if(rb.velocity.sqrMagnitude<speed_lim){
            rb.AddForce(forwardMovement);
        }

        // Steer
        float turnInput = Input.GetAxis("Horizontal");
        float turn = turnInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void Update(){
    }

    void ShootSpheres(string side){
        if(side=="forward"){
            ShootSphere(frontShootPoint);
        }
    }


    void ShootSphere(Transform shootPoint)
    {
        // Instantiate the sphere at the shoot point's position and rotation
        GameObject sphere = Instantiate(spherePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody sphereRb = sphere.GetComponent<Rigidbody>();
        Vector3 shootingDirection = Quaternion.Euler(-20, -45, 0) * cannon.transform.forward;
        shootingDirection = new(shootingDirection.x,0.5f,shootingDirection.z);
        Debug.Log(shootingDirection);
        // Vector3 dir = new(-shootPoint.forward.x,0.75f,-shootPoint.forward.z);        // Apply force to the sphere in the shoot point's forward direction
        sphereRb.AddForce(shootingDirection*shootingForce);
    }

}
