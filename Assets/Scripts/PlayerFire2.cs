using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire2 : MonoBehaviour
{
    public GameObject bulletFactory;
    // 총구
    public GameObject firePos;
    public GameObject Player2;
    public GameObject enemy;
    public List<Image> listAttackBar;
    public Image AttackBar;
    public PlayerController2 pc;
    public float maxCharge = 3.0f;
    public float chargeRate = 1.0f;
    public float currentCharge;
    public bool canFire = false;
    public GameObject pang;

    public Color nomalColor;
    public Color fullColor;
    // Start is called before the first frame update
    void Start()
    {
        pc = gameObject.GetComponent<PlayerController2>();

        //currentCharge = 0;
        UpdateChargeBar();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 게임중이 아니라면 함수를 나가자.
        if (GameManager.instance.isPlaying == false) return;
        if (currentCharge < 1)
        {
            currentCharge += chargeRate * Time.deltaTime;
            UpdateChargeBar();
        }
        if (currentCharge > (1f / listAttackBar.Count))
        {
            canFire = true;
        }
    }
    public PlayerMove2 pm2;
    public void FastAttack()
    {

        if (currentCharge < (1f / maxCharge))
        {
            return;
        }

        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_PATTACK);
        GameObject bullet2 = Instantiate(bulletFactory);

        if (pc.nearestEnemyObj == null)
        {
            bullet2.transform.position = firePos.transform.position;

            pang.transform.forward = pm2.transform.TransformDirection(pm2.AttackSpot());
            Vector3 dir = pm2.transform.TransformDirection(pm2.AttackSpot());
            dir.y = 0;
            firePos.transform.forward = dir;


        }
        else
        {
                bullet2.transform.position = firePos.transform.position;

                pang.transform.forward = (pc.nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position);
                firePos.transform.forward = (pc.nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position).normalized;
            // float zscaleValue = Mathf.Clamp(dir.magnitude, 0f, 10f);
            //transform.localScale = new Vector3(1, 1, 10);
            //transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
        //if (pc.nearestEnemyObj == null)
        //{
        //    pang.transform.forward = (pc.UpdateAttackDirection());
        //    firePos.transform.forward = pc.UpdateAttackDirection();



        //    bullet2.transform.position = firePos.transform.position;

        //}
        //else
        //{
        //    pang.transform.forward = (pc.nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position);
        //    firePos.transform.forward = (pc.nearestEnemyObj.transform.position + Vector3.up * 0.8f - transform.position).normalized;


        //    bullet2.transform.position = firePos.transform.position;
        //}
        bullet2.transform.forward = firePos.transform.forward;

        currentCharge = currentCharge - (1 / maxCharge);


    }


    public void Attack()
    {

        if (currentCharge < maxCharge / 3)
        {
            //여기 UI넣으면됨
            //return;
        }

        firePos.transform.forward = pm2.transform.TransformDirection(pm2.AttackSpot());
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_PATTACK);


        GameObject bullet2 = Instantiate(bulletFactory);
        bullet2.transform.forward = firePos.transform.forward;
        bullet2.transform.position = firePos.transform.position;
        currentCharge = currentCharge - (1 / maxCharge);

    }
    void UpdateChargeBar()
    {
        // currentCharge가 0~1로 차오른다
        //1번은 0~0.3333
        //2번은? 0.333~0.666
        //3번은? 0.666~1
        for (int i = 0; i < listAttackBar.Count; i++)
        {
            float myAlpha = ((currentCharge - ((1f / (float)listAttackBar.Count) * i)) / (1f / (float)listAttackBar.Count));

            myAlpha = Mathf.Clamp(myAlpha, 0f, 1f);

            listAttackBar[i].fillAmount = myAlpha;
            if (myAlpha == 1)
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
