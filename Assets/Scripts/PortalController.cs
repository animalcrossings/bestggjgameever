using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameObject playerTPblock;
    public GameObject blockTPblock;
    public GameObject end_TPblock;
    public Camera the_camera;
    private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cameraTP()
    {
        offset = the_camera.transform.position - transform.position;
        the_camera.transform.position = end_TPblock.transform.position + offset;
    }
}
