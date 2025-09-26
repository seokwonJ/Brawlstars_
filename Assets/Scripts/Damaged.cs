using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaged : MonoBehaviour

{
    public float duration = 0.5f;
    Color startColor = Color.white;
    public Color nowColor;
    public SkinnedMeshRenderer[] mr;
    public GameObject Player;
    public JHPSystem Js;
    float elapsedTime = 0f;
    float nowHp;

    // Start is called before the first frame update
    void Start()
    {
        Js = GetComponent<JHPSystem>();
        nowColor = Color.white;
        nowHp = Js.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Js = GetComponent<JHPSystem>();
        //print(nowHp);
        // print("JS" + Js.currHP);
        //RoundCheckColor();
        ChangeColor();

        if (Js.currHP < nowHp)
        {
            print("여까지됬다" + Js.currHP);
            nowHp = Js.currHP;
            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].material.color = Color.red;
            }
            elapsedTime = 0;
            startColor = Color.red;
        }
        //if(currHP = maxHP -currHP).length 
    }


    public void ChangeColor()
    {
        for (int i = 0; i < mr.Length; i++)
        {
            mr[i].material.color = Color.Lerp(startColor, nowColor, elapsedTime / duration);
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime > 1)
        {
            elapsedTime = 0;
            startColor = nowColor;
        }
    }
}
