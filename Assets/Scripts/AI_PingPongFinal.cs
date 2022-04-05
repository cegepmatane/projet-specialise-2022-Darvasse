using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AI_PingPongFinal : Agent
{
    [SerializeField] private Transform targetBall;
    [SerializeField] private Collider targetTableColl;
    [SerializeField] private Collider netColl;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private Material finalMaterial;
    [SerializeField] private List<MeshRenderer> cubeMeshRenderer;
    bool alreadyEntered = false;
    //[SerializeField] private Collider targetEnnemyTable;
    //[SerializeField] private Collider targetNet;
    public override void OnEpisodeBegin()
    {
        float randX = Random.Range(-2.5f, 1.5f);
        float randY = Random.Range(-1f, -4f);
        float randZ = Random.Range(-2.7f, 2.7f);
        targetBall.localPosition = new Vector3(randX, randY, randZ);
        transform.localPosition = new Vector3(-6, -3.5f, 0);
        Debug.Log("new Start");
        GetComponent<Collider>().enabled = true;
        alreadyEntered = false;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);
        sensor.AddObservation(transform.localPosition.z);
        sensor.AddObservation(targetBall.localPosition.x);
        sensor.AddObservation(targetBall.localPosition.y);
        sensor.AddObservation(targetBall.localPosition.z);
        sensor.AddObservation(targetTableColl.bounds.min.x - transform.parent.position.x);
        sensor.AddObservation(targetTableColl.bounds.min.z - transform.parent.position.z);
        sensor.AddObservation(targetTableColl.bounds.max.x - transform.parent.position.x);
        sensor.AddObservation(targetTableColl.bounds.max.z - transform.parent.position.z);
        sensor.AddObservation(netColl.bounds.min.x - transform.parent.position.x);
        sensor.AddObservation(netColl.bounds.min.y - transform.parent.position.y);
        sensor.AddObservation(netColl.bounds.max.x - transform.parent.position.x);
        sensor.AddObservation(netColl.bounds.max.y - transform.parent.position.y);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float moveZ = actions.ContinuousActions[2];
        //float movespeed = actions.ContinuousActions[3];
        float moveSpeed = 4f;
        transform.localPosition += new Vector3(moveX, moveY, moveZ) * Time.deltaTime * moveSpeed;
        /*float rotateX = actions.ContinuousActions[3];
        float rotateY = actions.ContinuousActions[4];
        float rotateZ = actions.ContinuousActions[5];
        transform.localRotation = Quaternion.Euler(rotateX, rotateY, rotateZ);*/
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        continuousAction[0] = Input.GetAxisRaw("Horizontal");
        continuousAction[1] = Input.GetAxisRaw("Vertical");
        continuousAction[2] = Input.GetAxisRaw("UpDown");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("insidesomtng");
        if (collision.gameObject.tag == "Goal"&&!alreadyEntered)
        {
            alreadyEntered = true;
            Debug.Log("insideGoal");
            AddReward(+5f);
            for (int i = 0; i < cubeMeshRenderer.Count; i++)
            {
                cubeMeshRenderer[i].material = winMaterial;
            }
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("insideWall");
            AddReward(-1f);
            for (int i = 0; i < cubeMeshRenderer.Count; i++)
            {
                cubeMeshRenderer[i].material = loseMaterial;
            }
            transform.parent.GetComponentInChildren<Goal>().gameObject.GetComponent<Rigidbody>().useGravity = false;
            Debug.Log("End Episode");
            EndEpisode();
        }
    }
    public void OnBallTouchingEnemySide()
    {
        AddReward(+400f);
        for (int i = 0; i < cubeMeshRenderer.Count; i++)
        {
            cubeMeshRenderer[i].material = finalMaterial;
        }
        targetBall.gameObject.GetComponent<Rigidbody>().useGravity = false;
        Debug.Log("End Episode");
        EndEpisode();
    }
    public void OnBallTouchingAllySide()
    {
        AddReward(-2f);
        for (int i = 0; i < cubeMeshRenderer.Count; i++)
        {
            cubeMeshRenderer[i].material = loseMaterial;
        }
        targetBall.gameObject.GetComponent<Rigidbody>().useGravity = false;
        Debug.Log("End Episode");
        EndEpisode();
    }
}
