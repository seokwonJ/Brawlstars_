using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RangeImage : MonoBehaviour
{
    public GameObject nearestThing;
    public LayerMask block;
    public bool ultran;
    
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
        gameObject.SetActive(true);
        print("1");
        // 현재 마우스 좌표에서 Ray 를 쏘자.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 닿은 지점이 있으면
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 pos = hit.point;
            pos.y = transform.position.y;
            Vector3 dir = pos - transform.position;
            //if(dir.magnitude > 10)
            //{
            //    dir.Normalize();
            //    dir = dir * 10;

            //}    
            #region
            //Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 25f);

            //if (hitEnemies.Length > 0)
            //{
            //    // 가장 가까운 적을 찾기
            //    Collider nearestEnemy = hitEnemies[0];
            //    float closestDistance = Vector3.Distance(transform.position, hitEnemies[0].transform.position);

            //    foreach (Collider enemy in hitEnemies)
            //    {
            //        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            //        if (distanceToEnemy < closestDistance)
            //        {
            //            closestDistance = distanceToEnemy;
            //            nearestEnemy = enemy;
            //        }

            //    }

            //    // Raycast로 적이 시야에 있는지 확인
            //    RaycastHit hit1;
            //    if (Physics.Raycast(transform.position, (nearestEnemy.transform.position - transform.position).normalized, out hit1, 28))
            //    {
            //        // print(hit.transform.gameObject.name + " / nearestEnemy : " + nearestEnemy.transform.gameObject.name); ;

            //        if (hit1.collider.gameObject == nearestEnemy.gameObject)
            //        {
            //            // 적에 데미지를 주는 함수 호출 (적 오브젝트에 구현되어 있어야 함)
            //            //Debug.Log("Enemy hit: " + nearestEnemy.name);
            //            //nearestEnemy.GetComponent<enemy>().TakeDamage(attackDamage);
            //            //GetComponent<PlayerFire>();

            //            nearestThing = nearestEnemy.gameObject;
            //        }
            //        else
            //        {
            //            nearestThing = null;
            //        }
            //    }
            //    else
            //    {
            //        nearestThing = null;
            //    }


            //Ray nearestRay 
            //if (Physics.Raycast(ray, out nearestRay, 20f))
            //{
            #endregion

            Ray nearestThing = new Ray(gameObject.transform.position , dir);
            RaycastHit hitinfo = new RaycastHit();
            if(Physics.Raycast(nearestThing, out hitinfo, 10f, block))
            {
                Vector3 pos1 = hitinfo.point;
                pos1.y = transform.position.y;
                Vector3 dir1 = pos1 - transform.position;
                transform.forward = dir1;
                float zscaleValue = Mathf.Clamp(dir1.magnitude, 0f, 10f);

                transform.localScale = new Vector3(1, 1, zscaleValue);
            }
            //transform.forward = dir;
            else
            {
                transform.forward = dir;
                float zscaleValue = Mathf.Clamp(dir.magnitude, 0f, 10f);
                transform.localScale = new Vector3(1, 1, zscaleValue);
            }
            
            
        }
            
       // }



    }
    //if(Physics.Raycast(ray1, outhit, LayerMask.NameToLayer("block")))
      

}

