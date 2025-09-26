using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JShellyBullet : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject player2;
    public UltGauge ug;
    Vector3 dir;

    public float bulletSpeed = 3;
    public float maxDistance = 1f;

    //public float creatTime = 4f;
    //public float currTime = 0;

    // Start is called before the first frame update

    //private void Awake()
    //{
    //    player2 = GameObject.Find("Player2");
    //}

    //void OnEnable()
    //{
    //    Collider playertomove = player2.GetComponent<player2move>().short_enemy;

    //    if (playertomove == null)
    //    {
    //        return;
    //    }

    //    dir = playertomove.transform.position - player2.transform.position;
    //    dir.Normalize();
    //}
    void Start()
    {
        player = GameObject.Find("Player3");
    }
    // Update is called once per frame

    public float currTime = 0;
    public float createTime = 0.2f;
    void Update()
    {

        transform.position = transform.position + transform.forward * bulletSpeed * Time.deltaTime;
        if (Vector3.Distance(player.transform.position, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
        // if ( currTime > 2)
        // dir = player.GetComponent<PlayerController>().nearestEnemyObj.transform.position - transform.position;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Destroy(collision.gameObject);
    //        Destroy(gameObject);

    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {

        //if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //{
        //    ug = player.GetComponent<UltGauge>();
        //    ug.GainGauge();
        //    //GainGauge();
        //    //UltGauge ultgauage = gameObject.GetComponent<UltGauge>();
        //    //if(ultgauage != null)
        //    //{
        //    //    ultgauage.GainGauge();
        //    //}
        //}

        Destroy(gameObject);


    }


    public void OnlisionEnter(Collision collision)
    {

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("block")))
        {
            //  print("1");

        }
    }

}
