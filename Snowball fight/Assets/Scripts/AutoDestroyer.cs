using UnityEngine;
using System.Collections;

public class AutoDestroyer : MonoBehaviour {
    public float lifeTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    float elapsedTime = 0f;
	void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > lifeTime)
        {
            this.enabled = false;
            Destroy(this.gameObject);
        }
	}
}
