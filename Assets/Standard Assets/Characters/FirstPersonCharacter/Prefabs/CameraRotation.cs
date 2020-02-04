using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationTest : MonoBehaviour
{
    public float minRotationAroundX;
    public float minRotationAroundY;
    private void LateUpdate() {

        this.transform.rotation = Quaternion.AngleAxis(Input.GetAxis("Vertical") * minRotationAroundX, Vector3.right);
        this.transform.rotation = Quaternion.AngleAxis(Input.GetAxis("Horizontal") * minRotationAroundY, Vector3.up);
        
    }
}
