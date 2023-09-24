using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 2.0f;

    [SerializeField]
    private float speed = 5.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
}
