using Interactable;
using QFramework;
using UnityEngine;


public class PlayerController : MonoSingleton<PlayerController>
{
    public bool interactable = true;

    public float speed;

    private Rigidbody rb;
    private Transform Camera;
    private bool cursorLocked = false;
    #region Monobehaviour Callbacks
    // private void OnEnable()
    // {
    //     interactable = true;
    // }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Camera = transform.Find("Camera");
        TypeEventSystem.Global.Register<InteractStart>(e => { interactable = false; }).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<InteractEnd>(e => { interactable = true; }).UnRegisterWhenGameObjectDestroyed(gameObject);
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
        if (!interactable) return;
        //Move
        float t_x = Input.GetAxisRaw("Horizontal");
        float t_z = Input.GetAxisRaw("Vertical");
        var t_move = new Vector3(t_x, 0, t_z);
        t_move = transform.TransformDirection(t_move);
        rb.velocity = t_move * speed;

        CheckInteract();
    }
    private void LateUpdate()
    {
        if (!interactable) return;
        //View
        float v_x = Input.GetAxisRaw("Mouse X");
        float v_y = Input.GetAxisRaw("Mouse Y");
        Quaternion adjx = Quaternion.AngleAxis(v_x * 1.2f, Vector3.up);
        Quaternion adjy = Quaternion.AngleAxis(-v_y * 1.2f, Vector3.right);
        transform.localRotation *= adjx;
        Camera.localRotation *= adjy;
    }
    #endregion
    void CheckInteract()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, InteractHintManager.Instance.Range))
        {
            if (hit.collider.gameObject.CompareTag("InteractableObjects"))
            {
                var a = hit.collider.gameObject.GetComponent<InteractableObject>();
                a.OnHover();
                if (Input.GetMouseButtonDown(0))
                {
                    a.OnTriggered();
                }
            }
        }
    }
}