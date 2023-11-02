using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraExploration : MonoBehaviour
{
    private Transform player;
    private Vector3 offset;
    private float smoothTime;
    private Vector3 velocity;

    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;

    [SerializeField] private SceneTransition sceneTransition;

    private void Start()
    {
        transform.position = sceneTransition.camPrevPosition;

        player = GameObject.FindWithTag("Player").transform;

        offset = new(0f, 0f, -10f);
        smoothTime = 0.25f;
        velocity = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (transform.position != player.position)
        {
            Vector3 targetPosition = player.position + offset;
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x); //set x boundary
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y); //set y boundary
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
