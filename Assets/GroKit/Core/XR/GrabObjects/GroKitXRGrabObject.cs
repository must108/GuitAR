using UnityEngine;
using System.Collections.Generic;

namespace Core3lb
{
    public class GroKitXRGrabObject : BaseXRGrabObject
    {
        //This is a Simplfied Generic XRGrab Object it does not require a rigidbody If you are looking for something with more advanced features use the XRI or Oculus GrabSystems
        //It uses a fake parenting system to attach the object to hand setting it to Kinemantic on Grab

        [CoreHeader("Grokit Grab Object")]
        [Tooltip("Throwing Requires Rigidbody disable throwing by setting these to zero adjust for you preference")]
        public float throwMulti = 1;
        [Tooltip("Adds or subtracts spin from your throw")]
        public float angularMulti = 1;

        //Parenting Calculations
        Vector3 startParentPosition;
        Quaternion startParentRotationQ;
        Vector3 startParentScale;

        Vector3 startChildPosition;
        Quaternion startChildRotationQ;
        Vector3 startChildScale;

        Matrix4x4 parentMatrix;

        public bool overrideMovement;

        [CoreToggleHeader("Allow Stealing")]
        public bool allowStealing = false;
        [CoreShowIf("allowStealing")]
        [CoreReadOnly]
        public XRHand thiefHand;

        [CoreToggleHeader("Velocity Follow")]
        public bool useVelFollow;

        [Tooltip("This can help with jitter")]
        [CoreShowIf("useVelFollow")]

        public float followVelocityAttenuation = .5f;
        [CoreShowIf("useVelFollow")]
        public float maxVel = 10;

