using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public static float distanceTravelled = 0.5f;
    public static float aliveDuration = 0.5f;
    public static float scale = 0.08f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(scale, scale, scale);
        Vector3 endPos = transform.position + new Vector3(Random.Range(-distanceTravelled, distanceTravelled), Random.Range(-distanceTravelled, distanceTravelled),
            transform.position.z);
        transform.DOMove(endPos, aliveDuration);
        Invoke("DestroySelf", aliveDuration + 0.1f);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
