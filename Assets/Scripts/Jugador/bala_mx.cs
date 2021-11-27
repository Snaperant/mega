using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala_mx : MonoBehaviour
{
    float speed;
    Rigidbody2D mybody;
    Animator myanimator;
    // Start is called before the first frame update
    void Start()
    {
        myanimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
         
    }
    public void Disparo(bool direccion,float velBala)
    {
        speed = velBala;
        mybody = GetComponent<Rigidbody2D>();
        if (direccion)
        {
            mybody.velocity = new Vector2(speed, 0);
        }
        else
            mybody.velocity = new Vector2(-speed, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        myanimator.SetTrigger("Hit");
        mybody.velocity = Vector2.zero;
        
    }
    void Destuir()
    {
        Destroy(gameObject);
    }
}
