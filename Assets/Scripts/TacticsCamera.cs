using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsCamera : MonoBehaviour
{
    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, -90, Space.Self);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            RotateLeft();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            RotateRight();
        }
    }
}
