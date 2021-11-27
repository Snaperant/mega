using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ataqueTorreta : MonoBehaviour
{
    [SerializeField] float distanciaRaycast=0.8f;
    [SerializeField] GameObject balaB;
    Animator animator;
    float coolDownAtk = 1;
    float actualCooldownAtk;
    // Start is called before the first frame update
    void Start()
    {
        actualCooldownAtk = 0;
    }

    // Update is called once per frame
    void Update()
    {
        actualCooldownAtk -= Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.left, Color.red, distanciaRaycast);
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.localPosition,Vector2.left,distanciaRaycast);
        if(hit2D.collider!=null)
        {
            if(hit2D.collider.CompareTag("jugador"))
            {
                if (actualCooldownAtk<0)
                {
                    Invoke( "dispararBala",0.5f);
                    animator.Play("disparoB");
                    actualCooldownAtk = coolDownAtk;
                }
            }
        }
    }
    void dispararBala()
    {
        GameObject nuevaBala;
        nuevaBala = Instantiate(balaB,transform.position,transform.rotation);

    }
}
