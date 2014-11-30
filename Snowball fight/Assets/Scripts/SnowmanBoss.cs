using UnityEngine;
using System.Collections;

public class SnowmanBoss : Enemy {
    public Vector3 jumpPower;
    public float chaseSpeed = 50f;
    public float ballSpeed = 3f;
	// Use this for initialization
	void Start () {
        animation["Wait"].wrapMode = WrapMode.Loop;
        animation["Throw"].wrapMode = WrapMode.Once;
        animation["Death"].wrapMode = WrapMode.Once;
        animation["Jump"].wrapMode = WrapMode.Once;
        animation["PreThrow"].wrapMode = WrapMode.Loop;
        animation["PostThrow"].wrapMode = WrapMode.Loop;

        state = STATE.WAIT;
        Setup();
	}

    Vector3 Jump(){
        Vector3 velocity = new Vector3();
        Vector3 dir = target.position - this.transform.position;
        dir.y = 0f;
        dir.Normalize();
        dir *= chaseSpeed;
        if (controller.isGrounded)
        {
            velocity.y = jumpPower.y;
            velocity += dir;
            animation.CrossFade("Jump", 0.2f);
        }
        return velocity;
    }

    //なげ
    protected IEnumerator Shot()
    {
        state = STATE.NONE;

        //最初に球を作り
        Transform ballTrans = transform.Find("Armature/Bone/Bone_001/Bone_002");
        ballTrans.position += (transform.up * 5);

        GameObject ball = (GameObject)Instantiate(snowBall, ballTrans.position, Quaternion.identity);
        // サイズをアレする
        ball.transform.localScale *= ballScale;
        SnowBall ballScr = (SnowBall)ball.GetComponent("SnowBall");
        ball.rigidbody.useGravity = false;

        print("start preattack");
        float time = Time.time;
        while (Time.time - time <= 0.02)
        {
            yield return new WaitForSeconds(0.001f);
        }

        print("start Swing");
        animation.CrossFade("Throw");
        while (animation.IsPlaying("Throw"))
        {
            if (animation["Throw"].time > 1.5f)
            {
                //ここで飛ばす
                //距離によってy値を調整
                if (ball != null)
                {
                    ball.rigidbody.useGravity = true;
                    float sqrLen = GetLen();
                    ballScr.SetVelocity((target.transform.position - this.transform.position) * ballSpeed);
                    ballScr.AddForce(Vector3.up * 10f);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }        

        animation.CrossFade("PostThrow", 0);
        while (Time.time - time <= 0.02)
        {
            yield return new WaitForSeconds(0.01f);
        }
        print("End AnimationCheck");

        animation.Blend("PostThrow", 0, 02);
        animation.CrossFade("Wait", 0.2f);
        state = STATE.WAIT;
    }

	// Update is called once per frame
	void Update () {
        float delta = Time.deltaTime;
        elapsedTime += delta;


        switch(state){
            case STATE.WAIT:
                moveDirection = Vector3.zero;
                ChaseTarget(delta);
                if (elapsedTime > 2)
                {
                    if (Random.Range(0, 100) < MOVE_RATIO)
                    {
                        state = STATE.JUMP;
                    }
                    else
                    {
                        state = STATE.ATTACK;
                    }
                    elapsedTime = 0;
                }
                animation.CrossFade("Wait", 0.2f);
                
                break;
            case STATE.JUMP:
                ChaseTarget(delta);
                SetTarget();
                moveDirection += Jump();
                elapsedTime = 0f;
                state = STATE.FLYING;
                break;
            case STATE.ATTACK:
                ChaseTarget(delta);
                StartCoroutine("Shot");
                elapsedTime = 0f;
                state = STATE.NONE;
                break;
            case STATE.DEATH:
                animation.CrossFade("Death", 0.2f);
                StartCoroutine("Die");
                break;
            case STATE.FLYING:
                if (controller.isGrounded)
                {
                    target = playerTransform;
                    state = STATE.WAIT;
                    elapsedTime = 0f;
                }
                break;
        }

        moveDirection.y -= Defines.gravity * delta;
        controller.Move(moveDirection * delta);
	}
}
