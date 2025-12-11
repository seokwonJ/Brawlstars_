using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BossFSM : MonoBehaviour
{

    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die,
        Skill1,
        Skill2,
    }


    // ���ʹ� ���� ����
    EnemyState m_State;

    // �÷��̾� �߰� ����
    public float findDistance = 8f;

    public GameObject PY;
    // �÷��̾� Ʈ������
    public Transform player;

    // ���� ���� ����
    public float attackDistance = 0.5f;

    // �̵� �ӵ�
    public float moveSpeed = 1f;

    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    // ���� �ð�
    float currentTime = 0;

    // ���� ������ �ð�
    float attackDelay = 0.5f;

    // ���ʹ��� ���ݷ�
    public float attackPower = 3;

    // �ʱ� ��ġ ����� ����
    Vector3 originPos;

    // �̵� ���� ����
    public float moveDistance = 20f;

    public float hp;

    public float maxHp = 1000;

    public bool aa = true;

    public bool bb = true;

    public float skillTiming = 15f;
    public float currentTiming = 0f;

    public NavMeshSurface navMeshSurface;

    public float BombDistance = 6f;
    float bombTiming = 4f;
    float bombCurrentTiming = 0f;

    public GameObject bomb;

    // ����ȿ������(Prefab)
    public GameObject exploFactory;

    // ������̼� ������Ʈ ����
    NavMeshAgent smith;
    Rigidbody rb;

    Vector3 playerPos;

    public float upLevel = 1.2f;

    Vector3 dir;

    Animator animator;
    public GameObject Earth;
    // Start is called before the first frame update

    GameObject nearEnemy;
    public LayerMask Enemy;


    void Start()
    {
        //mat =  mr1.material;
        //mat.color = Color.white;

        // ������ ���Ӵ� ���´� ���(Idle)�� �Ѵ�.
        m_State = EnemyState.Idle;

        // �÷��̾��� Ʈ������ ������Ʈ �޾ƿ���
        PY = GameObject.Find("Player");

        nearEnemy = GameObject.Find("Player");

        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // ������̼� ������Ʈ ������Ʈ �޾ƿ���
        smith = GetComponent<NavMeshAgent>();

        //  �ִ� ü���� ���� ü�¿� �ִ´�.
        hp = maxHp;

        // skill2 ���� �̵���ų
        rb = GetComponent<Rigidbody>();

        // �ִϸ��̼�
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        // ���� �������� �ƴ϶�� �Լ��� ������.
        if (GameManager.instance.isPlaying == false) return;
        
        findEnemy();

        RoundCheckColor();

        currentTiming += Time.deltaTime;
        if(currentTiming>skillTiming)
        {
            animator.SetBool("isAttack", false);
            if (Random.Range(0, 2) == 1)
            {
                m_State = EnemyState.Skill1;
            }
            else
            {
                m_State = EnemyState.Skill2;
            }

            currentTiming = 0;
        }
        if (Vector3.Distance(transform.position, player.position) >  BombDistance)
        {
            bombCurrentTiming += Time.deltaTime;
            if (bombCurrentTiming > bombTiming)
            {
                bombCurrentTiming = 0;
              
                Skill3();
            }
        }

        // ���� ���¸� üũ�� �ش� ���º��� ������ ����� �����ϰ� �ϰ� �ʹ�.
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack: // ��Ÿ
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
            case EnemyState.Skill1: // ������ �߻�
                Skill1();
                break;
            case EnemyState.Skill2: // ����
                Skill2();
                break;

        }
    }
    void Idle()
    {
        // ����, �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move���·� ��ȯ�Ѵ�.
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            //print("���� ��ȯ: Idle->Move");
        }
    }

    void Move()
    {
        //// ���� ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ �Ѿ�ٸ�..
        //if (Vector3.Distance(transform.position, originPos) > moveDistance)
        //{
        //    // ���� ���¸� ����(Return)�� ��ȯ�Ѵ�.
        //    m_State = EnemyState.Return;
        //    print("���� ��ȯ: Move -> Return");
        //}
        animator.SetBool("isAttack", false);

        // ���� �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̷��� ���� �̵��Ѵ�.
        if (Vector3.Distance(transform.position, player.position) > attackDistance - 0.5f)
        {
            //// �̵� ���� ����
            //Vector3 dir = (player.position - transform.position).normalized;

            ////// ĳ���� ��Ʈ�ѷ��� �̿��� �̵��ϱ�
            //cc.Move(dir * moveSpeed * Time.deltaTime);



            // ������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�.
            smith.stoppingDistance = attackDistance-1;

            // ������̼��� �������� �÷��̾��� ��ġ�� �����Ѵ�.
            smith.SetDestination(player.position);

            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.

  
            // ������̼� �������� �÷��̾��� ��ġ�� �����ϳ�..
            //smith.destination = player.position;


            currentTime += attackDelay;
        }

        // �׷��� �ʴٸ�, ���� ���¸� ����(Attack)���� ��ȯ�Ѵ�.
        else
        {
            //m_State = EnemyState.Attack;
            m_State = EnemyState.Attack; // ��ų ���ܾ� �׽�Ʈ
            print("���� ��ȯ: Move -> Attack");

            smith.isStopped = true;
            smith.ResetPath();

            // ���� �ð��� ���� ������ �ð���ŭ �̸� ������� ���´�.
            //currentTime = attackDelay;
        }
    }

    void Attack()
    {
        animator.SetBool("isAttack", true);

        Vector3 direction = player.position - transform.position;
        direction.y = 0;  // Y�� ȸ���� ����

        // ȸ�� ���� ���
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // �� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 60);


        // ���� �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̷��� �����Ѵ�.
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // ������ �ð����� �÷��̾ �����Ѵ�.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {

                SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_EATTACK);

                //player.GetComponent<JplayerMove>().DamageAction(attackPower);
                PY.GetComponent<JHPSystem>().UpdateHP(-1000);
                //print("����");
                currentTime = 0;
            }
        }
        // �׷��� �ʴٸ�, ���� ���¸� �̵�(Move)���� ��ȯ�Ѵ�(���߰� �ǽ�).
        else
        {
            m_State = EnemyState.Move;
           // print("���� ��ȯ: Attack -> Move");
            currentTime = 0;
        }
    }

    void Skill1()
    {
        if (bb)
        {
            StartCoroutine(Skill1Start());
        }
    }
    public GameObject bulletPrefab;
    IEnumerator Skill1Start()
    {
        smith.isStopped = true;
        bb = false;
        yield return new WaitForSeconds(1f);
        int bulletCount = 8;
        float interval = 0.2f;
        float coneAngle = 45f; // ���� ����
        float angleStep = coneAngle / (bulletCount - 1);
        float startAngle = -coneAngle / 2;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 direction = rotation * transform.forward;

            Instantiate(bulletPrefab, transform.position + transform.up * 1f, Quaternion.LookRotation(direction));
 
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForSeconds(1f);
        smith.isStopped = false;
        bb = true;
        m_State = EnemyState.Attack;
        //print("Skill1 -> Move");
    }
    void Skill2()
    {
        if (aa)
        {
            StartCoroutine(Skill2Start());
        }
    }

    bool skilling = false;
    IEnumerator Skill2Start()
    {
        skilling = true;
        aa = false;
        // ��� ���߱�
        smith.isStopped = true;
        Vector3 dashDirection = transform.forward;
        yield return new WaitForSeconds(1f); // 1�ʰ� ����
        gameObject.tag = "Skill";
        // �����ϱ�
        //smith.isStopped = false;
        smith.speed = moveSpeed * 4; // ���� �ӵ� ����
        
        float dashTime = 2f; // ���� �ð�
        float elapsedTime = 0f;
        float soundTime = 0f;
        float soundEndTime = 0.2f;
        while (elapsedTime < dashTime)
        {
            if (soundTime > soundEndTime)
            {

                SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_BDASH);
                GameObject ear = Instantiate(Earth);
                ear.transform.position = transform.position;
                soundTime = 0;

            }
            //cc.Move(dashDirection * smith.speed * Time.deltaTime);
            rb.linearVelocity = dashDirection * smith.speed;
            //transform.position += dashDirection * smith.speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            soundTime += Time.deltaTime;
            yield return null;
        }
        rb.linearVelocity = Vector3.zero;
        // �ٽ� ���߱�
        navMeshSurface.BuildNavMesh();
        smith.isStopped = true;
        gameObject.tag = "Boss";


        yield return new WaitForSeconds(1f); // 1�ʰ� ����
        skilling = false;
        P1stun = false;
        P2stun = false;
        P3stun = false;
        // Move ���·� ���ư���
        aa = true;
        //print("Skill2 -> Move");
        smith.isStopped = false;
        smith.speed = moveSpeed; // ���� �ӵ��� ����
        m_State = EnemyState.Move;
        smith.ResetPath();
        

    }

    void Skill3()
    {
        GameObject bbom = Instantiate(bomb);
        bbom.transform.position = transform.position + Vector3.up * 3;
    }

    


    public void HitEnemy(int hitPower)
    {
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }
        // �÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��.
        hp -= hitPower;

        // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
        smith.isStopped = true;
        smith.ResetPath();

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ�Ѵ�.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ: Any State -> Damaged");
            Damaged();
        }
        // �׷��� �ʴٸ� ���� ���·� ��ȯ�Ѵ�.
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ: Any state -> Die");
            Die();
        }
    }

    // ���� �����Լ�
    public void Die()
    {
        // ���� ���� �ǰ� �ڷ�ƾ�� �����Ѵ�.
    }
    void Damaged()
    {
        // �ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�;
        StartCoroutine(DamageProcess());

        // ���� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ�� ��Ȱ��ȭ��Ų��.
        cc.enabled = false;

        // 2�� ���� ��ٸ� �Ŀ� �ڱ� �ڽ��� �����Ѵ�.
        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(0.5f);

        // ���� ���¸� �̵����·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("bullet"))
        {
            float enemyDist;
            playerPos = GameObject.Find("Player").transform.position;
            enemyDist = Vector3.Distance(gameObject.transform.position, playerPos);
            //print("3�� �ֳʹ� " + enemyDist);
            //�� �ű� ���ƴ�!!!!! 7.12
            if ((enemyDist < 3))
            {
                JHPSystem close = GetComponent<JHPSystem>();
                close.UpdateHP(-2750);
            }
            if ((gameObject.transform.position - playerPos).magnitude >= 3f)
            {
                JHPSystem jhp = GetComponent<JHPSystem>();
                jhp.UpdateHP(-560);

            }
            EStartShake(0.3f, 0.1f, other.gameObject.transform.position);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("bullet2"))
        {
            float enemyDist;
            playerPos = PY.transform.position;
            enemyDist = Vector3.Distance(gameObject.transform.position, playerPos);


            //�� �ű� ���ƴ�!!!!! 7.12

                JHPSystem close = GetComponent<JHPSystem>();
                close.UpdateHP(-1500);



            //EStartShake(0.3f, 0.3f, other.gameObject.transform.position);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("bullet4"))
        {
            JHPSystem jhp = GetComponent<JHPSystem>();
            jhp.UpdateHP(-600);
        }


        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMove pm = other.gameObject.GetComponent<PlayerMove>();
            if (pm == null);
            else if (pm.isUsingUltimate == true)
            {
                JHPSystem jhp = GetComponent<JHPSystem>();
                jhp.UpdateHP(-2000);
                print("�õ�");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && gameObject.tag == "Skill")
        {

            StunEnemies(other.gameObject);
            // �Ŀ� �Ʊ� �Ǹ� ����
            //JHPSystem Php = PY.GetComponent<JHPSystem>();
            //Php.UpdateHP(-100);
        }
    }


    private void OnDestroy()
    {
        // 2�ʰ� ��� ���� ����ٰ� ��ȯ
        // ������ ������ ���� Ŭ���� ȭ������ ��ȯ.
        GameObject explo = Instantiate(exploFactory);
        // ������ ����ȿ���� ���� ��ġ�� ����.
        explo.transform.position = transform.position;
        //SceneManager.LoadScene("ClearScene");
    }

    bool P1stun = false;
    bool P2stun = false;
    bool P3stun = false;

    //���Ͻ�ų
    public void StunEnemies(GameObject py)
    {

        if (py.GetComponent<PlayerMove>() && P1stun == false)
        {
            P1stun = true;
            PlayerMove playerMove = py.GetComponent<PlayerMove>();
            StartCoroutine(playerMove.Stun(1.0f, transform.position));
        }
        else if (py.GetComponent<PlayerMove2>() && P2stun == false)
        {
            P2stun = true;
            PlayerMove2 playerMove2 = py.GetComponent<PlayerMove2>();
            StartCoroutine(playerMove2.Stun(1.0f, transform.position));
        }
        else if (py.GetComponent<JShellyFSM>() && P3stun == false)
        {
            P3stun = true;
            JShellyFSM JShellyfsm = py.GetComponent<JShellyFSM>();
            StartCoroutine(JShellyfsm.Stun(1.0f, transform.position));
        }


    }


    // ���帶�� ���ݷ� �ӵ� ü�� �����ϴ� �Լ�
    void RoundCheckColor()
    {

        if (GameManager.instance.round == 2)
        {
            if (upLevel == 1.3f) return;
            upLevel = 1.3f;
            smith.speed *= upLevel;
            attackPower *= upLevel;
        }
        if (GameManager.instance.round == 3)
        {
            if (upLevel == 1.4f) return;
            upLevel = 1.4f;
            smith.speed *= upLevel;
            attackPower *= upLevel;
        }
        if (GameManager.instance.round == 4)
        {
            if (upLevel == 1.5f) return;
            upLevel = 1.5f;
            smith.speed *= upLevel;
            attackPower *= upLevel;
        }
        if (GameManager.instance.round == 5)
        {
            if (upLevel == 1.6f) return;
            upLevel = 1.6f;
            smith.speed *= upLevel;
            attackPower *= upLevel;
        }
    }

    public void EStartShake(float duration, float magnitude, Vector3 pos)
    {
        dir = transform.position - pos;
        dir.Normalize();
        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float currentTimeShake = 0;

        //����
        while (currentTimeShake < duration)
        {
            dir.y = 0;
            transform.position += dir * magnitude;

            dir = -dir;

            currentTimeShake += Time.deltaTime;

            //�� ������ ��ٸ���
            yield return null;
        }
    }

    public void findEnemy()
    {
        // OverlapSphere�� ����Ͽ� ���� ���� ���� ��� �� ����
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 100f, Enemy);
        if (hitEnemies.Length > 0)
        {
            // ���� ����� ���� ã��
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
        }
        else
        {
            player = nearEnemy.transform;
            PY = nearEnemy.gameObject;
        }
    }
}
