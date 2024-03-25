using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTParticle : Particle
{
    public static float startScale = 0.1f;
    public static float endScale = 1;
    public static float growDuration = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3 (startScale, startScale, startScale);
        transform.DOScale(endScale, growDuration);
        Invoke("DestroySelf", growDuration + 0.05f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
