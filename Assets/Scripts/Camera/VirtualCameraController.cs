using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    CinemachineVirtualCamera _vCam;

    public int curPriority = 5;
    public int activePriority = 20;

    private void Awake()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _vCam.Priority = activePriority;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _vCam.Priority = curPriority;
        }
    }
}
