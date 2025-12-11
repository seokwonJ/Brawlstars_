using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JEnemyFSM : MonoBehaviour
{
    // Enemy 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }


    // 에너미 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    public float findDistance = 8f;

    // 플레이어 트랜스폼
    GameObject PY;

    Transform player;

    // 공격 가능 범위
    public float attackDistance = 2f;

    // 이동 속도
    public float moveSpeed = 5f;

    // 캐릭터 콘트롤러 컴포넌트
    CharacterController cc;

    // 누적 시간
    float currentTime = 0;

    // 공격 딜레이 시간
    float attackDelay = 0.5f;

    // 에너미의 공격력
    public float attackPower = 3;

    // 초기 위치 저장용 변수
    Vector3 originPos;

    // 이동 가능 범위
    public float moveDistance = 20f;

    public float hp;

    public float maxHp=100;

    public float upLevel = 1.2f;

    //플레이어의 위치값
    Vector3 playerPos;


    // 내비게이션 에이전트 변수
    NavMeshAgent smith;

    Vector3 dir;

    private Animator animator;

    GameObject nearEnemy;
    public LayerMask Enemy;


    // 폭발효과공장(Prefab);
    public GameObject exploFactory;

    // Start is called before the first frame update
    void Start()
    {

        // 최초의 에머니 상태는 대기(Idle)로 한다.
        m_State = EnemyState.Idle;

        // 플레이어의 트랜스폼 컴포넌트 받아오기
        PY = GameObject.Find("Player");
        player = PY.transform;
        nearEnemy = GameObject.Find("Player");

        // 캐릭터 콘트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        // 내비게이션 에이전트 컴포넌트 받아오기
        smith = GetComponent<NavMeshAgent>();

        //  최대 체력을 현재 체력에 넣는다.
        hp = maxHp;

        // 애니메이션
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        // 현재 게임중이 아니라면 함수를 나가자.
        if (GameManager.instance.isPlaying == false) return;

        findEnemy();

        RoundCheckColor();

        // 현재 상태를 체크해 해당 사태별로 정해진 기능을 수행하게 하고 싶다.
        if (isStunned)
        {
            // 기절 상태에서는 행동하지 않도록 처리
            return;
        }
        switch(m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                //Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }


        // 적의 일반적인 행동 로직
    }
    void Idle()
    {
        // 만일, 플레이어와의 거리가 액션 시작 범위 이내라면 Move상태로 전환한다.
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            //print("상태 전환: Idle->Move");
        }
    }

    void Move()
    {
        //// 만일 현재 위치가 초기 위치에서 이동 가능 범위를 넘어간다면..
        //if (Vector3.Distance(transform.position, originPos) > moveDistance)
        //{
        //    // 현재 상태를 복귀(Return)로 전환한다.
        //    m_State = EnemyState.Return;
        //    print("상태 전환: Move -> Return");
        //}

        animator.SetBool("isAttack", false);
        // 만일 플레이어와의 거리가 공격 범위 밖이라면 플레이러를 향해 이동한다.
        if(Vector3.Distance(transform.position, player.position) > attackDistance - 0.5f)
        {
            //// 이동 방향 설정
            //Vector3 dir = (player.position - transform.position).normalized;

            ////// 캐릭터 콘트롤러를 이용해 이동하기
            //cc.Move(dir * moveSpeed * Time.deltaTime);



            // 내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다.
            smith.stoppingDistance = attackDistance-1f;

            // 내비게이션의 목적지를 플레이어의 위치로 설정한다.
            smith.SetDestination(player.position);

            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.


            //smith.stoppingDistance = attackDistance;

            // 내비게이션 목적지를 플레이어의 위치로 설정하낟..
            //smith.destination = player.position;

            currentTime += Time.deltaTime;

        }

        // 그렇지 않다면, 현재 상태를 공격(Attack)으로 전환한다.
        else
        {
            m_State = EnemyState.Attack;
            //print("상태 전환: Move -> Attack");

            smith.isStopped = true;
            smith.ResetPath();

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓는다.
            //currentTime = attackDelay;
        }
    }

    void Attack()
    {
        animator.SetBool("isAttack", true);
        Vector3 direction = player.position - transform.position;
        direction.y = 0;  // Y축 회전만 고려

        // 회전 각도 계산
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // 적 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8);


        // 만일 플레이어가 공격 범위 이내에 있다면 플레이러를 공격한다.
        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            // 일정한 시간마다 플레이러르 공격한다.
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                //player.GetComponent<JplayerMove>().DamageAction(attackPower);

                SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_EATTACK);
                PY.GetComponent<JHPSystem>().UpdateHP(-attackPower);
                currentTime = 0;
            }
        }
        // 그렇지 않다면, 현재 상태를 이동(Move)으로 전환한다(재추격 실시).
        else
        {
            m_State = EnemyState.Move;
            //print("상태 전환: Attack -> Move");
            currentTime = 0;
        }
    }

    public void HitEnemy(int hitPower)
    {
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        } 
        // 플레이어의 공격력만큼 에너미의 체력을 감소시킨다.
        hp -= hitPower;

        // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
        smith.isStopped = true;
        smith.ResetPath();

        // 에너미의 체력이 0보다 크면 피격 상태로 전환한다.
        if (hp >0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any State -> Damaged");
            Damaged();
        }
        // 그렇지 않다면 죽음 상태로 전환한다.
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            Die();
        }
    }

    // 죽은 상태함수
    public void Die()
    {
        // 진행 중인 피격 코루틴을 중지한다.
    }
    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴을 실행한다;
        StartCoroutine(DamageProcess());

        // 죽음 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 콘트롤러 컴포넌트를 비활성화시킨다.
        cc.enabled = false;

        // 2초 동안 기다린 후에 자기 자신을 제거한다.
        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(0.5f);

        // 현재 상태를 이동사태로 전환한다.
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }
    //여기두
    //public GameObject jkyPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("bullet"))
        {            
            float enemyDist ;
            playerPos = PY.transform.position;
            enemyDist = Vector3.Distance(gameObject.transform.position, playerPos);
           
            
            //ㅇ ㅕ기 고쳤다!!!!! 7.12
            if ((enemyDist < 3))
            {
                
                JHPSystem close = GetComponent<JHPSystem>();
                close.UpdateHP(-2750);
            }
            if ((transform.position - player.transform.position).magnitude >= 3f)
            {
                JHPSystem jhp = GetComponent<JHPSystem>();
                jhp.UpdateHP(-560);

            }

            EStartShake(0.3f, 0.3f, other.gameObject.transform.position);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("bullet2"))
        {
            float enemyDist;
            playerPos = PY.transform.position;
            enemyDist = Vector3.Distance(gameObject.transform.position, playerPos);


            JHPSystem close = GetComponent<JHPSystem>();
            close.UpdateHP(-1500);
     

            //EStartShake(0.3f, 0.3f, other.gameObject.transform.position);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("bullet4"))
        {
            JHPSystem jhp = GetComponent<JHPSystem>();
            jhp.UpdateHP(-600);
        }

        //궁극기맞으면
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMove pm = other.gameObject.GetComponent<PlayerMove>();
            if (pm ==null)
            {
            }
            else if (pm.isUsingUltimate == true)
            {
                JHPSystem jhp = GetComponent<JHPSystem>();
                jhp.UpdateHP(-2000);
                //print("궁딜");

            }

        }
       
    }


    private bool isStunned = false;

    public IEnumerator Stun(float duration)
    {
        if (isStunned) yield break;

        isStunned = true;
        // 여기서 기절 상태를 표시하는 애니메이션이나 효과를 넣을 수 있습니다.
        // 예: animator.SetBool("isStunned", true);
        // 기절 상태 동안 대기
        yield return new WaitForSeconds(duration);
        //kprint("stun");
        isStunned = false;
        // 기절 해제 애니메이션이나 효과를 제거할 수 있습니다.
        // 예: animator.SetBool("isStunned", false);
    }

    void RoundCheckColor()
    {

        if (GameManager.instance.round == 2)
        {
            if (upLevel == 1.3f) return;
            upLevel = 1.3f;
            smith.speed *= upLevel;
            maxHp *= upLevel;
            attackPower *= upLevel;
        }
        if (GameManager.instance.round == 3)
        {
            if (upLevel == 1.4f) return;
            upLevel = 1.4f;
            smith.speed *= upLevel;
            maxHp *= upLevel;
            attackPower *= upLevel;
        }
        if (GameManager.instance.round == 4)
        {
            if (upLevel == 1.5f) return;
            upLevel = 1.5f;
            smith.speed *= upLevel;
            maxHp *= upLevel;
            attackPower *= upLevel;
        }
        if (GameManager.instance.round == 5)
        {
            if (upLevel == 1.6f) return;
            upLevel = 1.6f;
            smith.speed *= upLevel;
            maxHp *= upLevel;
            attackPower *= upLevel;

        }
    }

    // 진동
    public void EStartShake(float duration, float magnitude, Vector3 pos)
    {
        dir = transform.position - pos;
        dir.Normalize();
        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float currentTimeShake = 0;

        //조건
        while (currentTimeShake < duration)
        {
            dir.y = 0;
            transform.position += dir * magnitude;

            dir = -dir;

            currentTimeShake += Time.deltaTime;

            //한 프레임 기다린다
            yield return null;
        }
    }

    private void OnDestroy()
    {
        // 폭발효과를 생성하자.
        GameObject explo = Instantiate(exploFactory);
        // 생성된 폭발효과를 나의 위치에 놓자.
        explo.transform.position = transform.position;
    }


    public void findEnemy()
    {
        // OverlapSphere를 사용하여 공격 범위 내의 모든 적 감지
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 100f, Enemy);
        if (hitEnemies.Length > 0)
        {
            // 가장 가까운 적을 찾기
            Collider nearestEnemy = hitEnemies[0];
            float closestDistance = Vector3.Distance(transform.position, hitEnemies[0].transform.position);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.gameObject.GetComponent<JHPSystem>().HPReturn() <= 0)
                {
                    continue;
                }
                if (nearestEnemy.gameObject.GetComponent<JHPSystem>().HPReturn() <= 0)
                {
                    nearestEnemy = enemy;
                }
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                    nearEnemy = enemy.gameObject;
                }

            }

            player = nearestEnemy.gameObject.transform;
            PY = nearestEnemy.gameObject;
            // Raycast로 적이 시야에 있는지 확인
            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, (nearestEnemy.transform.position - transform.position).normalized, out hit, 100f))
            //{
            //    // print(hit.transform.gameObject.name + " / nearestEnemy : " + nearestEnemy.transform.gameObject.name); ;

            //    if (hit.collider.gameObject == nearestEnemy.gameObject)
            //    {
            //        // 적에 데미지를 주는 함수 호출 (적 오브젝트에 구현되어 있어야 함)
            //        //Debug.Log("Enemy hit: " + nearestEnemy.name);
            //        //nearestEnemy.GetComponent<enemy>().TakeDamage(attackDamage);
            //        //GetComponent<PlayerFire>();

            //        player = nearestEnemy.gameObject.transform;
            //        PY = nearestEnemy.gameObject;
            //    }
            //    else
            //    {
            //        player = nearEnemy.transform;
            //        PY = nearEnemy.gameObject;
            //    }
            //}
            //else
            //{
            //    player = nearEnemy.transform;
            //    PY = nearEnemy.gameObject;
            //}

        }
        else
        {
            player = nearEnemy.transform;
            PY = nearEnemy.gameObject;
        }
    }

}
