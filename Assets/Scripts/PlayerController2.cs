using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    public float attackRange = 10.0f;
    public float attackDamage = 10f;

    public GameObject bulletFactory;
    // 총구
    public GameObject firePos;
    public GameObject Player2;
    //public GameObject enemy;
    Vector3 distance;
    public int iniBulletCnt = 3;
    public List<GameObject> magazine = new List<GameObject>();
    // Start is called before the first frame update

    public GameObject nearestEnemyObj;
    public GameObject nearestEnemy;
    private bool isChargingAttack;
    private Vector3 attackDirection;
    private Vector3 UltDirection;
    public PlayerFire2 pf;
    public GameObject aimIndicatorAttack;
    public int maxSkillUses = 3;
    public GameObject point;

    public float skillCooldown = 1f; // 스킬 사용 간격 (쿨다운)
    private bool isSkillOnCooldown;
    public int skillUsesLeft;
    public float aimRadius = 5f; // 조준 반경
                                 //public GameObject aimIndicatorPrefab; // 조준 반경을 표시하는 프리팹
                                 //public Image aim;
                                 // private GameObject aimIndicatorInstance;
    public GameObject pang;
    public JHPSystem hpup;
    float hpp;
    void Start()
    {
        skillUsesLeft = maxSkillUses;
        isSkillOnCooldown = false;
        hpp = hpup.currHP;
        // 조준 반경 표시를 위한 인스턴스 생성
        //if (aimIndicatorPrefab != null)
        //{
        //    aimIndicatorInstance = Instantiate(aimIndicatorPrefab);
        //    aimIndicatorInstance.SetActive(false);
        //}
        //AimIndicatorAttack = GameObject.Find("AimIndicatorAttack");
    }
    public LayerMask Enemy;
    public LayerMask obstructionLayer;
    //JHPSystem hpup = GetComponent<JHPSystem>();
    // hp 자동 증가 시스템 활성 여부
    public bool hpupdown = true;
    public float curTime = 0;
    public bool isAttacking = false;
    public RangeAttack ra;
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlaying == false) return;
        Attack();


        if (hpup.currHP < hpp)
        {
            hpp = hpup.currHP;
            SetHpUpDown(false);
        }
        // 만약에 hp 자동 증가 시스템이 비활성화라면
        else if (hpupdown == false)
        {
            // 시간을 증가시키고
            curTime += Time.deltaTime;
            //print(curTime);
            // 2초가 지났으면
            if (curTime > 2.5)
            // hp 자동 증가 시스템을 활성화
            {
                SetHpUpDown(true);
            }
        }
        else if (hpupdown == true)
        {
            // 시간을 증가시키고
            curTime += Time.deltaTime;
            //print(curTime);
            // 2초가 지났으면
            if (curTime > 1 && hpup.currHP != hpup.maxHP)
            {
                // HP 를 증가시키고 싶다.
                hpup.UpdateHP(hpup.maxHP / 5);
                hpp = hpup.currHP;
                curTime = 0;
            }
        }

    }

    //내가 마우스버튼을 누르고 있다
    // - 시간을 측정해야 한다
    //0.2초 이상 누르고 있다가 때면 releaseAttack
    //그 이하면 playerFire의 fastattack으로 한다
    public float createTime = 1.2f;
    public float currTime = 0;


    public void Attack()
    {
        if (Input.GetButton("Fire44"))
        {

            currTime += Time.deltaTime;

            if (currTime > createTime)
            {
                //ShowAimIndicator(true);
                //RotateToMouseCursor();
                aimIndicatorAttack.SetActive(true);
                ra = aimIndicatorAttack.GetComponent<RangeAttack>();
                ra.range1();
            }
        }
        if (Input.GetButtonUp("Fire44") && currTime > createTime)
        {
            aimIndicatorAttack.SetActive(false);
            //isAttacking = true;
            ReleaseAttack();
            isAttacking = false;
            currTime = 0;
            SetHpUpDown(false);

        }
        else if (Input.GetButtonUp("Fire44") && currTime <= createTime)
        {
            aimIndicatorAttack.SetActive(false);
            //isAttacking = true;
            FastAttack();
            isAttacking = false;
            currTime = 0;
            SetHpUpDown(false);
        }
        else
        {
            isAttacking = false;
        }

        // OverlapSphere를 사용하여 공격 범위 내의 모든 적 감지
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, Enemy);

        if (hitEnemies.Length > 0)
        {
            // 가장 가까운 적을 찾기
            Collider nearestEnemy = hitEnemies[0];
            float closestDistance = Vector3.Distance(transform.position, hitEnemies[0].transform.position);

            foreach (Collider enemy in hitEnemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }

            }


            // Raycast로 적이 시야에 있는지 확인
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (nearestEnemy.transform.position - transform.position).normalized, out hit, attackRange))
            {
                // print(hit.transform.gameObject.name + " / nearestEnemy : " + nearestEnemy.transform.gameObject.name); ;

                if (hit.collider.gameObject == nearestEnemy.gameObject)
                {
                    // 적에 데미지를 주는 함수 호출 (적 오브젝트에 구현되어 있어야 함)
                    //Debug.Log("Enemy hit: " + nearestEnemy.name);
                    //nearestEnemy.GetComponent<enemy>().TakeDamage(attackDamage);
                    //GetComponent<PlayerFire>();

                    nearestEnemyObj = nearestEnemy.gameObject;
                }
                else
                {
                    nearestEnemyObj = null;
                }
            }
            else
            {
                nearestEnemyObj = null;
            }

        }
        else
        {
            nearestEnemyObj = null;

        }
        //Q누르면 스킬
        //if (Input.GetKeyDown(KeyCode.Q) && !isSkillOnCooldown && skillUsesLeft > 0)
        //{
        //    StartCoroutine(UseSkill());
        //}
    }

    public IEnumerator UseSkill()
    {
        StunEnemies();
        skillUsesLeft--;
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_GAGET);
        isSkillOnCooldown = true;

        yield return new WaitForSeconds(skillCooldown);

        isSkillOnCooldown = false;
    }

    public void SetHpUpDown(bool updown)
    {
        hpupdown = updown;
        curTime = 0;
    }

    void StartChargingAttack()
    {
        isChargingAttack = true;
    }

    public Vector3 UpdateAttackDirection()
    {
        // 마우스 위치를 월드 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // 공격 방향 설정
            //print(hit.point);
            hit.point = hit.point + Vector3.up * -0.8f;
            attackDirection = (hit.point - transform.position).normalized;

            //            point.transform.position = hit.point;

        }
        else
        {
            attackDirection = transform.forward;
        }

        attackDirection.y = 0f;

        return attackDirection;

    }

    public Vector3 UpdateAttackPosition()
    {
        // 마우스 위치를 월드 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 point = Vector3.zero;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // 공격 방향 설정
            attackDirection = (hit.point - transform.position);

            //            point.transform.position = hit.point;

            attackDirection.y = 0;
            point = hit.point;
        }
        else
        {
            attackDirection = transform.forward;
            attackDirection.y = 0;
        }
        return point;
    }

    void ReleaseAttack()
    {
        isAttacking = true;
        //pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position), 9f * Time.deltaTime);
        //pang.transform.forward = (nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position);
        pang.transform.forward = UpdateAttackDirection();

        pf = gameObject.GetComponent<PlayerFire2>();

        pf.Attack();


    }
    void FastAttack()
    {
        isAttacking = true;
        //pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(UpdateAttackDirection()), 9f * Time.deltaTime);

        //pang.transform.forward = UpdateAttackDirection();
        pf = gameObject.GetComponent<PlayerFire2>();

        pf.FastAttack();


    }

    void OnDrawGizmosSelected()
    {
        // 공격 범위를 시각적으로 표시 (디버깅 용도)
        //Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public float stunRadius = 10f;
    public float stunDuration = 3.5f;



    //스턴스킬
    public void StunEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, stunRadius, Enemy);

        foreach (Collider enemy in enemies)
        {
            JEnemyFSM3 enemyController3 = enemy.GetComponent<JEnemyFSM3>();
            if (enemyController3 != null)
            {
                StartCoroutine(enemyController3.Stun3(stunDuration));
                //print("h");
            }
            JEnemyFSM enemyController = enemy.GetComponent<JEnemyFSM>();
            if (enemyController != null)
            {
                StartCoroutine(enemyController.Stun(stunDuration));
                //print("h");
            }
        }



    }
    //void RotateToMouseCursor()
    //{
    //    Vector3 mousePosition = Input.mousePosition;
    //    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    //    mousePosition.z = Camera.main.nearClipPlane;

    //    Vector3 direction = mousePosition - transform.position;
    //    direction.Normalize();
    //    //GetComponent<>(UpdateAttackDirection).attackDirection
    //    //print(mousePosition);
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    //}
    //void ShowAimIndicator(bool show)
    //{
    //    if (aimIndicatorInstance != null)
    //    {

    //        aimIndicatorInstance.SetActive(show);
    //        aimIndicatorInstance.transform.position = transform.position;
    //        Ray UltRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(UltRay, out hit, 25f))
    //        {
    //            // 공격 방향 설정
    //            UltDirection = (hit.point - transform.position).normalized;

    //            //            point.transform.position = hit.point;

    //        }
    //        else
    //        {
    //                UltDirection = transform.forward;
    //        }
    //        UltDirection.y = 0;
    //        aimIndicatorInstance.transform.localScale = new Vector3(aimRadius * 2, aimRadius * 2, 1); // 반경 크기 조정



    //            // 마우스 위치를 월드 좌표로 변환


    //    }
    //}
}




