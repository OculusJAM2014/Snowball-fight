using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
    public GameObject engineer;
    public GameObject attacker;
    public GameObject defencer;
    public Transform target;
    public int maxEnemyCnt;
    public float enemyCreateCycle;
    public static GameLogic instance;
    int enemyCnt { set; get; }
    Object locker = new Object();

    public void DecreaceEnemy()
    {
        lock (locker)
        {
            enemyCnt--;
            if (enemyCnt < 0)
            {
                enemyCnt = 0;
            }
            Util.DebugPrint("Enemy destroyed." + enemyCnt);
        }
    }

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

    public void CreateEnemy()
    {
        if (enemyCnt > maxEnemyCnt)
        {
            return;
        }
        Vector3 ofsPos = new Vector3(Random.Range(-30, 30), 20, Random.Range(-30, 30));
        Vector3 pos = target.position + ofsPos;

        if (Random.Range(0, 100) < 30)
        {
            Instantiate(engineer, pos, Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0)));
        }
        else if (Random.Range(0, 100) < 60)
        {
            Instantiate(attacker, pos, Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0)));
        }
        else
        {
            Instantiate(defencer, pos, Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0)));
        }
        enemyCnt++;
        Util.DebugPrint("Enemy created." + enemyCnt);
    }

	// Use this for initialization
	void Start () {
        CreateEnemy();
        CreateEnemy();
        CreateEnemy();
	}

    float elapsedTime;
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > enemyCreateCycle + Random.Range(0, 2))
        {
            CreateEnemy();
            elapsedTime = 0f;
        }
	}
}
