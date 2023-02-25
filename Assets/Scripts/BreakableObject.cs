using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] AudioClip breakSound;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (breakSound)
            {
                AudioSource.PlayClipAtPoint(breakSound, gameObject.transform.position);
            }
            Destroy(gameObject);
        }
    }
}
