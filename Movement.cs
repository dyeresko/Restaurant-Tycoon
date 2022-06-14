using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    int num;
    float startTime;
    float waitFor = 4;
    bool timerStart = true;
    NavMeshAgent agent;
    public bool isQeued;
    public bool caBeServed;
    private Animator animator;
    public GameObject[] waypoints;
    int waypointIndex;
    int tableIndex;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        num = Random.Range(1, 5);
        startTime = Time.time;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (agent.remainingDistance > 0)
        {
            if(animator.GetInteger("move") != 1)
            {
                animator.SetFloat("speed", 1);
                animator.SetInteger("move", 1);
            }
            
        }
        else
        {
            if (animator.GetInteger("move") != 0)
            {
                animator.SetInteger("move", 0);
                animator.SetFloat("speed", 1);
            }
        }
        if (!isQeued)
        {
            if (!waypoints[0].GetComponent<IsServing>().isServing)
            {
                waypoints[0].GetComponent<IsServing>().isServing = true;
                agent.SetDestination(waypoints[0].transform.position);
            }
        }
        if (Vector3.Distance(transform.position, waypoints[0].transform.position) < 1)
        {
            StartCoroutine("Waiting");
        }
        if (!caBeServed)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }

        if (Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) < 1)
        {
            Quaternion targetRot = Quaternion.Euler(waypoints[waypointIndex].transform.eulerAngles.x, waypoints[waypointIndex].transform.eulerAngles.y, waypoints[waypointIndex].transform.eulerAngles.z);
            
            
            transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);
            transform.rotation = targetRot;
            
            waypoints[waypointIndex].GetComponent<IsSitting>().isSitting = true;
            animator.SetTrigger("sitting");

            if (!target.GetComponent<IsSitting>().madeAnOrder)
            {
                timerStart = true;
                StartCoroutine("MaikingAnOrder");
                
            }
        }


    }
    void UpdateDestination()
    {

        if (!waypoints[waypointIndex].GetComponent<IsSitting>().isSitting && isQeued && !waypoints[waypointIndex].GetComponent<IsSitting>().isTargeted)
        {
            waypoints[waypointIndex].GetComponent<IsSitting>().isTargeted = true;
            target = waypoints[waypointIndex];
            agent.SetDestination(target.transform.position);
            
            caBeServed = true;
            
            
        }

    }

    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 1;
        }
    }


    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1.0f);
        startTime = Time.time;
        isQeued = true;
        waypoints[0].GetComponent<IsServing>().isServing = false;
        timerStart = false;
        
    }

    IEnumerator MaikingAnOrder()
    {
        yield return new WaitForSeconds(num);
        
        startTime = Time.time;
        target.GetComponent<IsSitting>().madeAnOrder = true;
        timerStart = false;

    }
}


