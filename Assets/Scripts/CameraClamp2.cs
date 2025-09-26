using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraClamp2 : MonoBehaviour
{
    public Transform targetPlayer;

    public float xMin = -12f;
    public float xMax = 12f;
    public float zMin = -5f;
    public float zMax = 18f;
    Transform ca;
    Vector3 campo;
    //Vector3 dir111;
   // public Transform groundLB;
    //public Transform groundRT;
    public GameObject player2;
    Camera myCam;
    
    public Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        offset = transform.position - targetPlayer.position;

        myCam = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        campo = transform.position;

    }
    // Update is called once per frame
    void Update()
    {

        //Vector3 LB = myCam.WorldToViewportPoint(groundLB.position);
        //Vector3 RT = myCam.WorldToViewportPoint(groundRT.position);
        //Vector3 dir = gameObject.transform.position - player.transform.position;

        //if (LB.x > 0)
        //{
        //    transform.position = campo;
        //    //if()
        //}
        //if (LB.y > 0)
        //{
        //    transform.position = campo;
        //}
        //if (RT.x < 1)
        //{
        //    transform.position = campo;
        //}
        //if (RT.y < 1)
        //{
        //    transform.position = campo;
        //}
        ////카메라의 포지션을 파악하고 다른 변수에 넣어주고
        //campo = gameObject.transform.position;
        //print(campo);
        ////그 카메라의 포지션의 .x .z 의 값을 확인하고
        ////넘어가면 그 값을 넘어가지 않게 하고

        ////해서 넘어가지 않은 값을 다시 카메라 포지션에 넣어준다
        //else
        //{
        //    gameObject.transform.position = campo;
        //}


        Vector3 pos = targetPlayer.transform.position + offset;


        pos.x = Mathf.Clamp(pos.x, xMin, xMax);

        pos.z = Mathf.Clamp(pos.z, zMin, zMax);

        //if (pos.x < xMin)
        //{
        //    pos.x = xMin;
        //}
        //else if (pos.x > xMax)
        //{
        //    pos.x = xMax;
        //}

        //if (pos.z < zMin)
        //{
        //    pos.z = zMin;
        //}
        //else if (pos.z > zMax)
        //{
        //    pos.z = zMax;
        //}

        transform.position = pos;

    }
}
