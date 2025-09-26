using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JplayerMove : MonoBehaviour
{

    float gravity = -20f;
    float yVelocity = 0;
 
    // 이동 속도 변수
    public float moveSpeed = 7f;
    CharacterController cc;
    int hp;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // wasd 키를 누르면 입력하면 캐릭터를 그 방향으로 이동시키고 시팓.

        // 1. 사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. 이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 3. 이동 속도에 맞춰 이동한다.
        // p = p0 + vt

        //Vector3 dir = (player.position - transform.position).normalized;

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        // 캐릭터 콘트롤러를 이용해 이동하기
        cc.Move(dir * moveSpeed * Time.deltaTime);

        //if(dir != Vector3.zero)
        //{
        //    Vector3 lookDirection = new Vector3(dir.x, 0, dir.z);
        //    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720 * Time.deltaTime);
        //}
    }

    public void DamageAction(int damage)
    {
        // 에너미의 공격력만큼 플레이어의 체력을 깎는다.
        hp -= damage;
    }
}
