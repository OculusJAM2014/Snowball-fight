using UnityEngine;
using System.Collections;

public class SnowmanBoss : Enemy {
    public Vector3 jumpPower;
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

    void Jump(){
        Vector3 dir = target.position - this.transform.position;
        
        dir.Normalize();
        dir *= GetLen() * 100;

        rigidbody.AddForce(dir + jumpPower);
    }

    //なげ
    protected IEnumerator Shot()
    {
        state = STATE.NONE;

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
            yield return new WaitForSeconds(0.01f);
        }

        Transform ballTrans = transform.Find("Armature/Root/West/Body/ArmA_R/ArmB_R/BallInitPosition");
        //ballTrans.position += (transform.right * 2);
        GameObject ball = (GameObject)Instantiate(snowBall, ballTrans.position, Quaternion.identity);
        // サイズをアレする
        ball.transform.localScale *= ballScale;
        SnowBall ballScr = (SnowBall)ball.GetComponent("SnowBall");
        ball.rigidbody.useGravity = false;

        //距離によってy値を調整
        float sqrLen = GetLen();
        ballScr.AddForce((transform.forward * throwPower.z) + new Vector3(0, 1f * DetectYPower(sqrLen) * throwPower.y));

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
                ChaseTarget(delta);
                if (elapsedTime > 2)
                {
                    state = STATE.JUMP;
                    elapsedTime = 0;
                }
                animation.CrossFade("Wait", 0.2f);
                
                break;
            case STATE.JUMP:
                ChaseTarget(delta);
                Jump();
                animation.CrossFade("Jump", 0.2f);
                
                break;
            case STATE.ATTACK:
                ChaseTarget(delta);
                animation.CrossFade("Attack", 0.2f);
                StartCoroutine("Shot");
                state = STATE.WAIT;
                break;
            case STATE.DEATH:
                animation.CrossFade("Death", 0.2f);
                StartCoroutine("Die");
                break;
        }

        moveDirection.y -= Defines.gravity * delta;
        controller.Move(moveDirection * delta);
	}
}
