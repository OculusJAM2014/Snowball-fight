using UnityEngine;
using System.Collections;

public class SnowBall : MonoBehaviour {
    float elapsedTime = 0;
    public float DEST_TIME;
    public GameObject ballDestroyParticle;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > DEST_TIME)
        {
            Destroy(this.gameObject);
        }
	}


    public void SetVelocity(Vector3 velo)
    {
        rigidbody.velocity = velo;
    }
    public void AddForce(Vector3 force)
    {
        rigidbody.AddForce(force);
    }

    void OnTriggerEnter(Collider collision)
    {
        Util.DebugPrint("ball Trigger");
        if (collision.tag.Equals("Player"))
        {
            Util.DebugPrint("wall collision.");

            //パーティクル出す
            Instantiate(ballDestroyParticle, this.transform.position, Quaternion.identity);

            //ここで主人公のライフを削る
            Player player = (Player)collision.GetComponent("Player");
            player.Damage(10);

            //これ自体は破壊
            Destroy(this.gameObject);
        }
    }
}
