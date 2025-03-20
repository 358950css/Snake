using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Collider foodArea;
    // Start is called before the first frame update
    void Start()
    {
        RandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        RandomPosition();
    }

    void RandomPosition()
    {
        transform.position = new Vector3(Random.Range(foodArea.bounds.min.x, foodArea.bounds.max.x), 
                                         Random.Range(foodArea.bounds.min.y, foodArea.bounds.max.y), 0);
    }
}
