using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JGameEndingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_WIN);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
