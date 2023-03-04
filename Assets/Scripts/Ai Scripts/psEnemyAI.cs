using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class psEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] travelPoints;
    [SerializeField] private MultiAimConstraint multiAimConstraint;
    private GameObject player;
    private int currentPoint;
    private int[] indexArray;
    private bool destinationSet;
    private bool targetSet;
    private bool xRotSet;
    private NavMeshAgent agent;
    private State state;
    private enum State
    {
        moving, isPerching, perched, loop
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        destinationSet = false;
        state = State.moving;
    }

    private void Update()
    {
        switch(state)
        {
            case State.moving:
                Moving();
            break;
            case State.loop:
                Loop();
            break;
            case State.isPerching:
                IsPerching();
            break;
            case State.perched:
                Perched();
            break;
        }
    }

    private void Moving()
    {
        if (currentPoint == travelPoints.Length)
        {
            state = State.loop;
        }
        else
        {
            if (!destinationSet)
            {
                agent.SetDestination(travelPoints[currentPoint].position);
                destinationSet = true;
            }

            float distanceToPoint = Vector3.Distance(this.transform.position, travelPoints[currentPoint].position);
            if (distanceToPoint <= 1f)
            {
                currentPoint++;
                destinationSet = false;
            }
        }

        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        if (distanceToPlayer < 20f)
        {
            state = State.isPerching;
        }
    }

    private void IsPerching()
    {
        destinationSet = false;
        if (!destinationSet)
        {
            agent.SetDestination(travelPoints[4].position);
            destinationSet = true;
        }

        float distanceToPerchPoint = Vector3.Distance(this.transform.position, travelPoints[4].position);
        if(distanceToPerchPoint < 1f)
        {
            state = State.perched;
        }
    }

    private void Perched()
    {
        if (!targetSet)
        {
            SwitchTarget(2,1);
            targetSet = true;
        }
        RaycastHit perchInformation;
        if (Physics.Raycast(this.transform.position, Vector3.down, out perchInformation, 10f))
        {
            float xRot = perchInformation.collider.gameObject.transform.eulerAngles.x;
            Vector3 direction = player.transform.position - this.transform.position;
            Vector3 rotation = Quaternion.LookRotation(direction).eulerAngles;
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x - xRot, rotation.y, this.transform.eulerAngles.z);
        }
    }

    private void Loop()
    {
        currentPoint = 0;
        state = State.moving;
    }

    private void SwitchTarget(int index, float weight)
    {
        WeightedTransformArray arrayOfTransforms = multiAimConstraint.data.sourceObjects;
        for(int i = 0; i < arrayOfTransforms.Count; i++)
        {
            arrayOfTransforms.SetWeight(i, 0f);
        }
        multiAimConstraint.data.sourceObjects = arrayOfTransforms;

        WeightedTransformArray a = multiAimConstraint.data.sourceObjects;
        a.SetWeight(index, weight);
        multiAimConstraint.data.sourceObjects = a;
    }
}
