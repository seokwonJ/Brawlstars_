using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGauge2 : MonoBehaviour
{
    
    public Image skill1;
    public Image skill2;
    public PlayerMove2 pc2;
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
        skillCount.text = pc2.skillUsesLeft.ToString();

        //print(pc.skillUsesLeft);
        if (pc2.skillUsesLeft == 0)
        {
            skill1.enabled = false;
        }
    }
}

