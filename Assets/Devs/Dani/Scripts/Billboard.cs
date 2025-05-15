using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(cam.transform.position, Vector3.up);
    }
}
