using UnityEngine;
using System.Collections;

public class VirtualInput : MonoBehaviour {
    public enum INPUT
    {
        LEAP,
        MOUSE,
        CONTROLLER,
    }
    public INPUT inputMode = INPUT.CONTROLLER;
    public static VirtualInput instance;
    HandController handController;
    void Awake()
    {
        //最初は初期状態
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ThrowBall()
    {
        switch (inputMode)
        {
            case INPUT.CONTROLLER:
                break;
            case INPUT.LEAP:
                break;
            case INPUT.MOUSE:
                if (Input.GetMouseButton(0))
                {

                }
                break;
        }
    }

    /*
    public bool CheckInput()
    {
        switch (inputMode)
        {
            case INPUT.CONTROLLER:
                break;
            case INPUT.LEAP:
                HandModel[] physicsHands = handController.GetAllPhysicsHands();
                for (int i = 0; i < physicsHands.Length; i++) {

                    HandModel handModel = physicsHands[i];
                    Leap.Hand hand = handModel.GetLeapHand();
                    int handID = (hand.IsLeft ? 0 : 1);
                    if (hand.IsValid) {

                        // 構える位置に手があれば
                        if (handModel.GetLeapHand().PalmPosition.DistanceTo(new Leap.Vector()) < setUpRange) {
                            throwState[handID] = ThrowState.Throwing;
                        }
                        else {
                            // もし、手が遠くにあり、今の状態が投げ状態であった場合
                            if (throwState[handID] == ThrowState.Throwing) {

                                this.ThrowBall(handModel);
                                throwState[handID] = ThrowState.Release;
                            }
                        }
                    }
                    else throwState[handID] = ThrowState.None;
                }
                break;
            case INPUT.MOUSE:
                if (Input.GetMouseButton(0))
                {

                }
                break;
        }
    }
     * */

    /*
    public void ThrowBall(GameObject ball,Vector3 ipos,Vector3 idir,HandModel handModel,float ballSpeed)
    {
        Vector3 pos = ipos;
        Vector3 dir = idir;
        switch (inputMode)
        {
            case INPUT.CONTROLLER:
                break;
            case INPUT.MOUSE:
                // プレハブからボールを生成
                GameObject newBall = (GameObject)Instantiate(ball, pos, new Quaternion());
                // ballControllerを取得
                BallController ballController = newBall.GetComponent<BallController>();

                RigidHand rigidHand = handModel.GetComponent<RigidHand>();

                vec = rigidHand.palm.rigidbody.velocity;
                vec.y += 2; // ちょっと上に投げる
                vec *= ballSpeed;

                // 速度を設定
                ballController.SetVec(vec);

                break;
            case INPUT.LEAP:
                pos = handModel.GetPalmPosition();
                GameObject centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
                Vector3 vec = centerEyeAnchor.transform.forward * ballSpeed;

                pos += centerEyeAnchor.transform.forward;

                // プレハブからボールを生成
                GameObject newBall = (GameObject)Instantiate(ball, pos, new Quaternion());
                // ballControllerを取得
                BallController ballController = newBall.GetComponent<BallController>();

                RigidHand rigidHand = handModel.GetComponent<RigidHand>();

                vec = rigidHand.palm.rigidbody.velocity;
                vec.y += 2; // ちょっと上に投げる
                vec *= ballSpeed;

                // 速度を設定
                ballController.SetVec(vec);


                // 視線方向に球を投げる
                //GameObject camera = GameObject.FindWithTag("MainCamera");
                //OVRCameraRig ovrCameraRig = camera.GetComponent<OVRCameraRig>();
                //Vector3 vec = ovrCameraRig.centerEyeAnchor.forward * ballSpeed;
                ////Debug.Log(ovrCameraRig.centerEyeAnchor.forward);
                //ballController.SetVec(ovrCameraRig.centerEyeAnchor.forward * ballSpeed);

                //ballController.SetVec(vec);
                break;
        }
    }
     * */

    public void Move()
    {
        switch (inputMode)
        {
            case INPUT.CONTROLLER:
                break;
            case INPUT.LEAP:
                break;
            case INPUT.MOUSE:
                
                break;
        }
    }

    void Start()
    {
        // LeapMotionのクラスを取得
        handController = GetComponent<HandController>();
    }
}
