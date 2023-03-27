using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class psEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] travelPoints;
    [SerializeField] public Transform[] perchedPositions;
    [SerializeField] private MultiAimConstraint multiAimConstraint;
    private GameObject player;
    private int currentPoint;
    [SerializeField] private GameObject[] blackLightTargets;
    private Transform selectedTarget;
    private GameObject closestTarget;
    [HideInInspector] public Transform closestPoint;
    private bool destinationSet;
    [HideInInspector] public bool isMoving, inPhase1, inPhase2;
    private NavMeshAgent agent;
    [HideInInspector] public State state;
    public enum State
    {
        moving, isPerching, perched, loop, idle
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        destinationSet = false;
        state = State.idle;
        inPhase1 = true;
        inPhase2 = false;
    }

    private void Update()
    {
        switch(state)
        {
            case State.idle:
            break;
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
        isMoving = true;
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
    }

    private void IsPerching()
    {
        isMoving = true;
        destinationSet = false;
        if (!destinationSet)
        {
            agent.SetDestination(closestPoint.position);
            destinationSet = true;
        }

        float distanceToPerchPoint = Vector3.Distance(this.transform.position, closestPoint.position);
        if(distanceToPerchPoint < 1f)
        {
            state = State.perched;
        }
    }

    private void Perched()
    {
        inPhase1 = false;
        inPhase2 = true;
        isMoving = false;
        destinationSet = false;
        if(selectedTarget == null) return;
        Vector3 direction = selectedTarget.transform.position - this.transform.position;
        Vector3 rotation = Quaternion.LookRotation(direction).eulerAngles;
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, rotation.y, this.transform.eulerAngles.z);
    }

    private void Loop()
    {
        currentPoint = 0;
        state = State.moving;
    }

    public void FindClosestPosition()
    {
        if (selectedTarget != null)
        {
            float closestDistance = Mathf.Infinity;

            foreach(Transform perchedPosition in perchedPositions)
            {
                float distance = Vector3.Distance(selectedTarget.transform.position, perchedPosition.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = perchedPosition;
                }
            }

            state = State.isPerching;
        }
    }

    public void SetSelectedTarget(Transform target)
    {
        selectedTarget = target;
    }

    public void SwitchTarget(int index, float weight)
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
