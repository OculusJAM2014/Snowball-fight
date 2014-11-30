using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public GameObject lifeGage;
    UISlider slider;
    
    public float maxLife = 100f;
    float life = 100f;

	// Use this for initialization
	void Start () {
        slider = (UISlider)lifeGage.GetComponent("UISlider");
        life = maxLife;
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = life/maxLife;
	}

    public void Damage(int damage)
    {
        life -= damage;
        if (life < 0)
        {
            life = 0f;
            //死ぬ

        }
    }
}
