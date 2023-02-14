using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoppingScript : MonoBehaviour
{

    [Header("Bop Settings: ")] 
    [SerializeField] private float bopSpeed = 1f;
    [SerializeField] private float bopAmount = 0.1f;

    private float _bopTimer = 0f;

    // Update is called once per frame
    private void Update()
    {
        var position = transform.position;
        _bopTimer += Time.deltaTime * bopSpeed;
        var yPos = Mathf.Sin(_bopTimer) * bopAmount;

        if (position.y + yPos < 0.1)
        {
            yPos = 0.1f - position.y;
        }
        
        position = new Vector3(position.x, yPos, position.z);
        transform.position = position;
    }
}
