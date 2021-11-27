using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class srciptAbeja : MonoBehaviour
{
    [SerializeField] int DisparosParaMorir;
    [SerializeField] AudioClip sfx_morir;

    Animator myAnimator;
    AIPath myAiPath;
    CircleCollider2D myCollider;
    bool Seguir = true;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CircleCollider2D>();
        myAiPath = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Seguir)
        {
            myAiPath.enabled = Physics2D.OverlapCircle(transform.position, 8f, LayerMask.GetMask("jugador"));//tamaño radio de detecion
        }
        

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, 8f);//tiene que ser igual a el numero que esta en el update 
    }
    public void DIE()
    {
        AudioSource.PlayClipAtPoint(sfx_morir, Camera.main.transform.position);
        myAiPath.enabled = false;
        Seguir = false;
        myCollider.enabled = false;
        myAnimator.SetTrigger("Destroy" );
        
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "balax")
        {
            ReducirVida();
            if(DisparosParaMorir<1)
            {
                DIE();
            }
            
        }
    }
    void ReducirVida()
    {
        DisparosParaMorir = DisparosParaMorir - 1;
    }


}


