using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mathtest : MonoBehaviour
{
    public GameObject Agent;
    public GameObject target;
    public Vector3 Agentforward;
    public Vector3 agentToTarget;
    public float dot;
    public float crosslength;
    public Vector3 cross;



    //dot  for front or back
    //cross for left or right


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Agentforward = Agent.transform.forward;
        agentToTarget = target.transform.position - Agent.transform.position;
        Debug.DrawRay(Agent.transform.position, Agentforward, Color.blue);
        Debug.DrawRay(Agent.transform.position, agentToTarget, Color.red);
        dot = Vector3.Dot(Agentforward, agentToTarget);



        cross = Vector3.Cross(Agentforward, agentToTarget);
        Debug.DrawRay(Agent.transform.position, cross, Color.yellow);

        crosslength = Vector3.Dot(cross, Agent.transform.up);
        // crosslength<0 turn left
        // crosslength >0 turn right


        //dot>0 target in front
        //dot<0 tatget is in  back


    }
}
