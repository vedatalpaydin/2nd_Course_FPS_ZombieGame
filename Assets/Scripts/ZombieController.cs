using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    enum STATE { IDLE, WANDER, ATTACK, CHASE, DEAD }
    private STATE state = STATE.IDLE;
    
    public GameObject target;
    public GameObject ragdoll;
    public AudioSource[] splats;
    
    private Animator anim;
    private NavMeshAgent agent;
    
    private string attacking = "IsAttacking";
    private string walking = "IsWalking";
    private string running = "IsRunning";
    private string dead = "IsDead";
    
    public float walkingSpeed;
    public float runningSpeed;
    public float damageAmount=5;
    


    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void TurnOffTrigger()
    {
        anim.SetBool(walking ,false);
        anim.SetBool(running, false);
        anim.SetBool(attacking, false);
        anim.SetBool(dead, false);
    }

   private float DistanceToPlayer()
   {
       if (GameStats.gameOver) return Mathf.Infinity;
        return Vector3.Distance(target.transform.position, transform.position);
    }
    private bool CanSeePlayer()
    {
        if (DistanceToPlayer() < 10)
            return true;
        return false;
    }

   private bool ForgetPlayer()
    {
        if (DistanceToPlayer() > 20)
            return true;
        return false;
    }

   public void KillZombie()
   {
       TurnOffTrigger();
       state = STATE.DEAD;
       anim.SetBool(dead,true);
   }
   void PlaySplatAudio()
   {
       AudioSource audioSource = new AudioSource();
       int n = Random.Range(1, splats.Length);

       audioSource = splats[n];
       audioSource.Play();
       splats[n] = splats[0];
       splats[0] = audioSource;
   }
   public void DamagePlayer()
   {
       if(target==null) return;
       target.GetComponent<FPController>().TakeDamage(damageAmount);
       PlaySplatAudio();
   }

    void Update()
    {
        if (target == null && GameStats.gameOver==false)
        {
            target = GameObject.FindWithTag("Player");
            return;
        }
        switch (state)
        {
            case STATE.IDLE:
                if (CanSeePlayer())
                    state = STATE.CHASE;
                else if(Random.Range(0,5000) < 5)
                    state = STATE.WANDER;
                break;
            case STATE.WANDER:
                if (!agent.hasPath)
                {
                    var newX = transform.position.x + Random.Range(-5, 5);
                    var newZ = transform.position.z + Random.Range(-5, 5);
                    var newY = Terrain.activeTerrain.SampleHeight(new Vector3(newX, 0, newZ));
                    Vector3 dest = new Vector3(newX, newY, newZ);
                    agent.SetDestination(dest);
                    agent.stoppingDistance = 0;
                    TurnOffTrigger();
                    agent.speed = walkingSpeed;
                    anim.SetBool(walking,true);
                }
                if (CanSeePlayer()) state = STATE.CHASE;
                else if (Random.Range(0, 5000) < 5)
                {
                    state = STATE.IDLE;
                    TurnOffTrigger();
                    agent.ResetPath();
                }
                break;
            case STATE.CHASE:
                if(GameStats.gameOver){ TurnOffTrigger(); state = STATE.WANDER; return;}
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 4;
                TurnOffTrigger();
                agent.speed = runningSpeed;
                anim.SetBool(running,true);

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                    state = STATE.ATTACK;
                if (ForgetPlayer())
                {
                    state = STATE.WANDER;
                    agent.ResetPath();
                }
                break;
            case STATE.ATTACK:
                if(GameStats.gameOver){ TurnOffTrigger(); state = STATE.WANDER; return;}
                TurnOffTrigger();
                anim.SetBool(attacking,true);
                transform.LookAt(target.transform.position);
                if (DistanceToPlayer() > agent.stoppingDistance+1) 
                    state = STATE.CHASE;
                break;
            case STATE.DEAD:
                Destroy(agent);
                AudioSource[] sounds = GetComponents<AudioSource>();
                foreach (AudioSource s in sounds)
                    s.volume = 0;
                GetComponent<Sink>().StartSink();
                break;
        }
    }
}
