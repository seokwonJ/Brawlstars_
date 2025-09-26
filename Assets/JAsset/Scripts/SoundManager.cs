using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 브를스타즈
    // enum, 열거형
    public enum ESoundType
    {
        // 처음 시작할 때
        EFT_INTRO,
        // 출발 할 때
        EFT_GO,
        // 팽 공격할 때
        EFT_P1Attack,
        // 보스가 돌진할 때
        EFT_BDASH,
        // 원거리 적이 공격할 때
        EFT_E3ATTACK,
        // 폭탄 터졌을 때
        EFT_BOMB,
        // 적이 강화되었을 때
        EFT_ENEMYPU,
        // 적이 근접 공격할 때
        EFT_EATTACK,
        // 이겼을 때
        EFT_WIN,
        // 로켓 발사 했을 때
        EFT_STARTROCKET,
        // 보스가 레이저 발사할 때
        EFT_RAISER,
        // 팽 궁극기
        BGM_FULT,
        

        // 포코 공격
        EFT_PATTACK,
        // 포코 궁극기
        EFT_PULT,
        // 쉘리 공격
        EFT_SATTACK,
        // 가젯 효과음
        EFT_GAGET,
        // 로봇 죽음음
        EFT_ROBOTDIE

    }

    public enum EBgmType
    {
        BGM_TITLE,
        BGM_INGAME,
        BGM_WARNING,
    }


    // 나를 담을 static 변수
    static SoundManager instance;
    public static SoundManager Get()
    {
        // 만약에 instance 가 null 이라면
        if (instance == null)
        {
            // soundManager Prefab을 읽어오자

            GameObject soundManagerFactory = Resources.Load<GameObject>("SoundManager");
            // SoundManager 공장에서 SoundManager를 만들자.
            GameObject soundManager = Instantiate(soundManagerFactory);
        }


        return instance;
    }


    // audiosource
    public AudioSource eftAudio;
    public AudioSource bgmAudio;

    public AudioClip[] eftAudios;
    public AudioClip[] bgmAudios;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 씬 전환이 되도 게임 오브젝트를 파괴하고 싶지않다.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // effectSound Play 하는 함수
    public void PlayEftSound(ESoundType idx)
    {
        int audioIdx = (int)idx;
        eftAudio.PlayOneShot(eftAudios[audioIdx]);
    }

    // bgm Sound
    public void PlayBgmSound(EBgmType idx)
    {
        int bgmIdx = (int)idx;
        // 플레이할 AudioClip을 설정
        bgmAudio.clip = bgmAudios[bgmIdx];
        bgmAudio.Play();

    }

    public void StopBgmSound()
    {
        bgmAudio.Stop();
    }

    public void AudioSourceEtc()
    {

        // 일시 정지
        bgmAudio.Pause();
        // 완저 멈춤
        bgmAudio.Stop();
        // 현재 실행되고 있느 ㄴ시간
        float currTime = bgmAudio.time;
        // 시간 건너뛰기
        bgmAudio.time += 10;

    }
}
