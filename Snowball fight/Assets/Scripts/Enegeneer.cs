using UnityEngine;
using System.Collections;

public class Enegeneer : Enemy {

	// Use this for initialization
	void Start () {
        animation["Run"].wrapMode = WrapMode.Loop;
        animation["Make"].wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
