using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class dd1 : MonoBehaviour
{
    NavMeshAgent meshAgent;
    public  Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        meshAgent.SetDestination( new Vector3( transform.position.x, 0, transform.position.z));
    }
}
