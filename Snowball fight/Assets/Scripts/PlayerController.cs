using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public double setUpRange = 400;
    public float ballSpeed = 10;
    public GameObject ball;

    public HandModel hand_model;

    protected const float MM_TO_M = 0.001f;

    // LeapMotionのクラス
    HandController handController;

    // 右手か左手か
    enum HandID { Left, Right, Max};
    
    // プレイヤーの投げる状態
    enum ThrowState { None, Throwing, Release, Max };
    ThrowState[] throwState = new ThrowState[2] { ThrowState.None, ThrowState.None };


	// Use this for initialization
	void Start () {
        // LeapMotionのクラスを取得
        handController = GetComponent<HandController>();


	}
	
	// Update is called once per frame
    void LateUpdate(){
	//void Update () {


        GameObject camera = GameObject.FindWithTag("MainCamera");
        OVRCameraRig ovrCameraRig = camera.GetComponent<OVRCameraRig>();
        //Vector3 vec = ovrCameraRig.centerEyeAnchor.forward * ballSpeed;
        //Debug.Log(ovrCameraRig.centerEyeAnchor.forward);

        
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


        // Fire1が押された！(押され続けは検知されない)
        if (Input.GetButtonDown("Fire1")) {
            this.ThrowBallForward();
        }
	}
    void ThrowBall(HandModel handModel) {
        
        // プレハブからボールを生成
        GameObject newBall = (GameObject)Instantiate(ball, handModel.GetPalmPosition(), new Quaternion());

        RigidHand rigidHand = handModel.GetComponent<RigidHand>();

        // 手と球の当たり判定を消す
        Leap.Utils.IgnoreCollisions(rigidHand.gameObject, newBall);

        // 腕の速度を球に与える
        Vector3 vec = rigidHand.palm.rigidbody.velocity;
        vec.y += 2; // ちょっと上に投げる
        vec *= ballSpeed;

        // ballControllerを取得
        BallController ballController = newBall.GetComponent<BallController>();
        // 速度を設定
        ballController.SetVec(vec);


        
    }

    void ThrowBallForward() {
         //視線方向に球を投げる
        GameObject camera = GameObject.FindWithTag("MainCamera");
        OVRCameraRig ovrCameraRig = camera.GetComponent<OVRCameraRig>();

        GameObject firingPort = GameObject.FindWithTag("FiringPort");
        // プレハブからボールを生成
        GameObject newBall = (GameObject)Instantiate(ball, firingPort.transform.position, new Quaternion());

        Vector3 vec = ovrCameraRig.centerEyeAnchor.forward * ballSpeed * 2;
        vec.y += 2; // ちょっと上に投げる
        // ballControllerを取得
        BallController ballController = newBall.GetComponent<BallController>();
        ballController.SetVec(vec);
    }
}
