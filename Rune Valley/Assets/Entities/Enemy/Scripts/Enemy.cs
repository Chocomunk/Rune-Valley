using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class Enemy : MonoBehaviour {

    private EnemyInfo info;
    private float attackCooldown;

	// Use this for initialization
	void Start () {
        info = this.GetComponent<EnemyInfo>();
        attackCooldown = 1 / info.attackSpeed;
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
        }
        else
        {
            attackCooldown = 1 / info.attackSpeed;
        }
	}

    bool inAttackRange()
    {
        if (PlayerManager.playerInstance)
        {
            float distance = Vector3.Distance(PlayerManager.playerInstance.transform.position, this.transform.position);
            return distance < info.attackRange;
        }
        return false;
    }

    bool checkAttack()
    {
        return attackCooldown <= 0;
    }

    void Attack()
    {
        attackCooldown = 1 / info.attackSpeed;
        PlayerManager.playerInstance.Damage(info.damage);
        Debug.Log(this.gameObject.name +" is attacking the player for "+info.damage+" damage");
    }
}
