using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public int life;
    public int speed;
    public Transform target;
    public string targetName;
    public float rotSpeed;
    public float ballScale;
    CharacterController controller;
    Vector3 moveDirection = new Vector3();
    public GameObject snowBall;
    public Vector3 throwPower;
    public float WAIT_CONTINUE_TIME;
    public float MOVE_CONTINUE_TIME;
    public int ATTACK_RATIO;
    public int MOVE_RATIO;


    //EnemyState state;
    enum STATE
    {
        NONE,
        WAIT,
        MOVE,
        ATTACK,
        DEATH,
    };
    STATE state = STATE.WAIT;
	// Use this for initialization
	void Start () {
	    //controller.
        animation["Wait"].wrapMode = WrapMode.Loop;
        animation["Throw"].wrapMode = WrapMode.Once;
        animation["Death"].wrapMode = WrapMode.Once;
        animation["PreThrow"].wrapMode = WrapMode.Loop;
        animation["PostThrow"].wrapMode = WrapMode.Loop;
        target = GameObject.Find(targetName).transform;

        controller = (CharacterController)GetComponent("CharacterController");
	}

    //なげ
    IEnumerator Shot() {
        state = STATE.NONE;

	    print("start preattack");
	    float time = Time.time;
	    while( Time.time - time <= 0.02){
		    yield return new WaitForSeconds( 0.001f );
	    }
	    print("start Swing");
	    animation.CrossFade("Throw");
	    while( animation.IsPlaying("Throw") ){
		    yield return new WaitForSeconds( 0.01f );
	    }
        
        Transform ballTrans = transform.Find("Armature/Root/West/Body/ArmA_R/ArmB_R/BallInitPosition");
        ballTrans.position += (transform.right * 2);
        GameObject ball = (GameObject)Instantiate(snowBall, ballTrans.position, Quaternion.identity);
        // サイズをアレする
        ball.transform.localScale *= ballScale;
        SnowBall ballScr = (SnowBall)ball.GetComponent("SnowBall");

        //距離によってy値を調整
        Vector3 distance = target.transform.position - this.transform.position;
        float sqrLen = distance.sqrMagnitude;
        ballScr.AddForce(throwPower.x * transform.forward.x, throwPower.y *1f * Mathf.Max(0,(sqrLen - 100)/1000), throwPower.z * transform.forward.z);

	    animation.CrossFade("PostThrow",0);
	    while( Time.time - time <= 0.02){
            yield return new WaitForSeconds(0.01f);
	    }
	    print("End AnimationCheck");

	    animation.Blend("PostThrow",0,02);
	    animation.CrossFade("Wait",0.2f);
        state = STATE.WAIT;
    }

    //この辺は後で実装
    void ChangeState(STATE prevState)
    {
        elapsedTime = 0f;

        int ratio = Random.Range(0, 100);
        
       
        switch (prevState)
        {
            case STATE.WAIT:
                if (ratio < ATTACK_RATIO)
                {
                    if (CheckAttackable())
                    {
                        state = STATE.ATTACK;
                    }//else nop
                    else
                    {
                        state = STATE.MOVE;
                    }
                }
                else if (ratio < ATTACK_RATIO + MOVE_RATIO)
                {
                    state = STATE.MOVE;
                }
                else
                {
                    state = STATE.WAIT;
                }
                break;
            case STATE.MOVE:
                if (ratio < ATTACK_RATIO)
                {
                    if (CheckAttackable())
                    {
                        state = STATE.ATTACK;
                    }//else nop 
                    else
                    {
                        state = STATE.WAIT;
                    }
                }
                else if (ratio < ATTACK_RATIO + MOVE_RATIO)
                {
                    state = STATE.MOVE;
                }
                else
                {
                    state = STATE.WAIT;
                }
                break;
            case STATE.ATTACK:
                if (ratio <  MOVE_RATIO)
                {
                    state = STATE.MOVE;
                }
                else
                {
                    state = STATE.WAIT;
                }
                break;
        }
        
        Util.DebugPrint("chnge state" + prevState + " to " + state);
    }

    Vector3 CalcMoveDir()
    {
        //直線的に来る
        Vector3 ret = this.transform.position;
        ret = target.position - ret;
        ret.Normalize();
        return ret;
    }

    //死ぬ
    IEnumerator Die()
    {
        state = STATE.NONE;
        while (animation.IsPlaying("Death"))
        {
            yield return new WaitForSeconds(0.01f);
        }
        GameLogic.instance.DecreaceEnemy();
        Destroy(this.gameObject);
    }

    //targetAim
    float AimTarget(Vector3 p1, Vector3 p2)
    {
        float dx = p2.x - p1.x;
        float dz = p2.z - p1.z;

        float rad = Mathf.Atan2(dz, dx);
        return rad * Mathf.Rad2Deg;
    }

	// Update is called once per frame
    float elapsedTime = 0f; //ある状態を継続した時間
    float angle = 0;
	void Update () {
        float delta = Time.deltaTime;
        elapsedTime += delta;

        if (life <= 0)
        {
            state = STATE.DEATH;
        }
        switch (state)
        {
            case STATE.WAIT:
                animation.CrossFade("Wait", 0.2f);
                ChaseTarget(delta);

                //ChaseTarget(delta);
                if (elapsedTime > WAIT_CONTINUE_TIME + Random.Range(0.0f,2.0f) )
                {
                    ChangeState(state);
                    //state = STATE.NONE;
                }
                moveDirection = Vector3.zero;
                break;
            case STATE.MOVE:
                animation.CrossFade("Run", 0.2f);
                /*
                Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
                Vector3 thisPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
                Vector3 dir = (targetPos - thisPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.time * 0.1f);
                 * */
                ChaseTarget(delta);
                moveDirection = transform.forward;
                moveDirection *= speed;
                
                /*
                moveDirection = CalcMoveDir() * speed;
                moveDirection.y -= Defines.gravity * Time.deltaTime;
                controller.Move(moveDirection * Time.deltaTime);
                 * */
                if (elapsedTime > MOVE_CONTINUE_TIME + Random.Range(0.0f, 2.0f) )
                {
                    ChangeState(state);
                   
                    //state = STATE.NONE;
                }

                break;
            case STATE.ATTACK:
                StartCoroutine("Shot");
                ChangeState(state);
                break;
            case STATE.DEATH:
                animation.CrossFade("Death", 0.2f);
                StartCoroutine("Die");
                break;
            default:
                //moveDirection = Vector3.zero;
                break;
        }

        moveDirection.y -= Defines.gravity * delta;
        controller.Move(moveDirection * delta);
	}

    private bool CheckAttackable()
    {
        Vector3 dir = (target.transform.position - this.transform.position);
        dir.Normalize();
        float rad = Vector3.Dot(dir, transform.TransformDirection(Vector3.forward));

        if (rad < 0.9f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ChaseTarget(float delta)
    {
        //rotate
        //transform.LookAt(new Vector3(target.position.x,this.transform.position.y,target.position.z));
        Vector3 dir = (target.transform.position - this.transform.position);
        dir.Normalize();
        float rad = Vector3.Dot(dir, transform.TransformDirection(Vector3.forward));

        float axis = 1;
        if (rad < 0)
        {
            axis *= -1;
        }
        Util.DebugPrint("rot = " + transform.rotation);
        if (rad < 0.9f)
        {
            //transform.rotation =  Quaternion.Euler(new Vector3(0, angle, 0));
            transform.Rotate(transform.up, delta * 200);
            //transform.LookAt(target.position);
        }
    }
}
