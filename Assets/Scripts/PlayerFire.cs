using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
public class PlayerFire : MonoBehaviour


{
    //총알공장(Prefab)
    public GameObject bulletFactory;
    // 총구
    public GameObject firePos;
    public GameObject Player;
    public GameObject enemy;
    Vector3 distance;
    //public int iniBulletCnt = 15;
    //public List<GameObject> magazine = new List<GameObject>();
    public List<Image> listAttackBar;

    public Image AttackBar;

    public PlayerController pc;
    public float maxCharge = 3.0f;
    public float chargeRate = 1.0f;
    public float currentCharge;
    public bool canFire = false;

    public Color nomalColor;
    public Color fullColor;
    public Animator FangAnim;



    //public Transform trFirePos;


    // Start is called before the first frame update
    public void Start()
    {
        //for(int i = 0; i < iniBulletCnt; i++)
        //{
        //}
        //GameObject bullet = Instantiate(bulletFactory);

        ////magazine.Add(bullet);
        ////bullet.SetActive(false);
        //bullet.transform.position = firePos.transform.position;
        pc = gameObject.GetComponent<PlayerController>();

        //currentCharge = 0;
        UpdateChargeBar();
    }
    public float currTime = 0;
    // Update is called once per frame
    public void Update()
    {
        // 현재 게임중이 아니라면 함수를 나가자.
        if (GameManager.instance.isPlaying == false) return;

        //if (currentCharge < maxCharge)
        //{
        //    currentCharge += chargeRate * Time.deltaTime;
        //    UpdateChargeBar();
            

        //}
        if(currentCharge < 1)
        {
            currentCharge += chargeRate * Time.deltaTime;
            UpdateChargeBar();
        }
        //if (currentCharge >= maxCharge/3)
        //{
        //    canFire = true;
        //    //currentCharge = maxCharge;
        //}
        if (currentCharge > (1f / listAttackBar.Count))
        {
            canFire = true;
        }

    }


    public GameObject pang;
    // 쏠 수 있는 탄창 갯수
    public void FastAttack()
    {
 

        if (currentCharge < (1f / maxCharge))
        {
            return;
        }


        // // 쏠 수 있는 탄창 갯수를 하나 줄여 0보다 크니?

        //탄창의 0번째를 뺴온다.
        //GameObject bullet = magazine[0];
        // 총알의 위치를 firpos위치로 한다.
        //bullet.transform.position = firePos.transform.position;
        GameObject bullet = Instantiate(bulletFactory);

        
        if (pc.nearestEnemyObj == null )
        {
            pang.transform.forward = (pc.UpdateAttackDirection());
            firePos.transform.forward = pc.UpdateAttackDirection();


            //마우스커서 방향으로 할것
            //bullet.transform.position = firePos.transform.position;
            bullet.transform.position = firePos.transform.position;

        }
        else
        {
            pang.transform.forward = (pc.nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position);
            firePos.transform.forward = (pc.nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position).normalized;
           // print(pc.nearestEnemyObj.transform.position);
         
            bullet.transform.position = firePos.transform.position;
        }
        bullet.transform.forward = firePos.transform.forward;
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_P1Attack);
        //currentCharge = currentCharge - maxCharge / 3;
        currentCharge = currentCharge - (1 / maxCharge);
        //bullet.transform.forward = firePos.transform.forward;
        FangAnim.SetTrigger("Attack");
        ////총알을 활성화한다.
        //bullet.SetActive(true);
        ////탄창의 0번쨰 오브젝트를 뺸다.
        //magazine.RemoveAt(0);

        // 쏠 수 있는 탄창 갯수를 하나 줄여
    }

    public void Attack()
    {
       
        if (currentCharge < (1f / maxCharge))
        {

            //여기 UI넣으면됨
            return;
        }
        //탄창의 0번째를 뺴온다.
        //GameObject bullet = magazine[0];
        //// 총알의 위치를 firpos위치로 한다.
        //bullet.transform.position = firePos.transform.position;

        //bullet.SetActive(true);

        //pc = gameObject.GetComponent<PlayerController>(UpdateatAckDirection);
        firePos.transform.forward = pc.UpdateAttackDirection();
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_P1Attack);
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.forward = firePos.transform.forward;
        bullet.transform.position = firePos.transform.position;
        currentCharge = currentCharge - (1 / maxCharge);
        FangAnim.SetTrigger("Attack");
        //탄창의 0번쨰 오브젝트를 뺸다.
        //magazine.RemoveAt(0);
        //currentCharge = currentCharge - maxCharge / 3;

    }
    void UpdateChargeBar()
    {
        // currentCharge가 0~1로 차오른다
        //1번은 0~0.3333
        //2번은? 0.333~0.666
        //3번은? 0.666~1
        for (int i =0; i < listAttackBar.Count;i++)
        {
            float myAlpha = ((currentCharge - ((1f  / (float)listAttackBar.Count) * i)) /  (1f / (float)listAttackBar.Count));

            myAlpha = Mathf.Clamp(myAlpha, 0f, 1f);

            listAttackBar[i].fillAmount = myAlpha;
            if(myAlpha == 1)
            {
                listAttackBar[i].color = fullColor;

            }
            else
            {
                listAttackBar[i].color = nomalColor;
            }
        }

    }

}
