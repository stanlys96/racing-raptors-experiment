using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float changeSeason = 5f;
    private float timeSinceLastChangeSeason = 0;
    public float[] speeds = new float[12];
    private float checkTime = 0f;
    private bool isFinished = false;
    private bool isRotating = false;
    private float delayGoingLeftOrRight = 0.25f;
    private float lastGoingLeftOrRight = Mathf.Infinity;
    private float timeSinceLastStart = Mathf.Infinity;
    private float timeBeforeAllStart = 5f;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastStart = 0;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (uiTokenId != null)
        {
            uiTokenId.text = tokenId.ToString();
        }
        if (gameObject.tag == "MainObject")
        {
            speeds = APICall.instance.secondPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[0])
        {
            speeds = APICall.instance.firstPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[1])
        {
            speeds = APICall.instance.secondPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[2])
        {
            speeds = APICall.instance.thirdPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[0])
        {
            speeds = APICall.instance.fourthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[1])
        {
            speeds = APICall.instance.fifthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[2])
        {
            speeds = APICall.instance.sixthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[3])
        {
            speeds = APICall.instance.seventhPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[4])
        {
            speeds = APICall.instance.eighthPlaceSpeeds;
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
        timeSinceLastStart += Time.deltaTime;
        if (timeSinceLastStart > timeBeforeAllStart)
        {
            checkTime += Time.deltaTime;
            timeSinceLastChangeSeason += Time.deltaTime;
            lastGoingLeftOrRight += Time.deltaTime;
        }

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
        if (tokenId == APICall.instance.fighters[1] && index == 3)
        {
            lastGoingLeftOrRight = 0f;
            GameObject targetFighter = GameObject.FindGameObjectWithTag("Fighter1");
            //transform.position = Vector2.MoveTowards(transform.position, targetFighter.transform.position, speed * 1.175f * Time.deltaTime);
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
            //transform.rotation = Quaternion.Euler(0, 0, rot_z - 90);
            StartCoroutine(SmoothRotation(targetFighter, diff));
            transform.Translate(Vector2.up * speed * 1.125f * Time.deltaTime);
        } 
        else
        {
            if (timeSinceLastStart > timeBeforeAllStart)
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
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

    IEnumerator SmoothRotation(GameObject Target, Vector3 difference)
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angleDeg), turnSpeed * Time.deltaTime);
        //iterate through the number of steps
        transform.Translate((difference.x < 0 ? Vector2.right : Vector2.left) * 0.5f * Time.deltaTime);
        int iMax = 100;
        for (int i = 1; i <= iMax; i++)
        {
            Vector3 diff = Target.transform.position - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y + 0.4f, diff.x) * Mathf.Rad2Deg;
            //turn 1 degree in  the right direction
            //transform.rotation = Quaternion.Euler(0, 0, rot_z * (i / iMax) - 90);
            Quaternion target = Quaternion.Euler(0, 0, rot_z - 90);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 5 * Time.deltaTime);
            //wait before next iteration
            yield return new WaitForSeconds(0.02f);
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
