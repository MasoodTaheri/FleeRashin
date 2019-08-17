using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGroupController : MonoBehaviour
{
    [SerializeField]
    AIPlaneBehaviour[] membersArray = new AIPlaneBehaviour[0];
    [SerializeField]
    int maxNumberOfEngagingMember;


    Vector3 patrolTargetVelocity;

    List<AIPlaneBehaviour> members = new List<AIPlaneBehaviour>();
    Dictionary<GameObject, List<AIPlaneBehaviour>> engagedMembers = new Dictionary<GameObject, List<AIPlaneBehaviour>>();

    private void Start()
    {
        foreach (AIPlaneBehaviour member in membersArray)
        {
            if (member != null)
            {
                members.Add(member);
                member.SetAIGroupController(this);
            }
        }
    }

    private void Update()
    {
        CheckMembers();

        if (members.Count > 0)
        {
            AlignMembers();
        }

        if (engagedMembers.Count > 0)
        {
            CheckEngagedMembers();
        }

    }

    void CheckEngagedMembers()
    {
        foreach (KeyValuePair<GameObject,List<AIPlaneBehaviour>> item in engagedMembers)
        {
            for (int i = 0; i < item.Value.Count; i++)
                if (item.Value[i] == null)
                    item.Value.RemoveAt(i--);

            if (item.Key == null && item.Value.Count > 0)
            {
                for (int j = 0; j < item.Value.Count; j++)
                {
                    members.Add(item.Value[j]);
                    item.Value.RemoveAt(j--);
                }
            }
            else if (item.Key != null && item.Value.Count < maxNumberOfEngagingMember)
            {

                while (item.Value.Count < maxNumberOfEngagingMember && members.Count > 0)
                {
                    item.Value.Add(GetAnAI());
                    item.Value[item.Value.Count - 1].target = item.Key;
                    item.Value[item.Value.Count - 1].ChangeState(AIPlaneBehaviour.EnemyPlaneState.chasingEnemy);
                }
            }
        }


    }

    void AlignMembers()
    {
        for (int i = 0; i < members.Count; i++)
        {
            members[i].targetPos = transform.position;
            members[i].ChangeState(AIPlaneBehaviour.EnemyPlaneState.FlyToward);
        }
    }

    void CheckMembers()
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i] == null)
            {
                members.RemoveAt(i--);
            }
        }
    }

    public void EnemySpotted(GameObject enemy)
    {
        if (engagedMembers.ContainsKey(enemy))
        {
            return;
        }
        
        List<AIPlaneBehaviour> engagedMemberList = new List<AIPlaneBehaviour>();
        for (int i = 0; i < maxNumberOfEngagingMember; i++)
        {
            if (members.Count > 0)
            {
                engagedMemberList.Add(GetAnAI());
            }
        }
        
        if (engagedMemberList.Count > 0)
        {
            engagedMembers.Add(enemy, engagedMemberList);
            foreach (AIPlaneBehaviour member in engagedMembers[enemy])
            {
                member.target = enemy;
                member.ChangeState(AIPlaneBehaviour.EnemyPlaneState.chasingEnemy);
            }
        }
    }

    AIPlaneBehaviour GetAnAI()
    {
        if (members.Count == 0)
            return null;

        AIPlaneBehaviour temp = members[members.Count - 1];
        members.RemoveAt(members.Count - 1);
        return temp;
    }
}
