using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 20f;
    public float screenBorder = 10;
    public Vector3 rotateValue = new Vector3();
    public float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - screenBorder)
        {
            currentPos.z += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= screenBorder)
        {
            currentPos.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= screenBorder)
        {
            currentPos.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - screenBorder)
        {
            currentPos.x += speed * Time.deltaTime;
        }
        float rotationHorizontal = Input.GetAxis("Horizontal");
        float rotationVertical = Input.GetAxis("Vertical");
        rotateValue.y = rotationHorizontal;
        rotateValue.x = -rotationVertical;
        transform.eulerAngles += rotateValue * rotationSpeed * Time.deltaTime;
        transform.position = currentPos;
   

    }
}
