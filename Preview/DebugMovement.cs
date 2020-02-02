using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace zadanie1
{
    public class DebugMovement : MonoBehaviour
    {
        [SerializeField]
        public UnityEvent collisionEvent;
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private Transform velocityMarker;

        void FixedUpdate()
        {
            Move(GetMovementTargetDir());
        }
        Vector3 GetMovementTargetDir()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(x,y,0);
            if (dir.sqrMagnitude > 1) dir = dir.normalized;
            return dir;
        }
        void Move(Vector3 targetDir)
        {
            if (targetDir != Vector3.zero)
            {
                rb.AddForce(targetDir * movementSpeed * Time.fixedDeltaTime, ForceMode.Force);
            }
            SetVelocityMarker(rb.velocity);
        }

        void SetVelocityMarker(Vector3 offset)
        {
            velocityMarker.localPosition = offset;
        }

        void OnCollisionEnter(Collision coll)
        {
            if(coll.relativeVelocity.sqrMagnitude > 2f) collisionEvent.Invoke();
        }
    }
}