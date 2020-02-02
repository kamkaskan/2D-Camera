using System.Collections.Generic;
using UnityEngine;
namespace kamkaskan.camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        float delay = 0.05f;
        [SerializeField]
        float maxZoom = 15f;
        [SerializeField]
        float minZoom = 7f;
        [SerializeField]
        float zoomLimit = 20f;
        [SerializeField]
        Camera cam;
        [SerializeField]
        Transform cameraTransform;
        [SerializeField]
        List<Transform> pointsOfInterest;
        Vector3 velocity = Vector3.zero;
        // Update is called once per frame
        void LateUpdate()
        {
            ResizeCamera();
            MoveCamera();
        }

        public void AddPointOfInterest(Transform point)
        {
            if (!pointsOfInterest.Contains(point)) pointsOfInterest.Add(point);
        }

        public void RemovePointOfInterest(Transform point)
        {
            if (pointsOfInterest.Contains(point)) pointsOfInterest.Remove(point);
        }

        void MoveCamera()
        {
            if (pointsOfInterest.Count == 0) return; 
            Vector3 newPosition = GetTargetPosition();
            newPosition.z = cameraTransform.localPosition.z;
            cameraTransform.localPosition = Vector3.SmoothDamp(cameraTransform.localPosition, newPosition, ref velocity, delay);
        }
        Vector3 GetTargetPosition()
        {
            Vector3 target = new Vector3();
            for(int i = 0; i < pointsOfInterest.Count; i++)
            {
                target += pointsOfInterest[i].position;
            }
            return target /= pointsOfInterest.Count;
        }

        void ResizeCamera()
        {
            if (pointsOfInterest.Count > 0) 
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, GetTargetZoom(), delay);
        }

        float GetTargetZoom()
        {
            Bounds bounds = new Bounds(pointsOfInterest[0].position, Vector3.zero);
            for(int i = 0; i < pointsOfInterest.Count; i++)
            {
                bounds.Encapsulate(pointsOfInterest[i].position);
            }
            float highestDistance = (bounds.size.x > bounds.size.y) ? bounds.size.x : bounds.size.y;
            return Mathf.Lerp(minZoom, maxZoom, highestDistance / zoomLimit);
        }
    }
}
