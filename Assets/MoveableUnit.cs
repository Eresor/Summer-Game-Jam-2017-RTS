
using UnityEngine;
using UnityEngine.AI;

public class MoveableUnit : NetworkBehaviour {
    
    enum State
    {
        idle,
        move
    }
    SelectableUnit selection;
    public bool CanCollectResources;
    public float movementSpeed = 10f;
    public float controlSpeed = 2f;
    public Projector destinationMark;

    private Vector3 asteroidPosition;

    NavMeshAgent agent;
    State currentState;

	// Use this for initialization
    protected override void NetworkStart () {
        base.NetworkStart();
        selection = GetComponent<SelectableUnit>();
        agent = GetComponent<NavMeshAgent>();
        destinationMark.transform.SetParent(null);
        currentState = State.idle;
    }
	
	// Update is called once per frame
    protected override void NetworkUpdate () {
        base.NetworkUpdate();
        CheckState();
        PrintDestinationMarker();

        if (photonView.isMine && Vector3.Distance(asteroidPosition,transform.position)<10f)
            ResourceManager.Instance.Gold += Time.deltaTime * 10f;
    }

    private void CheckState()
    {
        if(agent.hasPath)
        {
            currentState = State.move;
        }
        else
        {
            currentState = State.idle;
        }
    }

    public void StopMoving()
    {
        currentState = State.idle;
        agent.isStopped = true;
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.isStopped = false;
        agent.SetDestination(point);
        var destinationPoint = new Vector3(point.x, 1, point.z);
        destinationMark.transform.position = destinationPoint;
    }

    public void CollectResources(Vector3 asteroidPosition)
    {
        this.asteroidPosition = asteroidPosition;
        MoveToPoint(asteroidPosition);
    }

    private void PrintDestinationMarker()
    {
        if( currentState == State.move && UnitSelection.IsSelected(selection) )
        {
            destinationMark.enabled = true;
        }
        else
        {
            destinationMark.enabled = false;
        }
    }

}
