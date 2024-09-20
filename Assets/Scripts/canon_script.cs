using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class CannonAiming : MonoBehaviour
{
    private Transform trans;
    public Transform front;
    public Transform boatTransform;

    [SerializeField] private float rotationSpeed = 10f;
    public float shootingForce = 750; // Force applied to the spheres when shooting
    public float max_angle = 45f;

    public GameObject spherePrefab;   // Prefab of the sphere to shoot
    public GameObject explosionPrefab;


    void Start()
    {
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        AimCannonAtCursor();

        if (Input.GetMouseButtonDown(0))
        {
            ShootSpheres("forward");
        }
    }

    void AimCannonAtCursor()
    {
        // Get the cursor's screen position
        Vector3 cursorScreenPosition = Input.mousePosition;

        // Convert the screen position to a world point
        Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
        
        // Calculate the intersection point of the ray with the water surface plane
        Plane waterPlane = new Plane(Vector3.up, new Vector3(0, trans.position.y, 0));
        if (waterPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            // Determine the direction from the cannon to the target point
            Vector3 direction = new(targetPoint.x - front.position.x, 0, targetPoint.z - front.position.z);
            
            Debug.DrawRay(front.position,direction, Color.green);
            Vector3 localDirection = boatTransform.InverseTransformDirection(direction);

            // Calculate the target local rotation (look rotation in local space)
            Quaternion targetRotation = Quaternion.LookRotation(localDirection);

            // Convert to Euler angles to clamp the Y-axis rotation (local to the boat)
            Vector3 targetEulerAngles = targetRotation.eulerAngles;

            // Adjust the angle for clamping (convert from 0-360 to -180 to 180)
            if (targetEulerAngles.y > 180)
                targetEulerAngles.y -= 360;

            // Clamp the Y rotation between min and max in local space
            targetEulerAngles.y = Mathf.Clamp(targetEulerAngles.y, -max_angle, +max_angle);

            // Smoothly rotate towards the target local rotation
            Quaternion clampedLocalRotation = Quaternion.Euler(-90, targetEulerAngles.y, 0);
            Quaternion finalRotation = boatTransform.rotation * clampedLocalRotation;

            // Apply the rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void ShootSpheres(string side){
        if(side=="forward"){
            ShootSphere(front);
        }
    }


    void ShootSphere(Transform shootPoint)
    {
        // Instantiate the sphere at the shoot point's position and rotation
        Quaternion shell_angle = Quaternion.Euler(front.rotation.eulerAngles.x,front.rotation.eulerAngles.y-90,front.rotation.eulerAngles.z+90);
        Debug.Log(front.rotation);
        GameObject sphere = Instantiate(spherePrefab, shootPoint.position, shell_angle);
        Rigidbody sphereRb = sphere.GetComponent<Rigidbody>();
        Quaternion correction = Quaternion.Euler(0,-90,0);
        Vector3 shootingDirection = correction*transform.right;
        shootingDirection = new(shootingDirection.x,0.05f,shootingDirection.z);
        // Vector3 dir = new(-shootPoint.forward.x,0.75f,-shootPoint.forward.z);        // Apply force to the sphere in the shoot point's forward direction
        sphereRb.AddForce(shootingDirection*shootingForce);

        GameObject explosion = Instantiate(explosionPrefab,shootPoint.position,shootPoint.rotation);
        explosion.transform.localScale *= 0.2f; // Adjust the multiplier to scale as needed (2x size in this case)

        Destroy(explosion, 2f); // Adjust the time based on the explosion duration
    }

}
