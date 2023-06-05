using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private List<GameObject> enemys = new List<GameObject>();
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "enm1" || col.tag=="enm2"|| col.tag=="enm3" || col.tag=="enm4")
        {
            enemys.Add(col.gameObject);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "enm1" || col.tag == "enm2" || col.tag == "enm3" || col.tag == "enm4")
        {
            enemys.Remove(col.gameObject);
        }
    }

    public float attackRateTime = 1; //多少秒攻击一次
    public float basicAttackRateTime;
    private float timer = 0;

    public GameObject bulletPrefab;//子弹
    public Transform firePosition;
    public Transform head;

    public bool useLaser = false;

    public float damageRate = 70;
    public float basicDamageRate;

    public LineRenderer laserRenderer;

    public GameObject laserEffect;

    public GameObject ExpEffect;

    public bool isfrozen = false;

    void Start()
    {
        timer = attackRateTime;
        basicAttackRateTime = attackRateTime;
        basicDamageRate = damageRate;
        StartCoroutine(Func());
    }

    void Update()
    {
        if (!isfrozen)
        if (enemys.Count > 0 && enemys[0] != null)
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }
        if (useLaser == false)
        {
            timer += Time.deltaTime;
            if (enemys.Count > 0 && timer >= attackRateTime)
            {
                timer = 0;
                Attack();
            }
        }
        else if(enemys.Count>0)
        {
            if (laserRenderer.enabled == false)
                laserRenderer.enabled = true;
            laserEffect.SetActive(true);
            if (enemys[0] == null)
            {
                UpdateEnemys();
            }
            if (enemys.Count > 0)
            {
                laserRenderer.SetPositions(new Vector3[]{firePosition.position, enemys[0].transform.position});
                enemys[0].GetComponent<Enemy>().TakeDamage(damageRate *Time.deltaTime );
                laserEffect.transform.position = enemys[0].transform.position;
                Vector3 pos = transform.position;
                pos.y = enemys[0].transform.position.y;
                laserEffect.transform.LookAt(pos);
            }
        }
        else
        {
            laserEffect.SetActive(false);
            laserRenderer.enabled = false;
        }
    }

    void Attack()
    {
        if (enemys[0] == null)
        {
            UpdateEnemys();
        }
        if (enemys.Count > 0)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);
        }
        else
        {
            timer = attackRateTime;
        }
    }

    void UpdateEnemys()
    {
        //enemys.RemoveAll(null);
        List<int> emptyIndex = new List<int>();
        for (int index = 0; index < enemys.Count; index++)
        {
            if (enemys[index] == null)
            {
                emptyIndex.Add(index);
            }
        }

        for (int i = 0; i < emptyIndex.Count; i++)
        {
            enemys.RemoveAt(emptyIndex[i]-i);
        }
    }
    IEnumerator Func()
    {
        while (true)// or for(i;i;i)
        {
            damageRate = GlobalRate.turrentRate * basicDamageRate;
            attackRateTime = GlobalRate.turrentRate * basicAttackRateTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    //edit by zlt 实现了防御塔炸毁和被冻结
    public void Die()
    {
        GameObject effect = GameObject.Instantiate(ExpEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }

    IEnumerator ResetBool()
    {
        // 等待3秒钟
        yield return new WaitForSeconds(3f);

        isfrozen = false;
    }

    public void Freeze()
    {
        isfrozen = true;
        StartCoroutine(ResetBool());
    }
}
