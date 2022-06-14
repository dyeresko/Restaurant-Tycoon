using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChefMovement : MonoBehaviour
{
    public GameObject[] orderWaypoints;
    public int numberOfOrders = -1;
    public bool isOnFridgePos = false;
    private Vector3 targetPos;
    public bool isOnBaker = false;
    public bool isServing = false;
    NavMeshAgent agent;
    Animator animator;
    public GameObject target;
    public GameObject fridge;
    public GameObject baker;
    
    // Start is called before the first frame update
    void Start()
    {

        
        animator = GetComponent<Animator>();
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
                
                animator.SetInteger("move", 0);
                animator.SetFloat("speed", 1);
            }
        }
        if (!isServing)
        {
            for (int i = 0; i < orderWaypoints.Length; i++)
            {
                if (orderWaypoints[i].GetComponent<IsServing>().isGivenAnOrder && !isServing)
                {
                    target = orderWaypoints[i];
                    if (target.GetComponent<IsServing>().isGivenAnOrder)
                    {
                        numberOfOrders++;
                    }
                    target.GetComponent<IsServing>().isGivenAnOrder = false;
                    targetPos = target.transform.position + new Vector3(1, 0, 0);
                    GoToTakeOrder();
                    
                }
            }
        }

        if (isServing)
        {
            if(!isOnFridgePos)
            {
                Invoke("TakeIngridients", 2);
            }
            if (!isOnBaker && isOnFridgePos)
            {
                Invoke("GoCook", 2);
            }
            if (isOnBaker)
            {
                Invoke("GoToTakeOrder", 2);
            }
            

        }
        
    }

    private void GoCook()
    {
        agent.SetDestination(baker.transform.position);
        if (Vector3.Distance(transform.position, baker.transform.position) < 2)
            isOnBaker = true;

    }
    private void TakeIngridients()
    {
        agent.SetDestination(fridge.transform.position);
        if (Vector3.Distance(transform.position, fridge.transform.position) < 2)
        {
            isOnFridgePos = true;
        }
    }

    private void GoToTakeOrder()
    {
        agent.SetDestination(targetPos);
        
        
        if (Vector3.Distance(transform.position, targetPos) < 2 && numberOfOrders >= 1)
        {
            Debug.Log(true);
            isServing = true;
        }
        
        if ((isOnBaker && Vector3.Distance(transform.position, targetPos) < 2))
        {
            Debug.Log(numberOfOrders);
            numberOfOrders--;
            isServing = false;
            isOnFridgePos = false;
            isOnBaker = false;
        }
    }
}
