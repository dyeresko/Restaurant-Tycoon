using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class WaiterMovement : MonoBehaviour
{
    public bool isWaited = false;
    float startTime;
    float waitFor = 4;
    bool timerStart = true;
    NavMeshAgent agent;
    public bool isQeued;
    public bool caBeServed;
    public bool tookTheOrder;
    private Animator animator;
    public GameObject[] waypoints;
    public GameObject[] orderWaypoints;
    public GameObject defaultWaypoint;
    int waypointIndex;
    
    GameObject target;
    GameObject targetO;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        startTime = Time.time;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance > 0.01)
        {
            
            if (animator.GetInteger("move") != 1)
            {
                animator.SetFloat("speed", 1);
                animator.SetInteger("move", 1);
            }

        }
        
        else
        {
            if (animator.GetInteger("move") != 0)
            {
                animator.SetFloat("speed", 1);
            }
        }
        
        if (targetO != null)
        {
            
            if (Vector3.Distance(transform.position, targetO.transform.position) < 1)
            {
                
                
                GoToDefault();
                

            }
        }
        if (target != null)
        {
            
            if (Vector3.Distance(transform.position, target.transform.position) < 1 && caBeServed)
            {
                
                Invoke("GoToOrderWaypoint", 1);
                target = null;
                
                
            }
            

        }
        if (!caBeServed)
            UpdateDestination();

        
        
        
        
    }



    void UpdateDestination()
    {
        for (int waypointIndex = 0; waypointIndex < waypoints.Length; waypointIndex++)
        {



            if (waypoints[waypointIndex].GetComponent<IsSitting>().isSitting && !waypoints[waypointIndex].GetComponent<IsSitting>().isServing && waypoints[waypointIndex].GetComponent<IsSitting>().madeAnOrder)
            {

                
                target = waypoints[waypointIndex];
                agent.SetDestination(target.transform.position + new Vector3(0.5f, 0, -0.5f));
                
                waypoints[waypointIndex].GetComponent<IsSitting>().isServing = true;
                caBeServed = true;

            }
        }
            
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1.0f);
        isWaited = true;
        
           
    }
    void GoToDefault()
    {
        
        agent.SetDestination(defaultWaypoint.transform.position);
        if (targetO != null)
        {
            targetO.GetComponent<IsServing>().isGivenAnOrder = true;
            targetO = null;
        }

    }

    void GoToOrderWaypoint()
    {
        for (int waypointIndex = 0; waypointIndex < orderWaypoints.Length; waypointIndex++)
            {
                
                if (!orderWaypoints[waypointIndex].GetComponent<IsServing>().isServing && targetO == null)
                {   
                    
                    orderWaypoints[waypointIndex].GetComponent<IsServing>().isServing = true;
                    targetO = orderWaypoints[waypointIndex];
                    agent.SetDestination(targetO.transform.position);
                    
                    tookTheOrder = false;
                }
                
                
                

            }
            tookTheOrder = false;
    }    

}
