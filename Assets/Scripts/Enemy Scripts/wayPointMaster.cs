using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayPointMaster : MonoBehaviour
{
    /* enemies
     * Assigned in editor by map creator to identify which enemies follow this path
     */
    public GameObject[] enemies;
    /* invokingEnemy
     * updated by waypointControl to pass the colliding enemy
     * WaypointControl::onTriggerEnter2D
     */
    public GameObject invokingEnemy;

    /* invokingArgs
     * tied to invokingEnemy 
     * update by waypointControl as a way to pass args on what to do with colliding enemy
     */
    public string[] invokingArgs;

    /* wayPoints
     * automatically assigned in order 
     */
    private Transform[] wayPoints;

    /* nextWayPoints
     * tracks the next waypoint for each enemy
     */
    private int[] nextWaypoint;

    //Awake
    //on start script
    protected override void myAwake()
    {
        //class and component initialization
        wayPoints = waypointControl.GetComponentsInChildren<Transform>();
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyPatrol>().wayPointMaster = GameObject;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /* hitWaypoint
     * called by onTriggerEnter
     * contains functions to execute on collision with a waypoint
     */
    public void hitWaypoint()
    {
        foreach (string arg in invokingArgs) {
            invoke(arg, 0);
        }
    }

    /* onCollide()
     * 
     */
    public void onCollide()
    {
        //MOVE COLLISION LOGIC HERE
        if (GameObject.ReferenceEquals(other.transform.gameObject, wayPoints[nextWayPoint].gameObject))
        {
            if ((nextWayPoint == waypointControl.transform.childCount || nextWayPoint == 1) && !patrolLoop)
            {
                reenableWaypoints();
                patrolDirection = !patrolDirection;
            }
            else if (nextWayPoint == waypointControl.transform.childCount && patrolLoop)
            {
                reenableWaypoints();
                nextWayPoint = 0;
            }
            else
                other.transform.gameObject.SetActive(false);

            if (patrolDirection)
                ++nextWayPoint;
            else
                --nextWayPoint;
            messageReceiver = new Message<int>(message_type.nextWaypoint);
        }
    }

    //moveTowardsNext
    //move towards next waypoint in wayPoints
    public IEnumerator moveTowardsNext()
    {
        Rigidbody2D myRigidbody = invokingEnemy.transform.GetComponent<Rigidbody2D>();
        float waitTime = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitTime;
        float waitToRotate = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitToRotate;
        myRigidbody.velocity = new Vector3(0, 0, 0);

        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);
        StartCoroutine(RotateTo(wayPoints[nextWayPoint].transform.position, 0));
        if (waitToRotate > 0)
            yield return new WaitForSeconds(waitToRotate);
        myRigidbody.velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
    }

    //rotateTowardsNext
    //rotates towards next waypoint if enemy patrol is stationary
    public IEnumerator rotateTowardsNext()
    {
        float waitTime = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitTime;
        float waitToRotate = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitToRotate;
        StartCoroutine(RotateTo(wayPoints[nextWayPoint].transform.position, 0));

        if (nextWayPoint == waypointControl.transform.childCount || nextWayPoint == 1)
        {
            patrolDirection = !patrolDirection;
        }

        if (patrolDirection)
            ++nextWayPoint;
        else
            --nextWayPoint;

        yield return new WaitForSeconds(waitToRotate);
        messageReceiver = new Message<Vector3>(message_type.nextWaypoint);
    }

    //disableWaypoints
    //disables all waypoints
    public void disableWaypoints()
    {
        foreach (Transform waypoint in wayPoints)
        {
            waypoint.gameObject.SetActive(false);
        }
    }

    //reenableWaypoints
    //reenables all waypoints at end of chain
    public void reenableWaypoints()
    {
        foreach (Transform waypoint in wayPoints)
        {
            waypoint.gameObject.SetActive(true);
        }
    }
}
