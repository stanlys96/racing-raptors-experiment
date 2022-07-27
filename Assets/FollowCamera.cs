using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Camera camera;
    private float cameraDelay = 2f;
    private float cameraDelayCounter = 0f;
    private bool cameraDelayBool = false;

    // Update is called once per frame
    void LateUpdate()
    {
        cameraDelayCounter += Time.deltaTime;
        if (target != null)
        {
            transform.position = target.transform.position;
        }
        if (gameObject.tag == "FollowCamera1")
        {
            GameObject cloud = GameObject.FindGameObjectWithTag("Cloud");
            if (cloud != null)
            {
                if (!cameraDelayBool)
                {
                    cameraDelayBool = true;
                    cameraDelayCounter = 0;
                }
                if (cameraDelayCounter > cameraDelay)
                {
                    target = cloud;
                    camera.rect = new Rect(0.75f, 0, 0.25f, 0.25f);
                }
            } 
            else
            {
                camera.rect = new Rect(0, 0, 0, 0);
            }
        }
    }
}
