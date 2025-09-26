using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UltGauge2 : MonoBehaviour
{
    public Image UltBar;
    public Image Bg;
    public Image Yellow;
    public Image Ult1;
    public float currentGauge;
    public float maxGauge = 100f;
    public GameObject Player;
    public float gainPerHit = 16f;
    // Start is called before the first frame update
    void Start()
    {
        currentGauge = 0f;

    }

    // Update is called once per frame
    void Update()
    {



        UpdateGaugeBar();


    }

    public void GainGauge()
    {

        currentGauge += gainPerHit;
        currentGauge = Mathf.Min(currentGauge, maxGauge);

        UpdateGaugeBar();
    }
    void UpdateGaugeBar()
    {
        if (Yellow.fillAmount == 1)
        {
            Ult1.enabled = false;
        }
        else
        {
            Ult1.enabled = true;
        }

        if (Yellow != null)
        {
            Yellow.fillAmount = currentGauge / maxGauge;

        }
    }

    private void OnTriggerEnter(Collider collider)
    {


        if (collider.gameObject.layer == LayerMask.NameToLayer("Ebullet"))
        {


            GainGauge();

        }
        //if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //{
        //    PlayerMove2 pm = GetComponent<PlayerMove2>();
        //    if (pm.isUsingUltimate == true)
        //    {
        //        GainGauge();

        //    }

        //}
    }
}
