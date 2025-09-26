using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public GameObject nearestThing;
    public LayerMask block;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void range1()
    {
        // 현재 마우스 좌표에서 Ray 를 쏘자.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 닿은 지점이 있으면
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 pos = hit.point;
            pos.y = transform.position.y;
            Vector3 dir = pos - transform.position;
   
            Ray nearestThing = new Ray(gameObject.transform.position, dir);
            RaycastHit hitinfo = new RaycastHit();
            if (Physics.Raycast(nearestThing, out hitinfo, 10f, block))
            {
                Vector3 pos1 = hitinfo.point;
                pos1.y = transform.position.y;
                Vector3 dir1 = pos1 - transform.position;
                transform.forward = dir1;
                float zscaleValue = Mathf.Clamp(dir1.magnitude, 2f, 2f);

                transform.localScale = new Vector3(1, 1, zscaleValue);
            }
            //transform.forward = dir;
            else
            {
                transform.forward = dir;
                float zscaleValue = Mathf.Clamp(dir.magnitude, 2f, 2f);
                transform.localScale = new Vector3(1, 1, zscaleValue);
            }


        }

        // }



    }
}
