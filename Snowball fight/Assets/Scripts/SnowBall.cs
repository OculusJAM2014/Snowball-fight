using UnityEngine;
using System.Collections;

public class SnowBall : MonoBehaviour {
    float elapsedTime = 0;
    public float DEST_TIME;
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

    public void AddForce(float x,float y,float z)
    {
        rigidbody.AddForce(new Vector3(x,y,z));
    }
}
