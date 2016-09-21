using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Collider groundCollider;
    [SerializeField]
    private GameObject selectBox;
    public List<GameObject> tracks = new List<GameObject>();

    private float edgeScrollDistance = 60;

    private GameObject currentTrack;

    void Awake()
    {
        currentTrack = CreateRandomTrack();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update ()
    {
        int extraEdgeOffset = 1;
        #if UNITY_EDITOR
        extraEdgeOffset = 0;
        #endif

        #region movement
        Vector3 moveDelta = new Vector3(Input.GetAxis("Horizontal"), -Input.GetAxis("Mouse ScrollWheel") * 10, Input.GetAxis("Vertical"));
        // edge scroll
        if (Input.mousePosition.x > Screen.width - edgeScrollDistance && Input.mousePosition.x < Screen.width + extraEdgeOffset)
            moveDelta += new Vector3(1, 0, 0);
        if (Input.mousePosition.x < edgeScrollDistance && Input.mousePosition.x > -extraEdgeOffset)
            moveDelta += new Vector3(-1, 0, 0);
        if (Input.mousePosition.y > Screen.height - edgeScrollDistance && Input.mousePosition.y < Screen.height + extraEdgeOffset)
            moveDelta += new Vector3(0, 0, 1);
        if (Input.mousePosition.y < edgeScrollDistance && Input.mousePosition.y > -extraEdgeOffset)
            moveDelta += new Vector3(0, 0, -1);

        moveDelta *= Time.deltaTime * moveSpeed;
        transform.position += moveDelta;
        #endregion

        Ray cameraToScreenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(groundCollider.Raycast(cameraToScreenRay, out hit, 1000))
        {
            selectBox.transform.position = new Vector3((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
            currentTrack.transform.position = new Vector3((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
            Debug.Log(hit.point);
        }

        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
        {
            currentTrack.transform.GetChild(0).localRotation *= Quaternion.Euler(new Vector3(0, 0, 90));
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentTrack.transform.GetChild(0).localRotation *= Quaternion.Euler(new Vector3(0, 0, -90));
        }

        if(Input.GetMouseButtonDown(0))
        {
            currentTrack.transform.position = new Vector3((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
            currentTrack = CreateRandomTrack();
        }
    }

    private GameObject CreateRandomTrack()
    {
        return Instantiate(tracks[Random.Range(0, tracks.Count)], Vector3.zero, Quaternion.identity) as GameObject;
    }
}
