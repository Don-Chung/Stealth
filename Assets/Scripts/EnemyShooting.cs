using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float minDamage = 30;

    private Animator anim;
    private bool haveShoot = false;
    private PlayerHealth health;

    private void Awake() {
        anim = this.GetComponent<Animator>();
        health = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (anim.GetFloat("Shot") > 0.5)
        {
            Shooting();
        }else{
            haveShoot = false;
        }
    }

    private void Shooting(){
        if (haveShoot == false)
        {
            //计算伤害
            float damage = minDamage + 90 - 9*(transform.position - health.transform.position).magnitude;
            health.TakeDamage(damage);

            haveShoot = true;
        }
    }
}
