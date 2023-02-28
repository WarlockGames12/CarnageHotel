using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class LookAtPlayer : MonoBehaviour
{

    private Transform lookPlayer;
    private float speed = 1f;

    private void Start()
    {
        lookPlayer = GameObject.FindWithTag("Player").transform;
    }
    
    // Update is called once per frame
    private void Update()
    {
        var position = lookPlayer.position;
        var position1 = transform.position;
        
        var targetPos = new Vector3(position.x, position1.y, position.z);
        var direction = targetPos - position1;
        var rotations = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotations, speed * Time.deltaTime);
    }
}
