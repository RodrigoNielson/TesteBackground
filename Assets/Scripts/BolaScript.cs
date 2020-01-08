using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaScript : MonoBehaviour
{
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rigidBody.velocity.x + " -- " + rigidBody.velocity.y);

        if (rigidBody.velocity.x > 25)
        {
            rigidBody.AddForce(new Vector2(15 - rigidBody.velocity.x, 0));
        }
        else if (rigidBody.velocity.x < 5)
        {
            rigidBody.AddForce(new Vector2(4, 0));
        }

        //if (rigidBody.velocity.y < 1)
        //{
        //    rigidBody.AddForce(new Vector2(0, 1- rigidBody.velocity.y));
        //}
    }
}
