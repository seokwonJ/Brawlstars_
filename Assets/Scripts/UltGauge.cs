using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UltGauge : MonoBehaviour
{
    public Image UltBar;
    public Image Bg;
    public Image Yellow;
    public Image Ult1;
    public float currentGauge;
    public float maxGauge = 100f;
    public GameObject Player;
    public float gainPerHit = 34f;
    // Start is called before the first frame update
    void Start()
    {
        currentGauge = 0f;
       // UpdateGaugeBar();

    }

    // Update is called once per frame
    void Update()
    {

     

        UpdateGaugeBar();

        
    }

    public void GainGauge()
    {
       
        currentGauge += gainPerHit;
        //print("¸Â¾Ò¾î");
        currentGauge = Mathf.Min(currentGauge, maxGauge);
        //print(Yellow.fillAmount);
        //print(111);
        UpdateGaugeBar();
    }
    void UpdateGaugeBar()
    {
        if(Yellow.fillAmount == 1)
        {
            Ult1.enabled = false;
        }
        else
        {
            Ult1.enabled = true;
        }

        if (Yellow != null)
        {
            //UltBar.fillAmount = currentGauge / maxGauge;
            //Yellow.fillAmount = Mathf.Lerp(Yellow.fillAmount, currentGauge / maxGauge, 30f * Time.deltaTime);
            Yellow.fillAmount = currentGauge / maxGauge;
            //print(Yellow.fillAmount + "1");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {


        if (collider.gameObject.layer == LayerMask.NameToLayer("Ebullet"))
        {
           

            GainGauge();
            //UltGauge ultgauage = gameObject.GetComponent<UltGauge>();
            //if(ultgauage != null)
            //{
            //    ultgauage.GainGauge();
            //}
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerMove pm = GetComponent<PlayerMove>();
            if(pm.isUsingUltimate == true)
            {
            GainGauge();

            }
            //UltGauge ultgauage = gameObject.GetComponent<UltGauge>();
            //if(ultgauage != null)
            //{
            //    ultgauage.GainGauge();
            //}
        }
    }
}
