using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JCameraShake : MonoBehaviour
{
    Vector3 originalPos;
    Vector3 originalPos2;
    Vector3 dir;
    public GameObject PY;

    private void Start()
    {
        PY = gameObject;
    }

    public void StartShake(float duration, float magnitude, bool isRand, Vector3 pos)
    {
        if (isRand)
        {
            originalPos2 = PY.transform.position;
            StartCoroutine(Shake(duration, magnitude, isRand));
  
        }
        else
        {
            dir = transform.position - pos;
            dir.Normalize();
            StartCoroutine(Shake(duration, magnitude, isRand));
        }
    }

    public IEnumerator Shake(float duration, float magnitude, bool isRand)
    {

        originalPos = transform.position;
        //shaking = true;

        float currentTime = 0;

        //조건
        while (currentTime < duration)
        {
            //조건에 충족하면 실행 내용
            if (isRand)
            {
                transform.position = originalPos + Random.insideUnitSphere * magnitude + (PY.transform.position - originalPos2);
            }
            else
            {
                dir.y = 0;
                transform.position = originalPos + dir * magnitude;

                dir = -dir;
            }
            
            currentTime += Time.deltaTime;

            //한 프레임 기다린다
            yield return null;

        }
        if (isRand)
        {
            transform.position = originalPos + (PY.transform.position - originalPos2);
        }
        else
        {
            transform.position = originalPos;
        }
            

        //shaking = false;
    }
}