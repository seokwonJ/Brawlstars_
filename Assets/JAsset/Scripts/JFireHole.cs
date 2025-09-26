using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JFireHole : MonoBehaviour
{
    public int damage = 10;          // 데미지 값
    public float duration = 10f;     // 함정 지속 시간
    private Collider fireCollider;
    public float damageTime = 2f;
    public float currTime = 0f;

    void Start()
    {
        fireCollider = GetComponent<Collider>();
        StartCoroutine(ActivateTrap());
        currTime = 2f;
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_BOMB);
    }

    IEnumerator ActivateTrap()
    {
        // 함정을 10초간 활성화
        fireCollider.enabled = true;
        yield return new WaitForSeconds(duration);
        fireCollider.enabled = false;
        Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        currTime += Time.deltaTime;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && currTime > damageTime)
        {

            // 플레이어에게 데미지 주기
            //PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            //if (playerHealth != null)
            //{
            //    playerHealth.TakeDamage(damage);
            //}
            other.GetComponent<JHPSystem>().UpdateHP(-400);
            //print("damage");
            currTime = 0;
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        currTime = 2f;
    }
 
}
