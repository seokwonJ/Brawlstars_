using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
//using static UnityEditor.U2D.ScriptablePacker;

//�ּ�: ctrl �������·� K->c
//    �ּ� Ǯ�� : ctrl �������·� k ->u
// �ڵ����� : ctrl �������·�  k-> f

public class PlayerMove : MonoBehaviour
{

    // Start is called before the first frame update


    //public float skillSpeed = 100f;
    public float moveSpeed = 6;
    public float lerpSpeed = 1f;
    Vector3 finalDir;
    Vector3 pp;
    public float ultimateSpeed = 4f;
    public bool isUsingUltimate = false;
    private float ultimateStartTime;
    public int hp = 15;
    //public LayerMask Enemy;
    //�ִ� ��ų �̵� Ƚ�� 4
    int maxSkillMoveCount = 4;
    //���� ��ų �̵� Ƚ�� 0
    int currentSkillMoveCount = 0;
    //���� �ε��� �� ����Ʈ
    public List<GameObject> ultEnemy = new List<GameObject>();
    //��ų�� ����ϸ� ���� ����
    Vector3 skillDir;
    public GameObject nextEnemy;


    public GameObject enemy;

    public float ultRange = 28f;
    public GameObject pang;
    public LayerMask Enemy;
    public UltGauge  ultGauge;
    Rigidbody rb;
    private bool isAiming;
    //public GameObject aimIndicatorPrefab; // ���� �ݰ��� ǥ���ϴ� ������
    //public GameObject aimIndicatorPrefab1; // ���� �ݰ��� ǥ���ϴ� ������
    //private GameObject aimIndicatorInstance;
    public RangeImage rg;
    float currTime = 0;
    float createTime = 1.5f;
    public bool isUlt = false;
    public GameObject AimIndicator;
    public bool stop = false;
    public Animator FangAnim;
    public int playerID;
    public bool isAlive = true;
    public GameObject UltParticle;
    GameObject ultParticle;
    public GameObject UltHitParticle;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ultGauge = GetComponent<UltGauge>();
        //AimIndicator = GameObject.Find("AimIndicator");
    }

    // Update is called once per frame
    void Update()
    {
        pp = transform.position;
        //Die();
        // ���� �������� �ƴ϶�� �Լ��� ������.

        if (GameManager.instance.isPlaying == false)
        {
            gameObject.transform.position = transform.position;
            rb.linearVelocity = new Vector3(0,0,0);
            return;
        }


        if (isStunned)
        {
            return;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //�⺻����
        //q�� ������ �⺻������ ������
        //�׷��� ���� ����� ���� ���� 1m�Ÿ� �ȿ� ������ ���������� �Ѵ�
        //�׷��� ������ �Ѿ��� �߻��Ѵ�

        //��ų


        // �ñر��̵�
        if (Input.GetKey(KeyCode.Space))
        {
            currTime += Time.deltaTime;
            if (currTime > createTime)
            {
                if (ultGauge.currentGauge == 100)
                {
                    AimIndicator.SetActive(true);
                    rg = AimIndicator.GetComponent<RangeImage>();
                    rg.range();
                }
                
            }

        }
        if(Input.GetKeyUp(KeyCode.Space) && isUsingUltimate == false && ultGauge.currentGauge == 100 && currTime > createTime)
        {
            AimIndicator.SetActive(false);
            SlowUltimate();
            currTime = 0;
            ultGauge.currentGauge = 0;
        }
         else if (Input.GetKeyUp(KeyCode.Space) && isUsingUltimate == false && ultGauge.currentGauge == 100)
        {
            AimIndicator.SetActive(false);
            //isUlt = false;
            //ShowAimIndicator(false);
            
            //isAiming = false;
            StartUltimate();

            //Physics.SphereCastAll �� ����� ����ؼ� ���� Enemy�� ã�´�
            //���ʹ̸� ã�� �Ŀ� ���ʹ� ������ �Ÿ����� ���ؼ�
            //�ּҰ��� ���ؼ� �� �ּҰ��� ���ʹ̿���
            //��Ƽ����Ʈ�̵��� �Ѵ�
            ultGauge.currentGauge = 0;


            //char[] numbers = new char[] { enemy1, enemy2, enemy3, enemy4, bos };
            // dir = enemy.transform.position - transform.position
        }
    
         if(slowUltStart)
        {

            Vector3 dirdir = endPos - transform.position;
            dirdir.y = 0;
            float distance = dirdir.magnitude;
            //if (distance < ultimateDistance)

            dirdir.Normalize();

            finalDir = dirdir * ultimateSpeed;

            //print(distance);
            
            //if (stop == true)

            //{
            //    print("???dlrjek");
            //    StopUltimate();
            //}
            if (distance < 0.3f)
            {
                //print("????????????");
                StopUltimate();
            }


        }

        //if(pc.isAttacking == true)
        //{
        //    pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(skillDir), 9f * Time.deltaTime);

        //}
        if (isUsingUltimate && pc.isAttacking == false)
        {
            FangAnim.SetBool("isMoving", false);
            //float step = ultimateSpeed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, step);
            /* transform.position += dir1 * skillSpeed * Time.deltaTime;
             dir1 = enemy.transform.position - transform.position;
             dir1 = dir1.normalized;*/

            pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(skillDir), 9f * Time.deltaTime);
            SkillDirMake();

            float distance = skillDir.magnitude;
            //if (distance < ultimateDistance)

            skillDir.Normalize();

            finalDir = skillDir * ultimateSpeed;

            if (distance > ultRange)
            {
                StopUltimate();
            }
        }
        //�Ϲ��̵�
        else
        {
            FangAnim.SetBool("isMoving", true);
            Vector3 dirH = transform.right * h;
            Vector3 dirV = transform.forward * v;
            Vector3 dir = dirH + dirV;

            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3);
            
            if(dir.magnitude > 0 && pc.isAttacking == false)
            {
                pang.transform.rotation = Quaternion.Lerp(pang.transform.rotation, Quaternion.LookRotation(dir), 7f * Time.deltaTime);
            }

            //// �̵��ӵ� �������ϰ�
            if (dir.magnitude > 1)
            {
                dir = dir.normalized;
            }

            finalDir = Vector3.Lerp(finalDir, dir, lerpSpeed * Time.deltaTime);

        }

        if (h == 0 && v ==0)
        {
            FangAnim.SetBool("FangUlt", false);
            FangAnim.SetBool("isMoving", false);
        }
        //transform.position += finalDir * moveSpeed * Time.deltaTime;
        rb.linearVelocity = finalDir * moveSpeed; 

    }

    void StartUltimate()
    {
        ultParticle = Instantiate(UltParticle);
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.BGM_FULT);
        isUsingUltimate = true;
        FangAnim.SetBool("FangUlt", true);
        ultEnemy.Add(GetComponent<PlayerController>().nearestEnemyObj);
        nextEnemy = GetComponent<PlayerController>().nearestEnemyObj;

        SkillDirMake();
    }
    public PlayerController pc;
    public bool slowUltStart = false;
    public Vector3 endPos;
    void SlowUltimate()
    {
        ultParticle = Instantiate(UltParticle);
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.BGM_FULT);
        slowUltStart = true;
        FangAnim.SetBool("FangUlt", true);
        endPos = pc.UpdateAttackPosition();
        //StartUltimate();
        //isUsingUltimate = true;
        ////if (currentSkillMoveCount >= maxSkillMoveCount)
        ////{
        ////    StopUltimate();
        ////    return;
        ////}

        ////if (ultEnemy[0] == null)
        ////{
        ////    StopUltimate();
        ////    return;
        ////}
        //skillDir = pc.UpdateAttackDirection() - transform.position;

        //skillDir.y = 0;
        //isUsingUltimate = true;

        //ultEnemy.Add(GetComponent<PlayerController>().nearestEnemyObj);
        //nextEnemy = GetComponent<PlayerController>().nearestEnemyObj;

        //SkillDirMake();
        //SkillDirMake();
    }

    void StopUltimate()
    {
        Destroy(ultParticle);
        FangAnim.SetBool("FangUlt", false);
        isUsingUltimate = false;
        slowUltStart = false;
        finalDir = Vector3.zero;
        currentSkillMoveCount = 0;
        ultEnemy.Clear();
        overlapTime = 0;
       // stop = false;
    }

    void SkillDirMake()
    {

        ultParticle.transform.position = transform.position;
        if (currentSkillMoveCount >= maxSkillMoveCount)
        {
            StopUltimate();
            return;
        }

        if (ultEnemy[0] == null)
        {
            StopUltimate();
            return;
        }

       

        skillDir = ultEnemy[currentSkillMoveCount].transform.position - transform.position;

        
        skillDir.y = 0;
    }

    void UltSkillOnCollisionEnemy()
    {
        //currentSkillMoveCount++;
        GameObject ultHit = Instantiate(UltHitParticle);
        ultHit.transform.position = transform.position;
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, ultRange, 1 <<  LayerMask.NameToLayer("Enemy"));

        //print(hitEnemies.Length);

        if (hitEnemies.Length > 0)
        {
            List<Collider> fillterList = new List<Collider>();

            for(int j = 0; j < hitEnemies.Length; j++)
            {
                bool inEnemy = false;

                for (int i = 0; i < ultEnemy.Count; i++)
                {
                    if (hitEnemies[j].gameObject == ultEnemy[i])
                    {
                        inEnemy = true;
                    }
                    else
                    {
                        Vector3 rayDir = (hitEnemies[j].gameObject.transform.position - transform.position).normalized;
                        Ray ray = new Ray(transform.position, rayDir);
                        RaycastHit hitinfo;

                        if (Physics.Raycast(ray, out hitinfo, ultRange))
                        {
                            if(hitinfo.transform.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                            {
                                //inEnemy = true;

                            }
                        }
                    }
                }

                if(inEnemy == false)
                {
                    fillterList.Add(hitEnemies[j]);
                }
            }

            //print(fillterList.Count);


            if(fillterList.Count > 0)
            {
                Collider nearestEnemy = fillterList[0];

                float closestDistance = Vector3.Distance(transform.position, fillterList[0].transform.position);
                nextEnemy = fillterList[0].gameObject;

                foreach (Collider enemy in fillterList)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                        nextEnemy = enemy.gameObject;
                    }
                }
                SoundManager.Get().PlayEftSound(SoundManager.ESoundType.BGM_FULT);
                ultEnemy.Add(nextEnemy);
                currentSkillMoveCount++;
                SkillDirMake();
            }
            else
            {
                StopUltimate();
            }


            // ���� ����� ���� ã��

        }
        else
        {
            StopUltimate();
        }

    }

    public void DamageAction(int damage)
    {
        hp -= damage;
    }

    //��������
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && slowUltStart)
        {
            slowUltStart = false;
            StartUltimate();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && isUsingUltimate)
        {
            UltSkillOnCollisionEnemy();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("block"))
        {
            //print("durl");
            StopUltimate();
            //stop = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.GameObject.layer == Layermask.nametolayer("enemy") && isUsingUltimate)
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && isUsingUltimate)
        {
             UltSkillOnCollisionEnemy();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && slowUltStart)
        {
            slowUltStart = false;
            StartUltimate();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("block"))
        {
            //print("ddfdsurl");
            StopUltimate();
            //stop = true;
        }
    }


    public float overlapTime = 0;
    float overlapLimitTime = 0.2f;
    private void OnTriggerStay(Collider other)
    {
        if (isUsingUltimate == false)
        {
            return;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            return;
        }

        if (nextEnemy == other.gameObject)
        {
            overlapTime += Time.deltaTime;

            if (overlapTime > overlapLimitTime)
            {
                StopUltimate();
            }
        }
        else
        {
            overlapTime = 0;
        }

    }


    
    //���콺�������� �����ؼ�
    //void RotateToMouseCursor()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        Vector3 direction = (hit.point - transform.position).normalized;
    //        direction.y = 0; 

    //        Quaternion lookRotation = Quaternion.LookRotation(direction);
    //        print(direction);
    //        aimIndicatorInstance.transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    //    }
    //}

    //void ShowAimIndicator(bool show)
    //{
    //    if (aimIndicatorInstance != null)
    //    {
    //        aimIndicatorInstance.SetActive(show);

    //        if (show)
    //        {
    //            print("instance ���Ծ�");

    //        }
    //    }
    //}

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
        pos2 = transform.position + Vector3.up*0.8f;
        pos3 = (transform.position + -1 * (Bpos - transform.position)) + Vector3.up * 0.8f;
        pos4 = transform.position + -1 * (Bpos - transform.position) - Vector3.up*1.6f;
        while (true) {
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
        transform.position = new Vector3(transform.position.x,originalPositionY, transform.position.z);
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
    //        GameManager2.Instance.PlayerDied(1);
            
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

    //private void OnCollisionStay(Collision collision)
    //{
    //    if(isUsingUltimate == false)
    //    {
    //        return;
    //    }
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        return;
    //    }

    //    if(nextEnemy == collision.gameObject)
    //    {
    //        overlapTime += Time.deltaTime;

    //        if(overlapTime > overlapLimitTime)
    //        {
    //            StopUltimate();
    //        }
    //    }
    //    else
    //    {
    //        overlapTime = 0;
    //    }
    //}
}