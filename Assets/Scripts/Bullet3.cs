using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;



public class Bullet3 : MonoBehaviour
{

  
    public GameObject player;
    public GameObject player2;
    public JHPSystem ug;
    public GameObject Attack;
    GameObject attack;
    Vector3 dir;

    public float bulletSpeed = 2.5f;
    public float maxDistance = 14f;
    public float scaleRate = 2.8f;


    void Start()
    {
        attack = Instantiate(Attack);
        attack.transform.position = transform.position;
        attack.transform.up = transform.up;
        attack.transform.forward = transform.forward;
        player2 = GameObject.Find("Player2");
        Destroy(gameObject, 2.5f);
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

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ug = other.GetComponent<JHPSystem>();
            //ug.currHP = ug.currHP + 20000;
            ug.UpdateHP(5000);

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



}
