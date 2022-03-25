using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveAI : MonoBehaviour
{
    public Transform[] wayPoints;
    public float patrolTime = 2f;
    public float chaseTime = 3f;

    private int index = 0;
    private float patrolTimer  = 0;
    private float chaseTimer  = 0;
    private NavMeshAgent navAgent;
    private EnemySight sight;
    private PlayerHealth health;

    void Awake()
    {
        navAgent = this.GetComponent<NavMeshAgent>();
        navAgent.destination = wayPoints[index].position;
        // navAgent.updatePosition = false;
        // navAgent.updateRotation = false;
        sight = this.GetComponent<EnemySight>();
        health = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if(sight.palyerInSight && health.HP >0){
            //shoot
            Shooting();
        }else if(sight.alertPosition != Vector3.zero && health.HP >0){
            //chase
            Chasing();
        }else{
            Patrolling();
        }
        // navAgent.updatePosition = false;
        // navAgent.updateRotation = false;
    }

    private void Shooting(){
        navAgent.isStopped = true;
    }

    //巡逻
    private void Patrolling(){
        navAgent.speed = 1.5f;
        if(navAgent.remainingDistance < 0.5f){
        navAgent.isStopped = true;
        patrolTimer += Time.deltaTime;
        if (patrolTimer > patrolTime)
        {
            index++;
            index %= 4;
            navAgent.destination = wayPoints[index].position;
            // navAgent.updatePosition = false;
            // navAgent.updateRotation = false;
            patrolTimer = 0;
        }
        }else
        {
            navAgent.isStopped = false;
            // navAgent.updatePosition = false;
            // navAgent.updateRotation = false;
        }
    }

    private void Chasing(){
        navAgent.isStopped = false;
        navAgent.speed = 3f;
        navAgent.destination = sight.alertPosition;

        if (navAgent.remainingDistance < 2f)
        {
            chaseTimer += Time.deltaTime;
            if (chaseTimer > chaseTime)
            {
                sight.alertPosition = Vector3.zero;
                GameController._instance.lastPlayerPosition = Vector3.zero;
                GameController._instance.alarmOn = false;
            }
        }
    }
}
