using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class CannonAiming : MonoBehaviour
{
    private Transform trans;
    [SerializeField] private float rotationSpeed = 10f;
    
    public float max_angle = 20f;
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
            Vector3 direction = new(targetPoint.x - trans.position.x, 0, targetPoint.z - trans.position.z);

            // Calculate the new rotation for the cannon to face the target point
            Quaternion targetRotation = Quaternion.LookRotation(direction);
// Get the target Y rotation angle and clamp it
            float targetYRotation = targetRotation.eulerAngles.y;
            float clampedYRotation = Mathf.Clamp(targetYRotation, -max_angle, max_angle);

            // Apply the clamped rotation to the cannon
            Quaternion clampedRotation = Quaternion.Euler(0, clampedYRotation, 0);
            trans.rotation = Quaternion.Slerp(trans.rotation, clampedRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
