using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRocketBomb : MonoBehaviour
{
    GameObject player;
    public GameObject bombFire;
    public float bombSpeed;
    Vector3 dir;

    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 pos4;
    Vector3 pos5;
    Vector3 pos6;
    Vector3 pos7;
    Vector3 pos8;
    Vector3 pos9;
    Vector3 pos10;


    public float explosionDelay = 3.0f;
    public float shakeDuration;
    public float shakeMagnitude;
    public float curr = 0f;
    public float lerpDuration = 3.0f;


    // 폭발효과공장(Prefab)
    public GameObject exploFactory;
    // 연기효과공장(Prefab)
    public GameObject smokeFactory;
    public GameObject firareaFactory;
    GameObject Camera1;
    GameObject Camera2;
    GameObject smoke;

    // Start is called before the first frame update
    void Start()
    {
        Camera1 = GameObject.Find("Main Camera");
        Camera2 = GameObject.Find("Main Camera2");
        int num = Random.Range(0, 3);
        if (num == 0)
        {
            player = GameObject.Find("Player");
        }
        else if (num == 1)
        {
            player = GameObject.Find("Player2");
        }
        else
        {
            player = GameObject.Find("Player3");
        }
        if  (player == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            dir = (player.transform.position + Vector3.down) - transform.position;
            dir.Normalize();
        }
        

        pos1 = transform.position;
        pos2 = transform.position + Vector3.up * 2;
        pos3 = player.transform.position + Vector3.up * 2;
        pos4 = player.transform.position + Vector3.up * -1;

        smoke = Instantiate(smokeFactory);

        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_STARTROCKET);
    }

    // Update is called once per frame
    void Update()
    {
        curr += Time.deltaTime / lerpDuration;
        curr = Mathf.Clamp01(curr);

        pos5 = Vector3.Lerp(pos1, pos2, curr);
        pos6 = Vector3.Lerp(pos2, pos3, curr);
        pos7 = Vector3.Lerp(pos3, pos4, curr);
        pos8 = Vector3.Lerp(pos5, pos6, curr);
        pos9 = Vector3.Lerp(pos6, pos7, curr);
        pos10 = Vector3.Lerp(pos8, pos9, curr);

        dir = pos10 - transform.position;
        transform.position = pos10;

        smoke.transform.position = transform.position;

        smoke.transform.forward = -dir;
        transform.up = dir;
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            GameObject bf = Instantiate(bombFire);
            bf.transform.position = transform.position;
            Explode();
        }
    }

    private void Explode()
    {
        // 여기서 폭발 관련 로직을 추가
        // 예: 폭발 이펙트 생성, 데미지 처리 등

        // 카메라 쉐이크 효과 호출
        JCameraShake cameraShake1 = Camera1.GetComponent<JCameraShake>();
        if (cameraShake1 != null)
        {
            GameObject explo = Instantiate(exploFactory);
            explo.transform.position = transform.position;
            cameraShake1.StartShake(0.3f, 0.8f,true,Vector3.back);
            GameObject FireA = Instantiate(firareaFactory);
            Destroy(FireA, 10f);
            FireA.transform.position = transform.position;
        }
        JCameraShake cameraShake2 = Camera2.GetComponent<JCameraShake>();
        if (cameraShake2 != null)
        {
            GameObject explo = Instantiate(exploFactory);
            explo.transform.position = transform.position;
            cameraShake2.StartShake(0.3f, 0.8f, true, Vector3.back);
            GameObject FireA = Instantiate(firareaFactory);
            Destroy(FireA, 10f);
            FireA.transform.position = transform.position;
        }


        // 폭발 후 폭탄 오브젝트 제거
        Destroy(smoke);
        Destroy(gameObject);
    }
}
