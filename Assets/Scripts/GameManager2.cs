using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{

    public static GameManager2 Instance;

    public GameObject player1;
    public GameObject player2;
    public Camera camera1;
    public Camera camera2;
    public Text respawnText;
    public Text respawnText2;
    
    public float deathCamSwitchDelay = 2f;
    public float respawnDelay = 12f;
    public PlayerMove pm;

    public Image fadeOverlay; // 화면을 덮는 UI 이미지

    public Image fadeOverlay2; // 화면을 덮는 UI 이미지

    private void Awake()
    {
        
        Instance = this;

    }

    public void PlayerDied(int playerID)
    {
        if (playerID == 1)
        {
            //print(111);
            StartCoroutine(SwitchCamera1());
        }
        else if (playerID == 2)
        {
            StartCoroutine(SwitchCamera2());
        }
    }

    private IEnumerator SwitchCamera1()
    {
        //print(222);
        yield return new WaitForSeconds(deathCamSwitchDelay);
        fadeOverlay.enabled = true;
        //yield return null;


        //StartCoroutine(FadeToGray1());

        // 화면 옅은 회색으로 전환

        Vector3 nowOffset = camera1.GetComponent<CameraClamp>().offset;
        camera1.GetComponent<CameraClamp>().enabled = false;
        camera1.GetComponent<CameraClamp2>().enabled = true;
        camera1.GetComponent<CameraClamp2>().offset = nowOffset;
        //10초 카운트다운
        respawnText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        respawnText.gameObject.SetActive(false);

        fadeOverlay.enabled = false;
        


        camera1.GetComponent<CameraClamp>().enabled = true;
        camera1.GetComponent<CameraClamp2>().enabled = false;
        camera1.GetComponent<CameraClamp>().offset = nowOffset;



        // 플레이어 부활
        Vector3 respawnPosition = new Vector3(-0.82f, -4.64f, 11.79f); // 부활 위치를 적절히 설정하세요.
        pm = player1.GetComponent<PlayerMove>();
        //deadPlayer.Respawn(respawnPosition);

        //toCamera.enabled = false;
        //fromCamera.enabled = true;

        // 회색 필터 제거
        //StartCoroutine(RemoveGrayFilter());
    }


    private IEnumerator SwitchCamera2()
    {
        print(222);
        fadeOverlay2.enabled = true;
        yield return new WaitForSeconds(deathCamSwitchDelay);
        //yield return null;


        //StartCoroutine(FadeToGray2());
        // 화면 옅은 회색으로 전환


        Vector3 nowOffset = camera2.GetComponent<CameraClamp>().offset;
        camera2.GetComponent<CameraClamp>().enabled = false;
        camera2.GetComponent<CameraClamp2>().enabled = true;
        camera2.GetComponent<CameraClamp2>().offset = nowOffset;
        //10초카운트다운
        respawnText2.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);

        respawnText2.gameObject.SetActive(false);

        fadeOverlay2.enabled = false;



        camera2.GetComponent<CameraClamp>().enabled = true;
        camera2.GetComponent<CameraClamp2>().enabled = false;
        camera2.GetComponent<CameraClamp>().offset = nowOffset;



        // 플레이어 부활
        Vector3 respawnPosition = new Vector3(-0.82f, -4.64f, 11.79f); // 부활 위치를 적절히 설정하세요.
        pm = player1.GetComponent<PlayerMove>();
        //deadPlayer.Respawn(respawnPosition);

        //toCamera.enabled = false;
        //fromCamera.enabled = true;

        // 회색 필터 제거
        //StartCoroutine(RemoveGrayFilter());
    }
    //private IEnumerator FadeToGray1()
    //{
    //    print("im gray");
    //    fadeOverlay.enabled = true;
        
    //    print("im gray2");

    //    yield return new WaitForSeconds(12f);
    //    fadeOverlay.enabled = false;
    //    //float duration = 1f;
    //    //float elapsed = 0f;

    //    //Color startColor = new Color(0, 0, 0, 0);
    //    //Color endColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    //    //while (elapsed < duration)
    //    //{
    //    //    elapsed += Time.deltaTime;
    //    //    fadeOverlay.color = Color.Lerp(startColor, endColor, elapsed / duration);
    //    //}
    //    yield return null;
    //}
    //private IEnumerator FadeToGray2()
    //{
    //    print("im gray");
    //    fadeOverlay2.enabled = true;
    //    print("im gray2");

    //    yield return new WaitForSeconds(12f);
    //    //float duration = 1f;
    //    //float elapsed = 0f;

    //    //Color startColor = new Color(0, 0, 0, 0);
    //    //Color endColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    //    //while (elapsed < duration)
    //    //{
    //    //    elapsed += Time.deltaTime;
    //    //    fadeOverlay.color = Color.Lerp(startColor, endColor, elapsed / duration);
    //    //}

    //    fadeOverlay2.enabled = false;
    //    yield return null;
    //}
    //private IEnumerator RemoveGrayFilter()
    //{
    //    fadeOverlay.enabled = false;
    //    //float duration = 1f;
    //    //float elapsed = 0f;

    //    //Color startColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    //    //Color endColor = new Color(0, 0, 0, 0);

    //    //while (elapsed < duration)
    //    //{
    //    //    elapsed += Time.deltaTime;
    //    //    fadeOverlay.color = Color.Lerp(startColor, endColor, elapsed / duration);
    //    //}
    //    yield return null;
    //}
}
