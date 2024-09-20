using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellScript2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeTime = 5f;
    void Start()
    {
        Destroy(gameObject, lifeTime);

    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the sphere immediately on collision
        Destroy(gameObject);
    }
}
