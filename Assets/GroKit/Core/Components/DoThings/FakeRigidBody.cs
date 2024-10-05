using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core3lb
{
    public class FakeRigidBody : MonoBehaviour
    {

        // Fake Rigidbody will react like a Rigidbody but is not one. It will go through objects, so best to set them as triggers.

        // Kinematic state
        public bool isKinematic = false;

        // Use Unity's gravity
        public bool useGravity = true;

        // Damping factors
        public float linearDrag = 0.1f;
        public float angularDrag = 0.05f;

        // Mass of the object
        public float mass = 1.0f;

        // Linear motion variables
        private Vector3 velocity = Vector3.zero;
        private Vector3 acceleration = Vector3.zero;

        // Angular motion variables
        private Vector3 angularVelocity = Vector3.zero;
        private Vector3 angularAcceleration = Vector3.zero;

        [CoreButton]
        public void _DoBlast()
        {
            _AddForce(Vector3.forward * 100);
        }

        void FixedUpdate()
        {
            if (!isKinematic)
            {
                // Apply gravity if enabled
                if (useGravity)
                {
                    acceleration += Physics.gravity;
                }

                // Linear motion
                velocity += acceleration * Time.fixedDeltaTime;

                // Apply linear drag
                velocity *= Mathf.Clamp01(1.0f - linearDrag * Time.fixedDeltaTime);

                transform.position += velocity * Time.fixedDeltaTime;

                // Angular motion
                angularVelocity += angularAcceleration * Time.fixedDeltaTime;

                // Apply angular drag
                angularVelocity *= Mathf.Clamp01(1.0f - angularDrag * Time.fixedDeltaTime);

                Quaternion deltaRotation = Quaternion.Euler(angularVelocity * Time.fixedDeltaTime);
                transform.rotation = transform.rotation * deltaRotation;

                // Reset accelerations for the next frame
                acceleration = Vector3.zero;
                angularAcceleration = Vector3.zero;
            }
        }

        // Add linear force
        public void _AddForce(Vector3 force)
        {
            if (!isKinematic)
            {
                // Apply force considering the mass
                acceleration += force / mass;
            }
        }

        // Add angular force (torque)
        public void _AddTorque(Vector3 torque)
        {
            if (!isKinematic)
            {
                // Apply torque considering the mass
                angularAcceleration += torque / mass;
            }
        }

        // Set kinematic state
        public void _SetKinematic(bool state)
        {
            isKinematic = state;
            if (isKinematic)
            {
                velocity = Vector3.zero;
                acceleration = Vector3.zero;
                angularVelocity = Vector3.zero;
                angularAcceleration = Vector3.zero;
            }
        }
    }
}