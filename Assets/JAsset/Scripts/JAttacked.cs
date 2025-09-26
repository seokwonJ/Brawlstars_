using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAttacked : MonoBehaviour
{
    public SkinnedMeshRenderer[] mr;

    public Color nowColor; // 변경할 색
    Color startColor = Color.white;
    public float duration = 0.5f; // 색상이 변하는데 걸리는 시간
    float elapsedTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        nowColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        RoundCheckColor();
        ChangeColor();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("bullet"))
        {
            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].material.color = Color.red;
            }
            elapsedTime = 0;
            startColor = Color.red;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ebullet"))
        {
            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].material.color = Color.red;
            }
            elapsedTime = 0;
            startColor = Color.red;
        }
        //궁극기맞으면
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMove pm = other.gameObject.GetComponent<PlayerMove>();
            if (pm == null) ;
            else if (pm.isUsingUltimate == true)
            {
                for (int i = 0; i < mr.Length; i++)
                {
                    mr[i].material.color = Color.red;
                }
                elapsedTime = 0;
                startColor = Color.red;
            }
        }

    }
    void RoundCheckColor()
    {
        if (GameManager.instance.round == 2)
        {
            if (nowColor == Color.magenta) return;
            nowColor = Color.magenta;
            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].material.color = nowColor;
            }
        }
        if (GameManager.instance.round == 3)
        {
            if (nowColor == Color.red) return;
            nowColor = Color.red;
            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].material.color = nowColor;
            }
        }
        if (GameManager.instance.round == 4)
        {
            if (nowColor == Color.yellow) return;
            nowColor = Color.yellow;
            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].material.color = nowColor;
            }
        }
        if (GameManager.instance.round == 5 && nowColor != Color.black)
        {
            if (nowColor == Color.black) return;
            nowColor = Color.black;
            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].material.color = nowColor;
            }
        }

    }

    public void ChangeColor()
    {
        for (int i =0;i < mr.Length;i++)
        {
            mr[i].material.color = Color.Lerp(startColor, nowColor, elapsedTime / duration);
        }
        
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 1)
        {
            elapsedTime = 0;
            startColor = nowColor;
        }
        // 최종 색상을 정확히 하얀색으로 설정
    }
}
