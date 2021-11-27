using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask capaSuelo;
    Rigidbody2D mybody;
    Animator myAnimatior;
    private BoxCollider2D boxCollider;
    Vector2 _movimiento;

    void Start()
    {
        mybody = GetComponent<Rigidbody2D>();
        myAnimatior = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        Salto();
    }
    /*bool EnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D boxCollider (BoxCollider.bounds.center, New Vector2(BoxCollider.bounds.size.x, BoxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit.collider != null;
    }*/
    void Movimiento()
    {
        float movH = Input.GetAxisRaw("Horizontal")*speed;
        if (movH != 0)
        {
            if (movH > 0)
            {
                mybody.velocity = new Vector2( movH,0);
                transform.localScale = new Vector2(1, 1);
                
            }
            else if (movH < 0)
            {
                mybody.velocity = new Vector2(movH,0);
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }
            myAnimatior.SetBool("Run", true);
        }
        else
            myAnimatior.SetBool("Run", false);
    }
    void Salto()
    {
        if (Input.GetKeyDown(KeyCode.Space)/*&& EnSuelo()*/)
        {
            mybody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    
}
