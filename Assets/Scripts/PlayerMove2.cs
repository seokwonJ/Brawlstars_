using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class PlayerMove2 : MonoBehaviour
{

    // Start is called before the first frame update
    //enum Player2State
    //{
    //    Idle,
    //    Move,
    //    Attack,
    //    Return,
    //    Damaged,

    //}
    public bool isAlive = true;
    public float hp;
    public LayerMask Enemy;
    public float lerpSpeed = 1f;
    public float maxHp = 300;
    Vector3 finalDir;
    // ���ʹ� ���� ����
    //Player2State m_State;
    // ���� ���� ����
    public float attackDistance = 2f;
    public float moveSpeed = 6;
    public GameObject pang;
    public GameObject firePos;
    public GameObject player;
    public GameObject bulletFactory3;
    Vector3 skillDir;

    public Animator PocoAnim;
    public GameObject HealArea;
    GameObject healarea;
    Vector3 pp;
    void Start()
    {
        // ������ ���Ӵ� ���´� ���(Idle)�� �Ѵ�.Player2State.Idle;
        //m_State = Player2State.Idle;
        rb = GetComponent<Rigidbody>();
        ultGauge2 = GetComponent<UltGauge2>();
        //  �ִ� ü���� ���� ü�¿� �ִ´�.
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        pp = transform.position;
        if (GameManager.instance.isPlaying == false)
        {
            gameObject.transform.position = transform.position;
            rb.linearVelocity = new Vector3(0, 0, 0);
            return;
        }
        //float f = Input.GetAxis("aaa");
        //float d = Input.GetAxis("bbb");
        AttackSpot();
       // print(f + ", " + d);
        //switch (m_State)
        //{
        //    case Player2State.Idle:
        //    case Player2State.Idle:
        //        Idle();
        //        break;
        //    case Player2State.Move:
        //        Move();
        //        break;
        //    case Player2State.Attack:
        //        Attack();
        //        break;
        //    case Player2State.Return:
        //        //Return();
        //        break;
        //    case Player2State.Damaged:
        //        Damaged();
        //        break;
        //    case Player2State.Die:
        //        Die();
        //        break;
        //}
        Idle();
        Move();
        Attack();
        Damaged();
        if (healarea != null)
        {
            healarea.transform.position = transform.position;
        }
    }



    public bool hpupdown = true;
    public float curTime = 0;
    public JHPSystem hpup;
    void Idle()
    {


       // m_State = Player2State.Move;
            
        
    }
    void Move()
    {
        float h = Input.GetAxis("Horizontal2");
        float v = Input.GetAxis("Vertical2");
        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir = dirH + dirV;

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3);
        //pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(skillDir), 9f * Time.deltaTime);

        if (dir.magnitude > 0) // && pc.isAttacking == false//
        {
            pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(dir), 7f * Time.deltaTime);
        }
        //// �̵��ӵ� �������ϰ�
        if (dir.magnitude > 1)
        {
            dir = dir.normalized;
        }
        
        finalDir = Vector3.Lerp(finalDir, dir, lerpSpeed * Time.deltaTime);
        
        if (finalDir == Vector3.zero)
        {
            PocoAnim.SetBool("isMoving", false);
        }
        else
        {
            PocoAnim.SetBool("isMoving", true);
        }
        //transform.position += finalDir * moveSpeed * Time.deltaTime;
        rb.linearVelocity = finalDir * moveSpeed;
    }


    public float createTime = 1.2f;
    public float currTime = 0;
    public GameObject aimIndicatorAttack;
    public RangeAttack ra;
    //public bool isAttacking = false;
    public UltGauge2 ultGauge2;
    Rigidbody rb;
    private bool isAiming;
    public RangeImage2 rg;
    public float createTim = 1.2f;
    public float currTim = 0;
    public bool stop = false;
    public GameObject AimIndicator;
    public float ultimateSpeed = 4f;
    //public bool isUsingUltimate = false;
    void Attack()
    {
        normalAttack();
        ultAttack();
        skill();
    }

    void normalAttack()
    {
        if (Input.GetButton("Fire44"))
        {

            currTime += Time.deltaTime;

            if (currTime > createTime)
            {
                //ShowAimIndicator(true);
                //RotateToMouseCursor();
                print("��Դ�����");
                AimIndicator.SetActive(true);
                rg = AimIndicator.GetComponent<RangeImage2>();
                rg.range();
            }
        }
        if (Input.GetButtonUp("Fire44") && currTime > createTime)
        {
            print("��Դ�����");
            AimIndicator.SetActive(false);
            //isAttacking = true;
            ReleaseAttack();
            //isAttacking = false;
            currTime = 0;
            SetHpUpDown(false);

        }
        else if (Input.GetButtonUp("Fire44") && currTime <= createTime)
        {
            AimIndicator.SetActive(false);
            //isAttacking = true;
            FastAttack();
            //isAttacking = false;
            currTime = 0;
            SetHpUpDown(false);
        }
        else
        {
            //isAttacking = false;
            //print(111);

        }
       // print(111);

    }
    void ultAttack()
    {
        //�ñر�
        if (Input.GetButton("Fire11"))
        {
            currTim += Time.deltaTime;
            if (currTim > createTim)
            {
                if (ultGauge2.currentGauge == 100)
                {
                    //print(111);
                    print(111);
                    
                    AimIndicator.SetActive(true);
                    rg = AimIndicator.GetComponent<RangeImage2>();
                    rg.range();
                }

            }

        }
        if (Input.GetButtonUp("Fire11") && ultGauge2.currentGauge == 100 && currTim > createTim)
        {
            //print(111);
            print("���̴�1");

            AimIndicator.SetActive(false);
            SlowUltimate();
            currTim = 0;
            ultGauge2.currentGauge = 0;
        }
        else if (Input.GetButtonUp("Fire11") && ultGauge2.currentGauge == 100)
        {
            AimIndicator.SetActive(false);
            print("���̴�1");
            StartUltimate();
            currTim = 0;

            ultGauge2.currentGauge = 0;


        }

    }
    private bool isSkillOnCooldown;
    public int skillUsesLeft;
    public float skillCooldown = 1f;
    public void skill()
    {
        //Q������ ��ų
        if (Input.GetButtonUp("Fire33") && !isSkillOnCooldown && skillUsesLeft > 0)
        {
            StartCoroutine(UseSkill());
            SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_GAGET);
        }

    }

    public IEnumerator UseSkill()
    {
        //print("v");
        StartCoroutine(Heal());
        skillUsesLeft--;
        isSkillOnCooldown = true;

        yield return new WaitForSeconds(skillCooldown);

        isSkillOnCooldown = false;
        //print("v");
        
    }
    public float heallAmountPerSecond = 5000f;
    public float skillRange = 15f;
    public float healDuration = 5f;
    IEnumerator Heal()
    {
        healarea = Instantiate(HealArea);

        float elapsed = 0f;
        while (elapsed < healDuration)
        {
            healarea.transform.position = transform.position;
            Collider[] hitPlayers = Physics.OverlapSphere(transform.position, skillRange, 1 << LayerMask.NameToLayer("Player"));
            print(hitPlayers.Count());
            foreach (Collider hitCollider in hitPlayers)
            {
                JHPSystem hp = hitCollider.GetComponent<JHPSystem>();

                //print(111);
                //hp.currHP = hp.currHP + heallAmountPerSecond * Time.deltaTime;
                //hp.currHP = Mathf.Min(hp.currHP + heallAmountPerSecond , hp.maxHP);
                hp.UpdateHP(2000);
            }
            elapsed += 1f;
            yield return new WaitForSeconds(1f);

        }
        Destroy(healarea);
    }
    void StartUltimate()
    {
        //print(111);
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_PULT);
        print("startltimate�Լ����� ���Դ�");

        GameObject bullet3 = Instantiate(bulletFactory3);

        //isUsingUltimate = true;

        //ultEnemy.Add(GetComponent<PlayerController>().nearestEnemyObj);
        //nextEnemy = GetComponent<PlayerController>().nearestEnemyObj;

        //SkillDirMake();
        firePos.transform.forward = player.transform.position - gameObject.transform.position;
        bullet3.transform.position = firePos.transform.position;
        bullet3.transform.forward = firePos.transform.forward;
        //print(111);
    }
    public PlayerController2 pc;
    public bool slowUltStart = false;
    public Vector3 endPos;
    void SlowUltimate()
    {
        print("slowultimate�Լ����� ���Դ�");
        GameObject bullet3 = Instantiate(bulletFactory3);
        bullet3.transform.position = firePos.transform.position;
        slowUltStart = true;
        print("�������");
        endPos = pc.UpdateAttackPosition();
        firePos.transform.forward = pc.UpdateAttackDirection();
        bullet3.transform.forward = firePos.transform.forward;
        //print(222);


    }

    void StopUltimate()
    {
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_PULT);
        //isUsingUltimate = false;
        slowUltStart = false;
        finalDir = Vector3.zero;
        //currentSkillMoveCount = 0;
        //ultEnemy.Clear();
        //overlapTime = 0;
         stop = false;
    }

    void Damaged()
    {

    }





    public void SetHpUpDown(bool updown)
    {
        hpupdown = updown;
        curTime = 0;
    }

    public PlayerFire2 pf;
    void ReleaseAttack()
    {
        //isAttacking = true;
        //pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position), 9f * Time.deltaTime);
        //pang.transform.forward = (nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position);

        pang.transform.forward = UpdateAttackDirection();

        pf = gameObject.GetComponent<PlayerFire2>();

        pf.Attack();


    }

    void FastAttack()
    {
        //isAttacking = true;
        //pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(UpdateAttackDirection()), 9f * Time.deltaTime);

        //pang.transform.forward = UpdateAttackDirection();
        pf = gameObject.GetComponent<PlayerFire2>();

        pf.FastAttack();


    }

    private Vector3 attackDirection;
    public Vector3 UpdateAttackDirection()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // ���� ���� ����
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


    public Vector3 AttackSpot()
    {
        float f = Input.GetAxis("aaa");
        float d = Input.GetAxis("bbb");

        Vector3 attackSpot = new Vector3(f, 0, d);
        return attackSpot;
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

            rb.linearVelocity = dir * 5f;
            if (curr == 1)
            {
                break;
            }
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, originalPositionY, transform.position.z);
        isStunned = false;
        // ���� ���� �ִϸ��̼��̳� ȿ���� ������ �� �ֽ��ϴ�.
        // ��: animator.SetBool("isStunned", false);
    }
    //public void Die()
    //{
    //    if (gameObject.GetComponent<JHPSystem>().currHP <= 0)
    //    {
    //        isAlive = false;
    //        gameObject.SetActive(false);
    //        GameManager2.Instance.PlayerDied(2);

    //        Respawn(pp);


    //    }
    //}



    ////Vector3 spawnPosition = new Vector3(-0.82f, -4.64f, 11.79f);
    //public void Respawn(Vector3 spawnPosition)
    //{
    //    isAlive = true;
    //    transform.position = spawnPosition;
    //    gameObject.SetActive(true); // �÷��̾� Ȱ��ȭ
    //    gameObject.GetComponent<JHPSystem>().currHP = gameObject.GetComponent<JHPSystem>().maxHP;

    //}
}
