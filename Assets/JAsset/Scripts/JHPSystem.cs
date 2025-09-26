using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JHPSystem : MonoBehaviour
{
    // 최대 HP
    public float maxHP = 10;
    public GameObject Player;
    // 현재 HP
    public float currHP = 0;
    // HP bar UI
    public Image hpBar;
    public Image slowhpBar;
    public Text HpText;
    public TextMesh DamageNum;
    public JHealthBarRotation JHBR;
    public PlayerMove pm;

    public GameObject Skin;
    public GameObject SkinBar;
    public bool isdie = false;
    // Start is called before the first frame update
    void Start()
    {
        // 현재 HP를 최대 HP로 하자.

        currHP = maxHP;
        DamageNum.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.H))
        {
            currHP = maxHP;
        }
        // HP bar를 갱신하자.
        // 0 ~ 1
        //hpBar.fillAmount = currHP / maxHP;
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, currHP / maxHP, 10f * Time.deltaTime);
        slowhpBar.fillAmount = Mathf.Lerp(slowhpBar.fillAmount, currHP / maxHP, 2f * Time.deltaTime);


        HpText.text = ((int)(currHP)).ToString();
        //if(Player.transform.position != LayerMask.NameToLayer("Ebullet").transform.position && )
        //if(nodam == true && noattack == false)
        //{
        //    currTime += Time.deltaTime;
        //    if (currTime > 2f)
        //    {
        //        currentCharge = currentCharge + maxCharge / 5 * 0.8f;
        //        currTime = 0;
        //        print("나의 체력 : " + currentCharge);
        //    }
        //}
       
    }

    //체력회복
    //public float currTime = 0;
    //private void OnTriggerEnter(Collider other)
    //{
    //    //if(Player.layer != LayerMask.NameToLayer("Ebullet"))
    //    //{
    //    //    //공격하지않았을때
    //    //    if()
    //    //    {
    //    //        currTime += Time.deltaTime;
    //    //        if (currTime > 2f)
    //    //        {
    //    //            currentCharge = currentCharge + maxCharge / 5 * 0.8f;
    //    //            currTime = 0;
    //    //            print("나의 체력 : " + currentCharge);
    //    //        }
    //    //    }
    //    //}
    //}

    //bool attack = false;
    //void attack()
    //{
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        true;
    //    }

    //}
    // 현재 HP를 증감하는 함수
    public void UpdateHP(float value)
    {
        if (isdie) return;
    
        if(DamageNum != null)
        {


           
            TextMesh Ga = Instantiate(DamageNum);


            if (gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (value < 0)
                {
                    if (gameObject.tag == "Shelly")
                    {
                        Animator animator = GetComponent<Animator>();
                        animator.SetTrigger("Attacked");
                    }
                    else
                    {
                        JHBR.TwinkleBar();
                    }
                    Ga.transform.position = transform.position;
                    Ga.text = "" + (int)value;
                    Ga.GetComponent<JDamageNum>().SetParent(gameObject);
                    Ga.color = Color.red;
                }
                else
                {

                    if (currHP + value > maxHP && value > 0)
                    {
                        Ga.transform.position = transform.position;
                        Ga.text = "" + (int)value;
                        Ga.GetComponent<JDamageNum>().SetParent(gameObject);
                        Ga.color = Color.green;
                        // 현재 HP 를 max 로 셋팅
                        currHP = maxHP;
                        return;

                    }


                    Ga.transform.position = transform.position;
                    Ga.text = "" + (int)value;
                    Ga.GetComponent<JDamageNum>().SetParent(gameObject);
                    Ga.color = Color.green;
                }
            }
            else
            {
                JHBR.TwinkleBar();
                Ga.transform.position = transform.position;
                Ga.text = "" + (int)value;
                Ga.GetComponent<JDamageNum>().SetParent(gameObject);
            }
        }
        // 현재 HP value 더하자.
        currHP += (int)value;
        // 현재 HP 가 max보다 커지면

        // 현재 HP가 0이면 

        if (currHP <= 0 && gameObject.layer == LayerMask.NameToLayer("Player") && isdie == false && gameObject.tag == "Fang")
        {
            GameManager2.Instance.PlayerDied(1);
            //pm = GetComponent<PlayerMove>();
            //pm.isAlive = false;
            StartCoroutine("deadd");
            // 파괴하자.
            //Destroy(gameObject);
        }
        else if (currHP <= 0 && gameObject.layer == LayerMask.NameToLayer("Player") && isdie == false && gameObject.tag == "Poco")
        {
            GameManager2.Instance.PlayerDied(2);
            //pm = GetComponent<PlayerMove>();
            //pm.isAlive = false;
            StartCoroutine("deadd");
            // 파괴하자.
            //Destroy(gameObject);
        }
        else if (currHP <= 0 && gameObject.layer == LayerMask.NameToLayer("Player") && isdie == false && gameObject.tag == "Shelly")
        {
            //pm = GetComponent<PlayerMove>();
            //pm.isAlive = false;
            gameObject.SetActive(false);
            // 파괴하자.
            //Destroy(gameObject);
        }
        else if (currHP <= 0 && gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
        }
    }
    //public UltGauge ug;
    //public void UltGauge()
    //{
    //    ug = gameObject.GetComponent<UltGauge>();
    //    ug.GainGauge();
    //}

    //public void NoAttack()
    //{
    //    if (currHP != maxHP)
    //    {

    //    }
    //    void OnTriggerEnter(Collider other)
    //    {
    //        if (other.Player.layer != LayerMask.NameToLayer("Player"))
    //        {
                
                
    //        }
    //        Destroy(gameObject);
    //    }
    //}
    
    public void hpplus()
    {
        currHP += maxHP/500;
    }

    public float HPReturn()
    {
        return currHP;
    }

    IEnumerator deadd()
    {
        isdie = true;
        Skin.SetActive(false);
        SkinBar.SetActive(false);
        yield return new WaitForSeconds(7.0f);
        Skin.SetActive(true);
        SkinBar.SetActive(true);
        currHP = maxHP;
        isdie = false;
    }
}
