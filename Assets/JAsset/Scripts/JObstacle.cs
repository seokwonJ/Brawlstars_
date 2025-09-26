//using System.Collections;
//using System.Collections.Generic;
//using Unity.AI.Navigation;
//using UnityEngine;

//public class JObstacle : MonoBehaviour
//{
//    public NavMeshSurface navMeshSurface;

//    // Start is called before the first frame update
//    void Start()
//    {
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    void OnTriggerStay(Collider collision)
//    {

//        if (collision.CompareTag("Skill"))
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void OnDestroy()
//    {
//        navMeshSurface.BuildNavMesh();
//    }


//}

using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;


public class JObstacle : MonoBehaviour
{

    public GameObject exploFactory;
    

    public NavMeshSurface navMeshSurface;
    JCameraShake cameraShake;
    // Start is called before the first frame update
    void Start()
    {
        cameraShake = Camera.main.GetComponent<JCameraShake>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider collision)
    {

        if (collision.CompareTag("Skill"))
        {
            // 폭발효과를 생성하자.
            GameObject explo = Instantiate(exploFactory);
            // 생성된 폭발효과를 나의 위치에 놓자.
            explo.transform.position = transform.position;

            navMeshSurface.BuildNavMesh();
            cameraShake.StartShake(0.3f, 0.05f, true, Vector3.back);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("bullet") || collision.gameObject.layer == LayerMask.NameToLayer("Ebullet"))
        {
            GetComponent<JCameraShake>().StartShake(0.3f, 0.08f, false, collision.transform.position);

        }
    }

    private void OnDestroy()
    {
 
       
    }


}
