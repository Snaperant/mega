using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebamovimiento : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] int saltosMax;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadio;
    [SerializeField] GameObject balamx;
    Rigidbody2D mybody;
    Animator myAnimator;
    Vector2 move;
    int saltosRestantes;
    
    
    
    

    //disparo
    float sgtDisparo;//cooldown
    float acDisparo;//nextfire
    float cooldownAniDisparo = 0.4f;//cooldownoffset tiempo para bajar animacion
    float cooldown=0.1f;//nextfireoffset tiempo volver a disparar

    //saltodoble
    bool dobleSalto = false;
    bool verficadorAire = false;
    //dash
    float inicioTmpDash;
    float tiempoDash = 1f;
    //caer
    bool caer;
    //ensuelo
    bool ensuelo;
    //voltear
    bool mirandoDerecha = true;







    void Start()
    {
        mybody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        saltosRestantes = saltosMax;
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        Saltar();
        Dash();
        Caer();
        enSuelo();
        Disparar();
        

    }
    private void FixedUpdate()
    {
        float velocidadH = move.normalized.x * speed;
        mybody.velocity = new Vector2(velocidadH, mybody.velocity.y);
    }
    void Movimiento()
    {
        float movH = Input.GetAxisRaw("Horizontal");
        move = new Vector2(movH, 0);

        if (movH != 0)
        {
            if (movH < 0 && mirandoDerecha == true)
            {
                Voltear();
                
            }
            else if (movH > 0 && mirandoDerecha == false)
            {
                Voltear();
            }
            myAnimator.SetBool("Run", true);
           
        }
        else
            myAnimator.SetBool("Run", false);
    }
    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1;
        transform.localScale = new Vector2(localScaleX, transform.localScale.y);
    }
    void enSuelo()
    {
        ensuelo = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadio, groundLayer);
        
    }
    void Saltar()
    {
        if (ensuelo)
        {
            saltosRestantes = saltosMax;
            
        }
        if (Input.GetKeyDown(KeyCode.Space) && ensuelo == true)
        {
            mybody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            myAnimator.SetTrigger("Jump");
            verficadorAire = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.Space) && ensuelo == false && saltosRestantes > 0 &&verficadorAire==true)
        {
            saltosRestantes = saltosRestantes - 1;
            verficadorAire = false;
            mybody.velocity = new Vector2(mybody.velocity.x, 0);
            mybody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            myAnimator.SetTrigger("Jump");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && ensuelo == true && saltosRestantes > 0&& verficadorAire)
        {

        }
        /*if(ensuelo)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                mybody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                myAnimator.SetTrigger("Jump");
                dobleSalto = true;
                verficadorAire = true;
                
            }
            
        }
        if (dobleSalto == true && verficadorAire == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mybody.velocity = new Vector2(mybody.velocity.x, 0);
                mybody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                myAnimator.SetTrigger("Jump");
                dobleSalto = false;
                verficadorAire = false;
            }
        }
        if (ensuelo && dobleSalto == true && verficadorAire == true)
        {
            dobleSalto = false;
            verficadorAire = false;
        }*/
    }
    
    void Caer()
    {
        if (mybody.velocity.y == 0 && ensuelo==true)
        {
            caer = false;
        }
        else if(mybody.velocity.y == 0 && ensuelo == false)
        {
            caer = true;
        }
        else if (mybody.velocity.y < 0 && ensuelo == true)
        {
            caer = false;
        }
        else if (mybody.velocity.y < 0 && ensuelo == false)
        {
            caer = true;
        }
        else if(mybody.velocity.y > 0 && ensuelo == true)
        {
            caer = false;
        }
        else if (mybody.velocity.y > 0 && ensuelo == false)
        {
            caer = true;
        }
        if (caer == true)
        {
            myAnimator.SetBool("Fall", true);
        }
        else if (caer == false)
        {
            myAnimator.SetBool("Fall", false);
        }


    }
    void Disparar()
    {
        if (Input.GetKeyDown(KeyCode.Z)&&Time.time>acDisparo)
        {
            
            GameObject mxbala =Instantiate(balamx, transform.position, transform.rotation);
            bool direccionDisparo = transform.localScale.x != 1 ? false : true;
            mxbala.GetComponent<bala_mx>().Disparo(direccionDisparo,speed*2);

            myAnimator.SetLayerWeight(1, 1);
            sgtDisparo = Time.time + cooldownAniDisparo;
            acDisparo = Time.time + cooldown;
        }
        else
        {
            if(Time.time>sgtDisparo)
            myAnimator.SetLayerWeight(1, 0);
        }    
    }
    void Dash()
    {
        float direccion = 1f;
            if (Input.GetKeyDown(KeyCode.X))
            {
                inicioTmpDash = Time.time;
                myAnimator.SetBool("Dash", true);
                //isDashing = true;
            }

            if (Input.GetKey(KeyCode.X))
            {
                if (Time.time <= inicioTmpDash + tiempoDash)
                {
                    mybody.velocity = new Vector2(speed * 2f * direccion,0);
                    myAnimator.SetBool("Dash", true);
                }
                else
                {
                    myAnimator.SetBool("Dash", false);
                    //isDashing = false;
                }
            }
            else
            {
                myAnimator.SetBool("Dash", false);
                //isDashing = false;
            }
        
    }
    
}
