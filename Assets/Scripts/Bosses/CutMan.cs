using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutMan : Enemy
{
    [SerializeField] private GameObject rollingCutterPrefab;
    [SerializeField] private GameObject rollingCutterPosition;
    private bool haveRollingCutter = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (haveRollingCutter)
        {
            ThrowRollingCutter();
        }

    }

    void ThrowRollingCutter()
    {
        Instantiate(rollingCutterPrefab, rollingCutterPosition.transform.position, rollingCutterPosition.transform.rotation);
        haveRollingCutter = false;
    }

    public void PickUpRollingCutter()
    {
        haveRollingCutter = true;
    }
}
