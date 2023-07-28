using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEffect : MonoBehaviour
{
    private Animator effectAnimator;
    private float effectTime;
    // Start is called before the first frame update
    void Start()
    {
        effectAnimator = GetComponent<Animator>();
        effectAnimator.Play("Explosion");
        effectTime = effectAnimator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, effectTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
