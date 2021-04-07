using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float SpeedPlayer = 7f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;
        var movementPlayerY = Input.GetAxis("Vertical") * Time.deltaTime * SpeedPlayer;

        if (movementPlayerX != 0 || movementPlayerY > 0)
            transform.Translate(movementPlayerX, movementPlayerY, 0f);
    }
}