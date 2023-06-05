using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    private List<Turret> nearby = new List<Turret>();
    public float speed = 10;
    public float basicspeed;
    public float hp = 150;
    private float totalHp;
    public GameObject explosionEffect;
    private Slider hpSlider;
    private Transform[] positions;
    private int index = 0;

    //edit by zlt 添加进入范围的塔，以作为技能释放对象
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Turret")
        {
            nearby.Add(col.GetComponent<Turret>());
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Turret")
        {
            nearby.Remove(col.GetComponent<Turret>());
        }
    }

    // Use this for initialization
    void Start () {
        positions = Waypoints.positions;
        totalHp = hp;
        hpSlider = GetComponentInChildren<Slider>();
        basicspeed = speed;
        StartCoroutine(Func());

    }
	
	// Update is called once per frame
	void Update () {
        Move();
	}


    void Move()
    {
        if (index > positions.Length - 1) return;
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * speed);
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }
        if (index > positions.Length - 1)
        {
            ReachDestination();
        }
    }
    //达到终点
    void ReachDestination()
    {
        GameManager.Instance.Failed();
        GameObject.Destroy(this.gameObject);
    }


    void OnDestroy()
    {
        EnemySpawner.CountEnemyAlive--;
    }

    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = (float)hp / totalHp;

        //edit by zlt 实现enemy2的技能，掉血加速
        if (this.tag == "enm2" && hp < 150)
            speed =30;

        if (hp <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);

        //edit by zlt enemy3的技能，死亡后摧毁附近的塔
        if (this.tag == "enm3")
            foreach (Turret near in nearby)
                if (near != null)
                {
                    Collider cld1 = this.GetComponent<Collider>();
                    Collider cld2 = near.GetComponent<Collider>();
                    Vector3 center1 = cld1.bounds.center;
                    Vector3 center2 = cld2.bounds.center;
                    float dist = Vector3.Distance(center1, center2);
                    Debug.Log("Distance between two colliders: " + dist);
                    if (dist < 10) near.Die();
                }

        //edit by zlt  enemy4的技能，冻结附近的塔
        if (this.tag == "enm4")
            foreach (Turret near in nearby)
            {
                Collider cld1 = this.GetComponent<Collider>();
                Collider cld2 = near.GetComponent<Collider>();
                Vector3 center1 = cld1.bounds.center;
                Vector3 center2 = cld2.bounds.center;
                float dist = Vector3.Distance(center1, center2);
                Debug.Log("Distance between two colliders: " + dist);
                if (dist < 15) near.Freeze();
            }

        Destroy(this.gameObject);
    }
    // ---------------------------------------邱天 add  每个一段时间判断是否被减速-------------------------------------------------------
    IEnumerator Func()
    {
        while (true)// or for(i;i;i)
        {
            speed = GlobalRate.speedRate * basicspeed;
            yield return new WaitForSeconds(0.1f);
        }
    }
    // ---------------------------------------邱天 add  每个一段时间判断是否被减速-------------------------------------------------------

}
