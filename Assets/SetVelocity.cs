using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour
{
    public float vX = 1;
    public float vY = 1;
    private Rigidbody2D _rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        _rigidBody.velocity = new Vector2(vX, vY);
        _rigidBody.rotation = Mathf.Rad2Deg * Mathf.Atan(vY / vX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
