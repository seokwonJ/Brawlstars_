using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeImage2 : MonoBehaviour
{
    public PlayerMove2 pm2;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void range()
    {
        
        //print("1");
        
        if (pm2.AttackSpot() == Vector3.zero)
        {
            Vector3 pm2 = player.transform.position;
            Vector3 dir1 = pm2 - gameObject.transform.position;

            //float zscaleValue1 = Mathf.Clamp(dir1.magnitude, 0f, 10f);
            //transform.localScale = new Vector3(1, 1, zscaleValue1);
            transform.rotation = Quaternion.LookRotation(dir1, Vector3.up);
        }
        else
        {
            Vector3 dir = pm2.transform.TransformDirection( pm2.AttackSpot());

            //print(dir);

           // float zscaleValue = Mathf.Clamp(dir.magnitude, 0f, 10f);
            //transform.localScale = new Vector3(1, 1, 10);
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
            //if (Physics.Raycast(nearestThing, out hitinfo, 10f, block))
            //{
            //    Vector3 pos1 = hitinfo.point;
            //    pos1.y = transform.position.y;
            //    Vector3 dir1 = pos1 - transform.position;
            //    transform.forward = dir1;
            //    float zscaleValue = Mathf.Clamp(dir1.magnitude, 0f, 10f);

            //    transform.localScale = new Vector3(1, 1, zscaleValue);
            //}
            ////transform.forward = dir;
            //else
            //{
            //    transform.forward = dir;
            //    float zscaleValue = Mathf.Clamp(dir.magnitude, 0f, 10f);
            //    transform.localScale = new Vector3(1, 1, zscaleValue);
            //}


        

        // }



    }
}
