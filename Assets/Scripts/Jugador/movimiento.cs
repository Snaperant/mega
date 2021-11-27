using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class movimiento : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] GameObject balamx;
    [SerializeField] GameObject disparador;
    [SerializeField] BoxCollider2D misPies;
    [SerializeField] AudioClip sfx_bala;
    [SerializeField] AudioClip sfx_saltar;
    [SerializeField] AudioClip sfx_morir;
    [SerializeField] AudioClip sfx_dash;
    [SerializeField] AudioClip sfx_aterrizar;
    [SerializeField] GameObject gameoverui;
    public static bool juegoPausado;
    

    Animator myAnimator;
    Rigidbody2D mybody;


    //disparo
    float cooldownAniDisparo = 0.4f;
    float sgtDisparo;
    float cooldown = 0.1f;
    float acDisparo;

    //doblesalto
    bool dobleSalto = false;
   
    //dash
    float tiempoDash = 0.4f;
    float tiempoIDash;
   




    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        mybody = GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        Correr();
        Saltar();
        Caer();
        Disparar();
        Dash();
      
    }
    public void nuevoJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        juegoPausado = false;
    }
    void Dash()
    {
        
        bool ensuelo = misPies.IsTouchingLayers(LayerMask.GetMask("plataformas"));
        if (ensuelo)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                tiempoIDash = Time.time;
                myAnimator.SetBool("Dash", true);
            }
            if (Input.GetKey(KeyCode.X))
            {
               if(tiempoIDash+tiempoDash>=Time.time)
                {
                    mybody.velocity = new Vector2(speed * 1.5f * transform.localScale.x, 0);
                }
               else
                {
                    myAnimator.SetBool("Dash", false);
                }
                
            }
            else
                myAnimator.SetBool("Dash", false);
        }
    }
    void Correr()
    {
        float direccion = Input.GetAxisRaw("Horizontal");

        if (direccion != 0)
        {
            if (direccion < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
                transform.localScale = new Vector2(1, 1);

            myAnimator.SetBool("Run", true);
            mybody.velocity = new Vector2(direccion * speed, mybody.velocity.y);

        }
        else
        {
            mybody.velocity = new Vector2(0, mybody.velocity.y);
            myAnimator.SetBool("Run", false);
        }
    }

    void Saltar()
    {
        bool ensuelo = misPies.IsTouchingLayers(LayerMask.GetMask("plataformas"));
        if (ensuelo &&!myAnimator.GetBool("Start"))
        {
            
            myAnimator.SetBool("Fall", false);
            myAnimator.SetBool("Start", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dobleSalto = true;
                mybody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                myAnimator.SetTrigger("Jump");
                myAnimator.SetBool("Start", true);
                AudioSource.PlayClipAtPoint(sfx_saltar, Camera.main.transform.position);
            }
        }
            //Escenario de doble salto
        if (!ensuelo && dobleSalto==true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioSource.PlayClipAtPoint(sfx_saltar, Camera.main.transform.position);
                mybody.velocity = new Vector2(mybody.velocity.x, 0);
                mybody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                myAnimator.SetTrigger("Jump");
                dobleSalto = false;
            }
        }
        if (ensuelo && dobleSalto == true&&!myAnimator.GetBool("Start"))
        {
            dobleSalto = false;
        }

    }
    void Caer()
    {
        bool ensuelo = misPies.IsTouchingLayers(LayerMask.GetMask("plataformas"));
        if (mybody.velocity.y < -0.05f && !ensuelo)
        {
            myAnimator.SetBool("Fall", true);
        }
        if(ensuelo)
        {
            myAnimator.SetBool("Fall", false);
            
        }
    }
    void TerminarDeSaltar()
    {
        myAnimator.SetBool("Fall", true);
        myAnimator.SetBool("Start", false);
    }
    void Disparar()
    {
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= acDisparo)
        {
            GameObject mxbala = Instantiate(balamx, transform.position, transform.rotation); //disparador.transform.position, disparador.transform.rotation);
            bool direccionDisparo = transform.localScale.x == 1;
            (mxbala.GetComponent<bala_mx>()).Disparo(direccionDisparo, speed * 2);
            AudioSource.PlayClipAtPoint(sfx_bala, Camera.main.transform.position);

            myAnimator.SetLayerWeight(1, 1);

            sgtDisparo = Time.time + cooldownAniDisparo;
            acDisparo = Time.time + cooldown;
           
        }
        else
        {
            if (Time.time > sgtDisparo)
                myAnimator.SetLayerWeight(1, 0);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if ( tag == "enemigos")
        {
            myAnimator.SetBool("Death", true);
            StartCoroutine(DIE());
        }
    }
    IEnumerator DIE()
    { 
        mybody.isKinematic = true;
        mybody.velocity = Vector2.zero;
        AudioSource.PlayClipAtPoint(sfx_morir, Camera.main.transform.position);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
        gameoverui.SetActive(true);
        juegoPausado = true;
        Destroy(gameObject);

    }


}
