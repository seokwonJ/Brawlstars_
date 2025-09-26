using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class player2move : MonoBehaviour
{

    public float moveSpeed = 6;
    public float lerpSpeed = 1f;
    Vector3 finalDir;

    public float ultimateSpeed = 4f;
    public bool isUsingUltimate = false;
    private float ultimateStartTime;


    public float radius = 0f;
    public LayerMask Enemy;
    public Collider[] colliders;
    public Collider short_enemy;
    public int hp = 15;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir = dirH + dirV;

        // 이동속도 스무스하게
        if (dir.magnitude > 1)
        {
            dir = dir.normalized;
        }

        finalDir = Vector3.Lerp(finalDir, dir, lerpSpeed * Time.deltaTime);
        transform.position += finalDir * moveSpeed * Time.deltaTime;



        colliders = Physics.OverlapSphere(transform.position, radius, Enemy);

        if (colliders.Length > 0)
        {
            //왜냐? 처음것이 무조건 최소값인줄 알고 우리는 행동한다
            float short_distance = Vector3.Distance(transform.position, colliders[0].transform.position);
            short_enemy = colliders[0];
            //비교를 하면서
            foreach (Collider col in colliders)
            {
                float short_distance2 = Vector3.Distance(transform.position, col.transform.position);
                //거리값이 기존값보다 작으면
                if (short_distance > short_distance2)
                {
                    // 최소값을 갱신하고 
                    short_distance = short_distance2;
                    //그 콜라이더를 최단에너미에 넣어준다
                    short_enemy = col;
                }
            }

        }




    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    public void DamageAction(int damage)
    {
        hp -= damage;
    }
}
