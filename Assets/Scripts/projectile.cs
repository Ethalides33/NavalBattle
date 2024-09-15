using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f; // Time in seconds before the sphere is destroyed automatically

    void Start()
    {
        // Destroy the sphere after lifeTime seconds
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the sphere immediately on collision
        Destroy(gameObject);
    }
}