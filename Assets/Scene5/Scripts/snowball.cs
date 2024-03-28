using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowball : MonoBehaviour
{
    float distx;
    float distz;

    [SerializeField] GameObject player;
    [SerializeField] float snowballSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distx = Mathf.Abs(player.transform.position.x - transform.position.x);
        distz = Mathf.Abs(player.transform.position.z - transform.position.z);

        transform.position += transform.forward * Time.deltaTime * snowballSpeed;

        if (distx > 200f || distz > 200f)
        {
            Destroy(gameObject);
            Debug.Log("destroyed");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "target")
        {
            //targetNum -= 1;
            Destroy(other.gameObject);
            // target.text = "Targets Remaining: " + targetNum;
            //Debug.Log(targetNum);
        }
    }
}
