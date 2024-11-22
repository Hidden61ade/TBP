using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool interactable = true;

    public float speed;

    private Rigidbody rb;
    private Transform Camera;
    private bool cursorLocked = false;
    private void OnEnable() {
        interactable = true;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Camera = transform.Find("Camera");
    }
    private void Update()
    {
        //Lock Cursor
        if ((!cursorLocked) && interactable)
        {
            if (Input.GetMouseButton(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                cursorLocked = true;
            }
            else if (cursorLocked)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }

        //Move
        float t_x = Input.GetAxisRaw("Horizontal");
        float t_z = Input.GetAxisRaw("Vertical");
        var t_move = new Vector3(t_x, 0, t_z);
        t_move = transform.TransformDirection(t_move);
        rb.velocity = t_move * speed;


    }
    private void LateUpdate()
    {
        //View
        float v_x = Input.GetAxisRaw("Mouse X");
        float v_y = Input.GetAxisRaw("Mouse Y");
        Quaternion adjx = Quaternion.AngleAxis(v_x * 1.2f, Vector3.up);
        Quaternion adjy = Quaternion.AngleAxis(-v_y * 1.2f, Vector3.right);
        transform.localRotation *= adjx;
        Camera.localRotation *= adjy;
    }
}