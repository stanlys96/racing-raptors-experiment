using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSceneManager : MonoBehaviour
{
    public GameObject readySetGoUI;
    private float timeSinceLastStart = Mathf.Infinity;
    private float instantiateWaitTime = 2f;
    private bool alreadyInstantiated = false;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastStart += Time.deltaTime;
        if (timeSinceLastStart > instantiateWaitTime)
        {
            if (!alreadyInstantiated)
            {
                alreadyInstantiated = true;
                Instantiate(readySetGoUI);
            }
        }
    }
}
