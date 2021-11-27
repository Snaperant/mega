using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torretaB : MonoBehaviour
{
    [SerializeField] GameObject bala;
    [SerializeField] int DisparosParaMorir;
    [SerializeField] AudioClip sfx_destruccion;

    Animator myAnimator;
    BoxCollider2D myCollider;

    float sgtDisparo;
    float Cooldown = 1f;
    bool cargado= true;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectarJugador() && Time.time > sgtDisparo && cargado)
        {
            Disparar();
        }
    }
    private bool detectarJugador()
    {
        Vector3 origin = transform.position;
        Vector3 destination = Vector2.left;

        RaycastHit2D myRaycast = Physics2D.Raycast(origin, destination, 20f, LayerMask.GetMask("jugador"));
        Debug.DrawRay(origin, new Vector3(-20f, 0), Color.red);

        return (myRaycast.collider != null);
    }
    private void Disparar()
    {
        Instantiate(bala, transform.position + new Vector3(0, 0.2f), transform.rotation);
        sgtDisparo = Time.time + Cooldown;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "balax")
        {
            ReducirVida();
            if (DisparosParaMorir < 1)
            {
                DIE();
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "balax")
        {
            ReducirVida();
            if (DisparosParaMorir < 1)
            {
                DIE();
            }

        }
    }
    void ReducirVida()
    {
        DisparosParaMorir = DisparosParaMorir - 1;
    }
    public void DIE()
    {
        cargado = false;
        myAnimator.SetTrigger("Destroy");
        myCollider.enabled = false;
        AudioSource.PlayClipAtPoint(sfx_destruccion, Camera.main.transform.position);
        

    }
    
}
