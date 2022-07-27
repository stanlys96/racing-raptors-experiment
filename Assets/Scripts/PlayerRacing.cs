using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerRacing : MonoBehaviour
{
    public float maxSpeed = 1f;
    public float minSpeed = 0.9f;
    public int tokenId;
    public bool canJump = false;
    public GameObject target;
    public bool isMain = false;
    public Text uiTokenId;
    public GameObject cloudGameObject;

    public SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;

    private float dashLength = 0.3f;
    private float dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    private bool onDash = false;
    private float dashSpeed = 1.6f;
    private bool isJumping = false;
    private int index = 0;
    private float changeSeason = 10f;
    private float timeSinceLastChangeSeason = 0;
    public float[] speeds = {};
    private float checkTime = 0f;
    private bool isFinished = false;
    private float[] mainObjectSpeeds = { 1f, 1.05f, 0.95f, 0.94f, 1.1f, 1f, 1.2f };
    private float[] firstPlaceSpeeds = { 0.92f, 0.95f, 1.05f, 1.03f, 1.02f, 0.97f, 1.1f };
    private float[] secondPlaceSpeeds = { 0.95f, 1.05f, 0.92f, 1f, 1.05f, 1.03f, 1.04f };
    private float[] thirdPlaceSpeeds = { 0.95f, 1.03f, 1.1f, 0.94f, 0.95f, 0.98f, 1.01f };
    private float[] fourthPlaceSpeeds = { 1.03f, 1.1f, 0.95f, 0.9f, 0.92f, 0.91f, 0.93f };
    private float[] fifthPlaceSpeeds = { 1.1f, 1.03f, 0.95f, 0.92f, 0.91f, 1.04f, 0.98f };
    private float[] sixthPlaceSpeeds = { 1f, 1.03f, 1f, 0.9f, 0.91f, 0.92f, 1.1f };
    private float[] seventhPlaceSpeeds = { 1.1f, 1.03f, 0.9f, 0.92f, 0.99f, 1.02f, 1f };
    private float[] eighthPlaceSpeeds =  { 1.03f, 1.04f, 0.94f, 0.92f, 1f, 1.03f, 1f };
    private bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (uiTokenId != null)
        {
            uiTokenId.text = tokenId.ToString();
        }
        if (gameObject.tag == "MainObject")
        {
            speeds = seventhPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[0])
        {
            speeds = firstPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[1])
        {
            speeds = secondPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[2])
        {
            speeds = thirdPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[0])
        {
            speeds = fourthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[1])
        {
            speeds = fifthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[2])
        {
            speeds = sixthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[3])
        {
            speeds = seventhPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[4])
        {
            speeds = eighthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.fighters[0])
        {
            gameObject.tag = "Fighter1";
        }
        if (tokenId == APICall.instance.fighters[1])
        {
            gameObject.tag = "Fighter2";
        }

        foreach (KeyValuePair<int, RuntimeAnimatorController> entry in APICall.instance.tokenIdToSprite)
        {
            if (tokenId == entry.Key)
            {
                animator.runtimeAnimatorController = entry.Value;
            }
        }
    }

    IEnumerator DoRotation(float speed, float amount, Vector3 axis)
    {
        isRotating = true;
        float rot = 0f;
        while (rot < amount)
        {
            yield return null;
            float delta = Mathf.Min(speed * Time.deltaTime, amount - rot);
            transform.RotateAround(target.transform.position, axis, delta);
            rot += delta;
        }
        isRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkTime += Time.deltaTime;
        timeSinceLastChangeSeason += Time.deltaTime;
        if (timeSinceLastChangeSeason > changeSeason)
        {
            timeSinceLastChangeSeason = 0f;
            index++;
            if (index == speeds.Length)
            {
                index = 0;
            }
        }
        float speed = isFinished ? 0f : speeds[index];
        //animator.SetBool("isMoving", true);
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    if (dashCoolCounter <= 0 && dashCounter <= 0)
        //    {
        //        if (gameObject.name == "7_0 (1)")
        //        {
        //            animator.SetTrigger("Jump");
        //            activeMoveSpeed = dashSpeed;
        //            dashCounter = dashLength;
        //            onDash = true;
        //        }

        //    }
        //}
        if (tokenId == APICall.instance.fighters[1] && index == 1)
        {
            GameObject targetFighter = GameObject.FindGameObjectWithTag("Fighter1");
            //transform.position = Vector2.MoveTowards(transform.position, targetFighter.transform.position, speed * 1.175f * Time.deltaTime);
            print(Vector2.Distance(transform.position, targetFighter.transform.position));
            if (Vector2.Distance(transform.position, targetFighter.transform.position) < 1f)
            {
                animator.SetTrigger("jump");
            }
            //transform.RotateAround(targetFighter.transform.position, Vector3.one, 1f
            //Vector2 relativePos = targetFighter.transform.position - transform.position;
            //Quaternion desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            //transform.rotation = desiredRotation;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, -50f * Time.deltaTime);
            Vector3 targetDir = transform.position - targetFighter.transform.position;
            float angleRad = Mathf.Atan2(transform.position.y - targetFighter.transform.position.y, transform.position.x - targetFighter.transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            float angle = Vector3.Angle(targetDir, transform.forward);
            //double asd = Math.Atan((targetFighter.transform.position.y - transform.position.y) / (targetFighter.transform.position.x - transform.position.x));
            //float targetAngle = 45;
            float turnSpeed = 5;
            //Vector3 right = targetFighter.transform.position - transform.position;
            //Vector3 forward = Vector3.Cross(right.normalized, Vector3.up);
            //transform.rotation = Quaternion.LookRotation(forward);
            Vector3 diff = targetFighter.transform.position - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angleDeg), turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, rot_z - 90);
            transform.Translate(Vector2.up * speed * 1.125f * Time.deltaTime);
            transform.Translate(Vector2.right * 0.8f * Time.deltaTime);
        } 
        else
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }

        //if (gameObject.name == "7_0 (1)")
        //{
        //    print(activeMoveSpeed);
        //}
        //if (dashCounter > 0)
        //{
        //    dashCounter -= Time.deltaTime;

        //    if (dashCounter <= 0)
        //    {
        //        activeMoveSpeed = speed;
        //        dashCoolCounter = dashCooldown;
        //    }
        //}

        //if (dashCoolCounter > 0)
        //{
        //    dashCoolCounter -= Time.deltaTime;
        //}
        if (canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                animator.SetTrigger("jump");
            }
        }
    }

    public void ResetJump()
    {
        animator.ResetTrigger("jump");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fighter1")
        {
            GameObject fighter1 = GameObject.FindGameObjectWithTag("Fighter1");
            Instantiate(cloudGameObject, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(fighter1);
        }
        if (isMain)
        {
            if (collision.tag == "FinishLine")
            {
                isFinished = true;
            }
        }
    }
}
