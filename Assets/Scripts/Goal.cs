using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(deplacement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Table")
        {
            Debug.LogError("Table");
            GetComponent<Rigidbody>().useGravity = false;
            transform.parent.GetComponentInChildren<AI_PingPongFinal>().OnBallTouchingAllySide();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if(collision.gameObject.tag == "Wrong"||collision.gameObject.tag == "Wall")
        {
            Debug.LogWarning("WrongOrWall");
            GetComponent<Rigidbody>().useGravity = false;
            transform.parent.GetComponentInChildren<AI_PingPongFinal>().OnBallTouchingAllySide();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
    private IEnumerator deplacement()
    {
        float randX, randY, randZ;
        randX = Random.Range(-1.75f,1.75f);
        randY = Random.Range(-1.75f,1.75f);
        randZ = Random.Range(-1.75f,1.75f);
        RaycastHit hit;
        float speed = Random.Range(0.5f,2f);
        float step =  speed * Time.deltaTime;
        if(Physics.Raycast(gameObject.transform.position,transform.TransformDirection(new Vector3(randX,randY,randZ)),out hit, Mathf.Infinity))
        {
            Debug.LogError("hit");
            Vector3.MoveTowards(gameObject.transform.position, new Vector3(randX, randY, randZ), step);
        }
        else
        {
            Debug.LogError("nohit");
        }
        yield return new WaitForSeconds(2.5f);
    }
}
