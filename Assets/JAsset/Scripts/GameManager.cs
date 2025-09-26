using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float angryTime = 20;
    public float playTime = 0;

    public Text PlayTimeText;
    public Text AngryTimeText;
    public Text AngryState;
    public Text BigStateText;
    public float bossHp;
    public float maxBossHp;
    public Text bossHpText;
    public Image bossHpBar;
    public GameObject Boss;

    public GameObject player3;
    bool P3Die=false;


    // Start is called before the first frame update
    public Text countDown;

    //현재시간을 담을 변수
    float currTime = 0;

    // 현재 게임이 시작 되었는지
    public bool isPlaying = false;

    // 이전 second 값
    int lastSecond = 0;
    bool start = false;


    string timeString;
    int minutes;
    int seconds;

    int Pminutes;
    int Pseconds;

    public int round = 0;

    
    private void Awake()
    {
        // 만약에 instance에 값이 없다면
        if (instance == null)
        {
            // instance에 값을 셋팅하자
            instance = this;
        }

        // 그렇지 않다면
        else
        {
            // 나의 게임오브젝트를 파괴하자.
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
        SoundManager.Get().StopBgmSound();
    }

    // Update is called once per frame
    void Update()
    {

        CountDown();
        if (!isPlaying) return;
        GameEnd();
        GameLevel();

        if (P3Die == false && player3.GetComponent<JHPSystem>().currHP <= 0)
        {
            StartCoroutine("DIeP");
        } 

    }

    void CountDown()
    {
        // 게임중이라면 함수를 나가자.
        if (isPlaying) return;


        // 1. 시간을 흐르게 하자.
        currTime += Time.deltaTime;
        // 2. 흐른 시간을 countDown에 셋팅하자.
        int second = (int)(6 - currTime);

        // 만약에 second가 lastSecond 와 다르다면
        if (second != lastSecond)
        {
            lastSecond = second;
        }



        // 만약에 second가 0보다 크다면
        if (second > 0)
        {
            // second 값을 보여주자.
            //countDown.text = second.ToString();
        }
        // 그렇지 않고 scond가 0이라면 
        else if (second == 0)
        {
            // Start를 보여주자
            //countDown.text = "Start!!";
        }
        // 그렇지 않으면 ( 0보다 작을 때)
        else if(start == false)
        {
            //countDown을 비활성화 하자.
            //countDown.gameObject.SetActive(false);
            //countDown.enabled = false;

            // 게임 중으로 설정하자
            start = true;
            isPlaying = true;
        }
    }

    private void GameLevel()
    {
        angryTime -= Time.deltaTime;
        playTime += Time.deltaTime;

        minutes = Mathf.FloorToInt(angryTime / 60F);
        seconds = Mathf.FloorToInt(angryTime % 60F);

        Pminutes = Mathf.FloorToInt(playTime / 60F);
        Pseconds = Mathf.FloorToInt(playTime % 60F);

        timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        bossHp = Boss.GetComponent<JHPSystem>().currHP;
        maxBossHp = Boss.GetComponent<JHPSystem>().maxHP;



        AngryTimeText.text = "보스가 되기까지: " + timeString;
        PlayTimeText.text = string.Format("{0:00}:{1:00}", Pminutes, Pseconds);

        bossHpText.text = bossHp / maxBossHp * 100 + "%";
        bossHpBar.fillAmount = (bossHp / maxBossHp);

        if (angryTime <= 0)
        {
            angryTime = 40f;
            round += 1;
            SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_ENEMYPU);
            if (round == 2)
            {
                StartCoroutine(WarningText(AngryState.text, AngryState.color));
                AngryState.text = "격분한";
                AngryState.color = Color.red;
                
            }
            if (round == 3)
            {
                StartCoroutine(WarningText(AngryState.text, AngryState.color));
                AngryState.text = "승천한";
                AngryState.color = Color.yellow;
            }
            if (round == 4)
            {
                StartCoroutine(WarningText(AngryState.text, AngryState.color));
                AngryState.text = "천하무적";
                AngryState.color = Color.black;
            }
            if (round == 5)
            {
                StartCoroutine(WarningText(AngryState.text, AngryState.color));
                angryTime = 300f;
            }
        }
    }

    private void GameEnd()
    {
        if (Boss == null)
        {
            SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_ROBOTDIE);
            SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_BOMB);
            SoundManager.Get().StopBgmSound();
            bossHp = 0;
            isPlaying = false;
            StartCoroutine(GoEndingScene());

        }

    }
    IEnumerator GoEndingScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ClearScene");
    }


    IEnumerator WarningText(string text, Color color)
    {

        BigStateText.enabled = true;
        BigStateText.text = text + " 보스!";
        BigStateText.color = color;

        float WEndTime = 5f;
        float WcurrTime = 0;

        // countDonw.gameObject 크기를 1.5로 하자.
        BigStateText.transform.localScale = Vector3.zero;
        // iTween 을 이용해서 움직임을 주자
        Hashtable hash = iTween.Hash(
            "scale", Vector3.one,
            "time", 0.5f,
            "easetype", iTween.EaseType.easeInBounce,
            "oncompletetarget", gameObject,
            "oncomplete", nameof(OnComplete));
        iTween.ScaleTo(BigStateText.gameObject, hash);



        yield return new WaitForSeconds(3f);


        //BigStateText.transform.localScale = Vector3.one;
        // iTween 을 이용해서 움직임을 주자
        BigStateText.transform.localScale = Vector3.one;
        Hashtable hash2 = iTween.Hash(
            "scale", Vector3.zero,
            "time", 0.5f,
            "easetype", iTween.EaseType.easeInBounce,
            "oncompletetarget", gameObject);
        iTween.ScaleTo(BigStateText.gameObject, hash2);

        yield return new WaitForSeconds(2f);
        BigStateText.enabled = false;
    }

    void OnComplete()
    {

    }

    IEnumerator DIeP()
    {
        P3Die = true;
        player3.SetActive(false);
        yield return new WaitForSeconds(7.0f);
        player3.SetActive(true);
        player3.GetComponent<JHPSystem>().currHP = player3.GetComponent<JHPSystem>().maxHP;
        player3.GetComponent<JHPSystem>().isdie = false;
        P3Die = false;
    }

}
