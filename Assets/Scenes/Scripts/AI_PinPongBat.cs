using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AI_PinPongBat : Agent
{
    [SerializeField] private Transform targetBall;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private List<MeshRenderer> cubeMeshRenderer;
    //[SerializeField] private Collider targetEnnemyTable;
    //[SerializeField] private Collider targetNet;
    public override void OnEpisodeBegin()
    {
        float randX = Random.Range(-9f, 9f);
        float randY = Random.Range(-9f, 9f);
        float randZ = Random.Range(-9f, 9f);
        targetBall.localPosition = new Vector3(randX, randY, randZ);
        transform.localPosition = new Vector3(0,0,0);
        Debug.Log("new Start");
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);
        sensor.AddObservation(transform.localPosition.z);
        sensor.AddObservation(targetBall.localPosition.x);
        sensor.AddObservation(targetBall.localPosition.y);
        sensor.AddObservation(targetBall.localPosition.z);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float moveZ = actions.ContinuousActions[2];
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

    void OnTriggerEnter(Collider collision)

    {
        Debug.Log("insidesomtng");
        if (collision.gameObject.tag == "Goal")
        {
            Debug.Log("insideGoal");
            SetReward(+1f);
            for (int i = 0; i < cubeMeshRenderer.Count; i++)
            {
                cubeMeshRenderer[i].material = winMaterial;
            }
            EndEpisode();
        }
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("insideWall");
            SetReward(-1f);
            for (int i = 0; i < cubeMeshRenderer.Count; i++)
            {
                cubeMeshRenderer[i].material = loseMaterial;
            }
            EndEpisode();
        }
    }
}
