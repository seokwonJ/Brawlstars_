using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JShellyFSM : MonoBehaviour
{

    // Enemy ���
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    // ���ʹ� ���� ����
    EnemyState m_State;

    // �÷��̾� �߰� ����
    public float findDistance = 8f;

    // �÷��̾� Ʈ������
    Transform player;

    // ���� ���� ����
    public float attackDistance = 2f;

    // �̵� �ӵ�
    public float moveSpeed = 5f;

    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    // ���� �ð�
    float currentTime = 0;

    // ���� ������ �ð�
    float attackDelay = 2f;

    // ���ʹ��� ���ݷ�
    public float attackPower = 3;

    // �ʱ� ��ġ ����� ����
    Vector3 originPos;

    // �̵� ���� ����
    public float moveDistance = 20f;

    public float hp;

    public float maxHp = 100;


    // ������̼� ������Ʈ ����
    NavMeshAgent smith;

    public GameObject bullet;

    //�÷����� ��ġ
    Vector3 playerPos;

    public float upLevel = 1.2f;

    Vector3 dir;

    private Animator animator;
    
    GameObject nearEnemy;
    public LayerMask Enemy;
    Rigidbody rb;
    // ����ȿ������(Prefab);
    //public GameObject exploFactory;

    // Start is called before the first frame update
    void Start()
    {
        // ������ ���Ӵ� ���´� ���(Idle)�� �Ѵ�.
        m_State = EnemyState.Idle;

        // �÷��̾��� Ʈ������ ������Ʈ �޾ƿ���
        player = GameObject.Find("Boss").transform;
        nearEnemy = GameObject.Find("Boss");

        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        //cc = GetComponent<CharacterController>();

        // ������̼� ������Ʈ ������Ʈ �޾ƿ���
        smith = GetComponent<NavMeshAgent>();

        //  �ִ� ü���� ���� ü�¿� �ִ´�.
        hp = maxHp;

        rb = GetComponent<Rigidbody>();
        // �ִϸ��̼�
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // ���� �������� �ƴ϶�� �Լ��� ������.
        if (GameManager.instance.isPlaying == false) return;

        if (isStunned)
        {
            return;
        }


        findEnemy();
        //// ���� �������� �ƴ϶�� �Լ��� ������.
        //if (GameManager.instance.isPlaying == false) return;

        //if (isStunned)
        //{
        //    // ���� ���¿����� �ൿ���� �ʵ��� ó��
        //    return;
        //}
        //// ���� ���¸� üũ�� �ش� ���º��� ������ ����� �����ϰ� �ϰ� �ʹ�.

        //RoundCheckColor();

        //if (isStunned)
        //{
        //    // ���� ���¿����� �ൿ���� �ʵ��� ó��
        //    return;
        //}
        // ���� ���¸� üũ�� �ش� ���º��� ������ ����� �����ϰ� �ϰ� �ʹ�.
        switch (m_State)
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
    }
    void Idle()
    {
        // ����, �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move���·� ��ȯ�Ѵ�.
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            // print("���� ��ȯ: Idle->Move");
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
        NavMeshHit Nhit;
        animator.SetBool("isMove", true);
        // ���� �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̷��� ���� �̵��Ѵ�.
        if (true)
        {


            // ������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�.
            smith.stoppingDistance = attackDistance - 9;

            // ������̼��� �������� �÷��̾��� ��ġ�� �����Ѵ�.
            smith.SetDestination(player.position);

            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.


            //smith.stoppingDistance = attackDistance;

            // ������̼� �������� �÷��̾��� ��ġ�� �����ϳ�..
            //smith.destination = player.position;

            currentTime += Time.deltaTime;

        }
        RaycastHit Rhit;

        // �׷��� �ʴٸ�, ���� ���¸� ����(Attack)���� ��ȯ�Ѵ�.
        if (Vector3.Distance(transform.position, player.position) < attackDistance && !NavMesh.Raycast(transform.position, player.position, out Nhit, NavMesh.AllAreas) && (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out Rhit, attackDistance) || Vector3.Distance(transform.position, player.position) < 2f))
        {

            if (Rhit.collider != null && Rhit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                return;
            }
            m_State = EnemyState.Attack;
            // print("���� ��ȯ: Move -> Attack");

            smith.isStopped = true;
            smith.ResetPath();


            // ���� �ð��� ���� ������ �ð���ŭ �̸� ������� ���´�.
            //currentTime = attackDelay;
        }
    }

    void Attack()
    {
        animator.SetBool("isMove", false);
        Vector3 direction = player.position - transform.position;
        direction.y = 0;  // Y�� ȸ���� ����

        NavMeshHit hit;

        // ȸ�� ���� ���
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 dd = (player.position - transform.position);
        dd.y = 0;
        transform.forward = dd;
        // �� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8);

        RaycastHit Rhit;
        if (!Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out Rhit, attackDistance) && Vector3.Distance(transform.position, player.position) > 2f)
        {
            smith.isStopped = false;
            smith.ResetPath();
            m_State = EnemyState.Move;
            //print("���� ��ȯ: Attack -> Move");
            return;
            //currentTime = 0;
        }

        // ���� �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̷��� �����Ѵ�.
        //if (Vector3.Distance(transform.position, player.position) < attackDistance && !NavMesh.Raycast(transform.position, player.position, out hit, NavMesh.AllAreas) && Rhit.collider.gameObject.tag == "Boss")
        if (Vector3.Distance(transform.position, player.position) < attackDistance && (Vector3.Distance(transform.position, player.position) < 2f || Rhit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")))
        {
   
            // ������ �ð����� �÷��̷��� �����Ѵ�.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_SATTACK);
                animator.SetTrigger("isAttack");
                //player.GetComponent<JplayerMove>().DamageAction(attackPower);
                //print("����");
                Instantiate(bullet, transform.position + Vector3.up, transform.rotation * Quaternion.Euler(0, 20f, 0));
                Instantiate(bullet, transform.position + Vector3.up, transform.rotation * Quaternion.Euler(0, 10f, 0));
                Instantiate(bullet, transform.position + Vector3.up, transform.rotation);
                Instantiate(bullet, transform.position + Vector3.up, transform.rotation * Quaternion.Euler(0, -10f, 0));
                Instantiate(bullet, transform.position + Vector3.up, transform.rotation * Quaternion.Euler(0, -20f, 0));

                currentTime = 0;
            }
        }
        // �׷��� �ʴٸ�, ���� ���¸� �̵�(Move)���� ��ȯ�Ѵ�(���߰� �ǽ�).
        else
        {
            smith.isStopped = false;
            smith.ResetPath();
            m_State = EnemyState.Move;
            //print("���� ��ȯ: Attack -> Move");
            //currentTime = 0;
        }
    }

    //void Return()
    //{
    //    // ���� �ʱ� ��ġ������ �Ÿ��� 0.1f �̻��̶�� �ʱ� ��ġ ������ �̵��Ѵ�.
    //    if (Vector3.Distance(transform.position, originPos) > 0.1f)
    //    {
    //        Vector3 dir = (originPos - transform.position).normalized;
    //        cc.Move(dir * moveSpeed * Time.deltaTime);
    //    }
    //    // �׷��� �ʴٸ�, �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ���� ���¸� ���� ��ȯ�Ѵ�.
    //    else
    //    {
    //        transform.position = originPos;
    //        // hp�� �ٽ� ȸ���Ѵ�.
    //        hp = maxHp;
    //        m_State = EnemyState.Idle;
    //        print("���� ��ȯ: Return-> Idle");
    //    }
    //}

    // ������ ���� �Լ�
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
            //print("���� ��ȯ: Any State -> Damaged");
            Damaged();
        }
        // �׷��� �ʴٸ� ���� ���·� ��ȯ�Ѵ�.
        else
        {
            m_State = EnemyState.Die;
            //print("���� ��ȯ: Any state -> Die");
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
        //print("�Ҹ�!");
        Destroy(gameObject);
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(0.5f);

        // ���� ���¸� �̵����·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        //print("���� ��ȯ: Damaged -> Move");
    }
    //�����
    //public GameObject Player;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("bullet"))
    //    {
    //        //print(333);
    //        float enemyDist;
    //        playerPos = player.position;
    //        enemyDist = Vector3.Distance(gameObject.transform.position, playerPos);
    //        //print("3�� �ֳʹ� " + enemyDist);
    //        //�� �ű� ���ƴ�!!!!! 7.12
    //        if ((enemyDist < 3))
    //        {
    //            JHPSystem close = GetComponent<JHPSystem>();
    //            close.UpdateHP(-180);
    //        }
    //        if ((gameObject.transform.position - player.position).magnitude >= 3f)
    //        {
    //            JHPSystem jhp = GetComponent<JHPSystem>();
    //            jhp.UpdateHP(-50);

    //        }

    //        EStartShake(0.3f, 0.3f, other.gameObject.transform.position);
    //    }
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        PlayerMove pm = other.gameObject.GetComponent<PlayerMove>();
    //        if (pm.isUsingUltimate == true)
    //        {
    //            JHPSystem jhp = GetComponent<JHPSystem>();
    //            jhp.UpdateHP(-200);
    //            print("�õ�");
    //        }
    //    }
    //}

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

    private void OnDestroy()
    {
        // ����ȿ���� ��������.
        //GameObject explo = Instantiate(exploFactory);
        // ������ ����ȿ���� ���� ��ġ�� ����.
        //explo.transform.position = transform.position;
    }

    public void findEnemy()
    {
        // OverlapSphere�� ����Ͽ� ���� ���� ���� ��� �� ����
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackDistance, Enemy);
        if (hitEnemies.Length > 0)
        {
            // ���� ����� ���� ã��
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


            // Raycast�� ���� �þ߿� �ִ��� Ȯ��
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (nearestEnemy.transform.position - transform.position).normalized, out hit, attackDistance))
            {
                // print(hit.transform.gameObject.name + " / nearestEnemy : " + nearestEnemy.transform.gameObject.name); ;

                if (hit.collider.gameObject == nearestEnemy.gameObject)
                {
                    // ���� �������� �ִ� �Լ� ȣ�� (�� ������Ʈ�� �����Ǿ� �־�� ��)
                    //Debug.Log("Enemy hit: " + nearestEnemy.name);
                    //nearestEnemy.GetComponent<enemy>().TakeDamage(attackDamage);
                    //GetComponent<PlayerFire>();

                    player = nearestEnemy.gameObject.transform;
                }
                else
                {
                    player = nearEnemy.transform;
                }
            }
            else
            {
                player = nearEnemy.transform;
            }

        }
        else
        {
            player = nearEnemy.transform;

        }
    }

    private bool isStunned = false;
    public float curr;
    public float lerpDuration;
    Vector3 pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, pos10;
    public IEnumerator Stun(float duration, Vector3 Bpos)
    {
        // ���⼭ ���� ���¸� ǥ���ϴ� �ִϸ��̼��̳� ȿ���� ���� �� �ֽ��ϴ�.
        // ��: animator.SetBool("isStunned", true);
        // ���� ���� ���� ���
        m_State = EnemyState.Idle;
        if (smith.isActiveAndEnabled)
        {
            smith.isStopped = true;
        }
        smith.enabled = false;



        float originalPositionY = transform.position.y;
        lerpDuration = duration;
        curr = 0;
        curr += Time.deltaTime / lerpDuration;
        curr = Mathf.Clamp01(curr);

        if (isStunned) yield break;
        isStunned = true;
        gameObject.GetComponent<JHPSystem>().UpdateHP(-1500);

        // ������ Ŀ��(������)
        pos1 = transform.position - Vector3.up * 1.6f;
        pos2 = transform.position + Vector3.up * 0.8f;
        pos3 = (transform.position + -1 * (Bpos - transform.position)) + Vector3.up * 0.8f;
        pos4 = transform.position + -1 * (Bpos - transform.position) - Vector3.up * 1.6f;
        while (true)
        {
            Vector3 dir;
            curr += Time.deltaTime / lerpDuration;
            curr = Mathf.Clamp01(curr);



            curr += Time.deltaTime / lerpDuration;
            curr = Mathf.Clamp01(curr);

            pos5 = Vector3.Lerp(pos1, pos2, curr);
            pos6 = Vector3.Lerp(pos2, pos3, curr);
            pos7 = Vector3.Lerp(pos3, pos4, curr);
            pos8 = Vector3.Lerp(pos5, pos6, curr);
            pos9 = Vector3.Lerp(pos6, pos7, curr);
            pos10 = Vector3.Lerp(pos8, pos9, curr);

            //transform.position = pos10;
            dir = pos10 - transform.position;

            //smith.velocity = dir * 5f;
            rb.linearVelocity = dir * 5f;
            if (curr == 1)
            {
                break;
            }

            yield return null;
        }
        rb.linearVelocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, originalPositionY, transform.position.z);
        isStunned = false;
        gameObject.tag = "Player";
        
        smith.enabled = true;

        if (smith.isActiveAndEnabled)
        {
            smith.isStopped = false;
        }
        smith.ResetPath();
        // ���� ���� �ִϸ��̼��̳� ȿ���� ������ �� �ֽ��ϴ�.
        // ��: animator.SetBool("isStunned", false);
    }
}