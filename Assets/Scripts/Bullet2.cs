using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;



public class Bullet2 : MonoBehaviour
{
    
    public GameObject enemy;
    public GameObject player2;
    public UltGauge2 ug;
    public GameObject Attack;
    GameObject attack;
    Vector3 dir;
    
    public float bulletSpeed = 2;
    public float maxDistance = 9f;
    public float scaleRate = 1.1f;


    void Start()
    {
        player2 = GameObject.Find("Player2");
        attack = Instantiate(Attack);
        attack.transform.position = transform.position;
        attack.transform.up = transform.up;
        attack.transform.forward = transform.forward;
        Destroy(gameObject, 1.3f);
    }
    // Update is called once per frame

    public float currTime = 0;
    public float createTime = 0.2f;
    void Update()
    {

        transform.position = transform.position + transform.forward * bulletSpeed * Time.deltaTime;
        transform.localScale = transform.localScale + Vector3.right * scaleRate * Time.deltaTime;
        if (Vector3.Distance(player2.transform.position, transform.position) > maxDistance)
        {
        //    Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            ug = player2.GetComponent<UltGauge2>();
            ug.GainGauge();

        }

       // Destroy(gameObject);


    }


    public void OnlisionEnter(Collision collision)
    {

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("block")))
        {
            //  print("1");

        }
    }

    //private void OnDestroy()
    //{
    //    Destroy(attack);
    //}

}
