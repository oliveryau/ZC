using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBattle : MonoBehaviour
{
    private Camera cam;
    private Vector3 startPosition;
    private float startOrthoSize;
    private float duration;

    private void Start()
    {
        cam = Camera.main;
        startPosition = cam.transform.position;
        startOrthoSize = cam.orthographicSize;
        duration = 0.25f;
    }

    public IEnumerator ZoomIn(Transform pos, float targetOrthoSize = 5f)
    {
        Vector3 initialPosition = cam.transform.position;
        float initialOrthoSize = cam.orthographicSize;
        Vector3 targetPosition = new Vector3(pos.position.x, pos.position.y, cam.transform.position.z);

        if (cam.transform.position != targetPosition)
        {
            float elapsedTime = 0.0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                cam.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                cam.orthographicSize = Mathf.Lerp(initialOrthoSize, targetOrthoSize, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cam.transform.position = targetPosition;
            cam.orthographicSize = targetOrthoSize;
        }
    }

    public IEnumerator ZoomOut()
    {
        Vector3 initialPosition = cam.transform.position;
        float initialOrthoSize = cam.orthographicSize;

        if (cam.transform.position != startPosition)
        {
            float elapsedTime = 0.0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                cam.transform.position = Vector3.Lerp(initialPosition, startPosition, t);
                cam.orthographicSize = Mathf.Lerp(initialOrthoSize, startOrthoSize, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cam.transform.position = startPosition;
            cam.orthographicSize = startOrthoSize;
        }
    }
}
