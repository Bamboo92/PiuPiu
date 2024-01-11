using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCam : MonoBehaviour
{
    [SerializeField] List<GameObject> targets;

    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime = .5f;

    [SerializeField] float minZoom = 40f;
    [SerializeField] float maxZoom = 10f;
    [SerializeField] float zoomLimiter = 50f;

    [SerializeField]
    private float leftLimit;
    [SerializeField]
    private float rightLimit;
    [SerializeField]
    private float bottomLimit;
    [SerializeField]
    private float topLimit;

    private Vector3 velocity;
    [SerializeField]
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        AddTarget(GameObject.Find("Player1"));
        AddTarget(GameObject.Find("Player2"));
    }

    void LateUpdate()
    {
        if (targets.Count == 0){
            return;
        }

        transform.position = new Vector3
        (
            Math.Clamp(transform.position.x, leftLimit, rightLimit),
            transform.position.y,
            Math.Clamp(transform.position.z, bottomLimit, topLimit)
        );
    
        Move();
        Zoom();
    }

    public void AddTarget(GameObject newTarget)
    {
        if (newTarget != null)
        {
            targets.Add(newTarget);
        }
    }

    public void RemoveTarget(GameObject oldTarget)
    {
        if (oldTarget != null)
        {
            targets.Remove(oldTarget);
        }
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++){
            bounds.Encapsulate(targets[i].transform.position);
        }

        return bounds.size.x;
    }
    
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1){
            return targets[0].transform.position;
        }

        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++){
            bounds.Encapsulate(targets[i].transform.position);
        }

        return bounds.center;
    }

    private void OnDrawGizmos()
    {
        //draw a box around our camera boundary
        Gizmos.color = Color.green;

        //top boundary line
        Gizmos.DrawLine(new Vector3(leftLimit, 0, topLimit), new Vector3(rightLimit, 0, topLimit));
        //right boundary line
        Gizmos.DrawLine(new Vector3(rightLimit, 0, topLimit), new Vector3(rightLimit, 0, bottomLimit));
        //bottom boundary line
        Gizmos.DrawLine(new Vector3(rightLimit, 0, bottomLimit), new Vector3(leftLimit, 0, bottomLimit));
        //left boundary line
        Gizmos.DrawLine(new Vector3(leftLimit, 0, bottomLimit), new Vector3(leftLimit, 0, topLimit));
    }
}