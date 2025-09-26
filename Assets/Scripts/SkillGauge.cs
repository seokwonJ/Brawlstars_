using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGauge : MonoBehaviour
{
    public Image skill1;
    public Image skill2;
    public PlayerController pc;
    public Text skillCount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkill();
    }

    void UpdateSkill()
    {
        skillCount.text = pc.skillUsesLeft.ToString();
        //print(pc.skillUsesLeft);
        if(pc.skillUsesLeft == 0)
        {
            skill1.enabled = false;
        }
    }
}

