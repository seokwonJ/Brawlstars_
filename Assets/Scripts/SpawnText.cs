using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnText : MonoBehaviour
{
    public float currTime = 0;
    public Text respawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        int second = (int)(6 - currTime);
        if(second > 0)
        {
            respawn.text = second.ToString();
        }
        else
        {
            gameObject.SetActive(false);
            currTime = 0;
        }
    }
}
