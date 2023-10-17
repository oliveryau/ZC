using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBattle : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private Vector3 startPosition;
    private float startOrthoSize;
    [SerializeField] private float targetOrthoSize;
    [SerializeField] private float duration;

    private void Start()
    {
        startPosition = cam.transform.position;
        startOrthoSize = cam.orthographicSize;
    }

    public IEnumerator ZoomInSingleTarget(Transform pos)
    {
        Vector3 targetPosition = new Vector3(pos.position.x, pos.position.y, cam.transform.position.z);
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            cam.orthographicSize = Mathf.Lerp(startOrthoSize, targetOrthoSize, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = targetOrthoSize;
    }

    public IEnumerator ZoomOut()
    {
        Vector3 initialPosition = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            cam.transform.position = Vector3.Lerp(initialPosition, startPosition, t);
            cam.orthographicSize = Mathf.Lerp(targetOrthoSize, startOrthoSize, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = startOrthoSize;
        cam.transform.position = startPosition;
    }
}
