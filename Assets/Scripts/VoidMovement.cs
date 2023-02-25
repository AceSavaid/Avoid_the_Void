using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidMovement : MonoBehaviour
{
    [SerializeField] Transform endpoint;
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, endpoint.position, speed * Time.deltaTime);
    }
}
