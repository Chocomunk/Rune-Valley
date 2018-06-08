using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class Enemy : MonoBehaviour {

    private EnemyStats stats;
    private float attackCooldown;

	// Use this for initialization
	void Start () {
        stats = this.GetComponent<EnemyStats>();
        attackCooldown = 1 / stats.attackSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        if (inAttackRange())
        {
            attackCooldown -= Time.deltaTime;
            if (checkAttack())
            {
                Attack();
            }
        } else {
            attackCooldown = 1 / stats.attackSpeed;
        }
	}

    void OnDrawGizmosSelected()
    {
        if (stats != null)
        {
            if (inAttackRange())
            {
                Gizmos.color = Color.red;
            } else
            {
                Gizmos.color = Color.yellow;
            }
            Gizmos.DrawWireSphere(this.transform.position, stats.attackRange);
        }
    }

    bool inAttackRange()
    {
        if (PlayerManager.instance.PlayerExists())
        {
            float distance = Vector3.Distance(PlayerManager.instance.playerInstance.transform.position, this.transform.position);
            return distance < stats.attackRange;
        }
        return false;
    }

    bool checkAttack()
    {
        return attackCooldown <= 0;
    }

    void Attack()
    {
        attackCooldown = 1 / stats.attackSpeed;
        PlayerManager.instance.playerInstance.Damage(stats.damage);
        Debug.Log(this.gameObject.name +" is attacking the player for "+stats.damage+" damage");
    }
}