        public void Awake()
        {
            //if no Body it will ignore throwing
            body = GetComponent<Rigidbody>();
            if(useVelFollow)
            {
                if(body == null)
                {
                    useVelFollow = false;
                }
                body.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out XRHand curHand))
            {
                if(currentHand == null)
                {
                    currentHand = curHand;
                    EnterEvent();
                }
                else
                {
                    if (currentHand != curHand)
                    {
                        thiefHand = curHand;
                    }
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (!isGrabbed)
            {
                if (other.TryGetComponent(out XRHand curHand))
                {
                    if (currentHand == curHand)
                    {
                        currentHand = null;
                        ExitEvent();
                    }
                    if(allowStealing)
                    {
                        if (thiefHand == curHand)
                        {
                            thiefHand = null;
                        }
                    }
                }
            }
        }

        public virtual bool GetGrabInputDown(XRHand whichHand,bool ignoreSphere = false)
        {
            if(whichHand == null)
            {
                return false;
            }
            if (whichHand.GrabProcessor.isDown && !ignoreSphere)
            {
                if (whichHand.SphereCheck(gameObject))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return whichHand.GrabProcessor.isDown;
        }


        public virtual bool GetGrabInput(XRHand whichHand)
        {
            if (whichHand == null)
            {
                return false;
            }
            return whichHand.GrabProcessor.getState;
        }



        public virtual void Update()
        {
            if (currentHand)
            {   
                if (GetGrabInputDown(currentHand))
                {
                    Grab();
                }
            }
            if (isGrabbed)
            {
                if (GetGrabInput(currentHand) == false)
                {
                    Drop();
                    return;
                }
                if(allowStealing)
                {
                    if (GetGrabInputDown(thiefHand))
                    {
                        currentHand.currentHeldObject = null;
                        currentHand = thiefHand;
                        Grab();
                        thiefHand = null;
                    }
                }
                if (overrideMovement)
                {
                    return;
                }
                TrackMovement();
                FollowHand();
            }
        }

        public void FixedUpdate()
        {
            if (overrideMovement)
            {
                return;
            }
            if (isGrabbed && useVelFollow && !overrideMovement)
            {
                VelFollow(Time.fixedDeltaTime);
            }
        }

        public void VelFollow(float fixedTime)
        {
            if (body)
            {
                body.isKinematic = false;
                Vector3 pos;
                Quaternion rot;
                FollowHandOut(out pos, out rot);
                body.VelocityFollow(pos, rot, fixedTime);
                body.velocity *= followVelocityAttenuation; // followVelocityAttenuation = 0.5F by default
                body.velocity = Vector3.ClampMagnitude(body.velocity, maxVel); // maxVelocity = 10f by default
            }
        }


        //This is used to ensure that if the object is disabled while grabbed it will drop
        public void OnDisable()
        {
            if(isGrabbed)
            {
                ForceDrop();
            }
        }

        public void FollowHand()
        {
            if(useVelFollow)
            {
                return;
            }
            else
            {
                parentMatrix = Matrix4x4.TRS(currentHand.transform.position, currentHand.transform.rotation, currentHand.transform.lossyScale);
                transform.position = parentMatrix.MultiplyPoint3x4(startChildPosition);
                transform.rotation = (currentHand.transform.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ;
            }
        }

        public void VelFollowOut(float fixedTime,out Vector3 velocity,Vector3 angular)
        {
            Vector3 pos;
            Quaternion rot;
            FollowHandOut(out pos, out rot);
            body.VelocityFollowOut(pos, rot, fixedTime,out velocity,out angular);
        }

        public void FollowHandOut(out Vector3 position, out Quaternion rotation)
        {
            parentMatrix = Matrix4x4.TRS(currentHand.transform.position, currentHand.transform.rotation, currentHand.transform.lossyScale);
            position = parentMatrix.MultiplyPoint3x4(startChildPosition);
            rotation = (currentHand.transform.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ;
        }

        public override void Drop()
        {
            isGrabbed = false;
            currentHand.currentHeldObject = null;
            //Only deal with rigidbody if exists
            if (body)
            {
                if (!kinematicOnDrop)
                {
                    body.isKinematic = false;
                }
                DoThrow();
            }
            base.Drop();
        }

        public void StealHand()
        {
            isGrabbed = false;
        }

        public override void ForceDrop()
        {
            if (isGrabbed)
            {
                Drop();
                currentHand = null;
            }
        }

        public override void ForceGrab(XRHand selectedHand)
        {
            currentHand = selectedHand;
            Grab();
        }

        public override void Grab()
        {
            if(isLocked || currentHand.currentHeldObject)
            {
                return;
            }
            isGrabbed = true;
            currentHand.currentHeldObject = this;
            StartParent();
            if (body)
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                body.isKinematic = true;
            }
            base.Grab();
        }

        void StartParent()
        {
            //Store values for Matrix
            startParentPosition = currentHand.transform.position;
            startParentRotationQ = currentHand.transform.rotation;
            startParentScale = currentHand.transform.lossyScale;
            startChildPosition = transform.position;
            startChildRotationQ = transform.rotation;
            startChildScale = transform.lossyScale;

            // Keeps child position from being modified at the start by the parent's initial transform
            startChildPosition = DivideVectors(Quaternion.Inverse(currentHand.transform.rotation) * (startChildPosition - startParentPosition), startParentScale);
        }

        #region Throwing Calcuations
        //#######################
        //Throw Calcuations

        public Queue<Vector3> positionQueue = new Queue<Vector3>();
        public Queue<Quaternion> rotationQueue = new Queue<Quaternion>();
        private int maxFrameCount = 10;


        public virtual void TrackMovement()
        {
            // Add the current position and rotation to the queues
            positionQueue.Enqueue(transform.position);
            rotationQueue.Enqueue(transform.rotation);

            // Ensure queues only contain the last 10 frames of data
            if (positionQueue.Count > maxFrameCount)
            {
                positionQueue.Dequeue();
                rotationQueue.Dequeue();
            }
        }

        public virtual Vector3 CalculateAverageChange()
        {
            if (positionQueue.Count == 0)
            { return Vector3.zero; }
            Vector3 sumOfChanges = Vector3.zero;
            Vector3 previous = positionQueue.Peek();

            foreach (Vector3 current in positionQueue)
            {
                sumOfChanges += current - previous;
                previous = current;
            }

            return sumOfChanges / positionQueue.Count;
        }

        public virtual Vector3 CalculateAngularVelocity()
        {
            //Quaternion sumOfDeltaRotations = Quaternion.identity;
            //Quaternion previousRotation = rotationQueue.Peek();

            //foreach (Quaternion currentRotation in rotationQueue)
            //{
            //    Quaternion deltaRotation = currentRotation * Quaternion.Inverse(previousRotation);
            //    sumOfDeltaRotations *= deltaRotation;
            //    previousRotation = currentRotation;
            //}

            //// Convert the summed rotation to angular velocity
            //return new Vector3(sumOfDeltaRotations.x, sumOfDeltaRotations.y, sumOfDeltaRotations.z);
            
            //new version
            if(rotationQueue.Count == 0)
            {
                return Vector3.zero;
            }
            Quaternion previousRotation = rotationQueue.Peek();
            Vector3 sumOfChanges = Vector3.zero;

            foreach (Quaternion currentRotation in rotationQueue)
            {
                Vector3 angularVelocity = GetAngularVelocity(currentRotation, previousRotation);
                sumOfChanges += angularVelocity;
                previousRotation = currentRotation;
            }

            return sumOfChanges / rotationQueue.Count;
        }

        Vector3 GetAngularVelocity(Quaternion currentFrameRotation, Quaternion lastFrameRotation)
        {
            Quaternion deltaRotation = currentFrameRotation * Quaternion.Inverse(lastFrameRotation);

            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

            angle *= Mathf.Deg2Rad;

            return (1.0f / Time.deltaTime) * angle * axis;
        }

        public virtual void DoThrow()
        {
            if (!body)
            {
                //No RigidBody you cannot be thrown;
                return;
            }
            Vector3 initialVelocity = CalculateAverageChange() / Time.fixedDeltaTime;
            Vector3 angularVelocity = CalculateAngularVelocity();
            //Add force gives a smoother throw
            if (initialVelocity.magnitude > 0.1f)
            {
                body.AddForce(100 * throwMulti * initialVelocity);
                body.AddTorque(angularMulti * angularVelocity, ForceMode.VelocityChange);
                positionQueue.Clear();
                rotationQueue.Clear();
            }
        }

        Vector3 DivideVectors(Vector3 num, Vector3 den) { return new Vector3(num.x / den.x, num.y / den.y, num.z / den.z); }
        #endregion

    }
}
