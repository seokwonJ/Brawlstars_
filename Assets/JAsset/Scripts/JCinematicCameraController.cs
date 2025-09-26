using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CinematicCameraController : MonoBehaviour
{
    public Camera bossCamera;         // 보스를 비추는 첫 번째 카메라
    public Camera playerCamera;       // 플레이어를 따라다니는 두 번째 카메라
    public Transform player;          // 플레이어의 Transform
    public Transform cameraEndPos;    // 첫 번째 카메라의 최종 위치
    public float cameraMoveDuration = 5f; // 첫 번째 카메라가 이동하는 데 걸리는 시간

    private float realTime = 0;
    private float elapsedTime = 0f;
    private bool isTransitioning = true;
    public Image Brawl;
    public Image Image1;

    private void Awake()
    {
        Image1.enabled = false;
        
    }
    void Start()
    {
        // 초기 설정: 첫 번째 카메라 활성화, 두 번째 카메라 비활성화
        bossCamera.enabled = true;
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_INTRO);
        //playerCamera.enabled = false;
    }

    void Update()
    {

        if (isTransitioning)
        {
            realTime += Time.deltaTime;
            // 첫 번째 카메라가 천천히 이동하여 플레이어를 비추게 함
            elapsedTime += Time.deltaTime * 0.01f;
            float t = Mathf.Clamp01(elapsedTime / cameraMoveDuration);
            bossCamera.transform.position = Vector3.Lerp(bossCamera.transform.position, cameraEndPos.position, t);
            bossCamera.transform.rotation = Quaternion.Lerp(bossCamera.transform.rotation, cameraEndPos.rotation, t);



            // 첫 번째 카메라의 이동이 완료되면 두 번째 카메라로 전환
            if (realTime >= 5f)
            {
                SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_GO);
                Brawl.enabled = true;
                // countDonw.gameObject 크기를 1.5로 하자.
                Brawl.transform.localScale = Vector3.one * 10f;
                // iTween 을 이용해서 움직임을 주자
                Hashtable hash = iTween.Hash(
                    "scale", Vector3.one,
                    "time", 0.5f,
                    "easetype", iTween.EaseType.easeOutBounce,
                    "oncompletetarget", gameObject,
                    "oncomplete", nameof(OnComplete));
                iTween.ScaleTo(Brawl.gameObject, hash);
                isTransitioning = false;
                bossCamera.enabled = false;
                playerCamera.enabled = true;
            }
        }


    }
    void OnComplete()
    {
        StartCoroutine(GoEndingScene());
    }

    IEnumerator GoEndingScene()
    {
        yield return new WaitForSeconds(1f);
        Brawl.enabled = false;
        Image1.enabled = true;
    }
}