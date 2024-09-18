using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class CannonAiming : MonoBehaviour
{
    private Transform trans;
    [SerializeField] private float rotationSpeed = 10f;
    public float debug_xangle = 0f;
    public float debug_yangle = 0f;
    public float debug_zangle = 0f;

    public Transform front;
    public float max_angle = 45f;
    void Start()
    {
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        AimCannonAtCursor();
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
            // Calculate the new rotation for the cannon to face the target point
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Transform boat_transform = trans.parent.gameObject.GetComponent<Transform>(); // C# version

            Quaternion localRotation = Quaternion.Inverse(boat_transform.rotation) * targetRotation;

            // Get the target Y rotation angle and clamp it
            float localYRotation = localRotation.eulerAngles.y +90;
            if (localYRotation > 180f) localYRotation -= 360f; // Convert to a range of -180 to 180 degrees

            float clampedYRotation = Mathf.Clamp(localYRotation, -max_angle, max_angle);
            // Mathf.Clamp(targetYRotation, -max_angle, max_angle);
            // Debug.Log(clampedYRotation);
            // Apply the clamped rotation to the cannon
            Quaternion clampedRotation = Quaternion.Euler(0, clampedYRotation, 90);

            trans.rotation = boat_transform.rotation * clampedRotation; // Combine boat's rotation with the clamped local rotation
            // trans.rotation = Quaternion.Slerp(trans.rotation, clampedRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
