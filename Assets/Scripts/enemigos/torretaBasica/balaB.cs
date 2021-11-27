using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaB : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D myRigidbody;
    BoxCollider2D BoxCollider2D;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime * -1, 0, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);

        if (layer == "plataformas")
        {
            
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);
        if (layer == "plataformas")
        {
            Destroy(gameObject);
        }
        if(layer == "jugador")
        {
            Destroy(gameObject);
        }


    }
}
    
