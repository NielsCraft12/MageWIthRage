using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(cam.transform.position, Vector3.up);
    }
}
