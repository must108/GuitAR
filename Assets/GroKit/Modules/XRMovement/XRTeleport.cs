using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Core3lb
{
    public class XRTeleport : MonoBehaviour
    {
        //ONE HAND!
        [CoreHeader("Required")]
        [CoreRequired]
        public XRMovement movementRef;
        [CoreRequired]
        public GameObject indicator;
        [CoreRequired]
        public LineRenderer myLine;
        [CoreRequired]
        public Transform lineStartPoint;


        //Line
        Vector3[] lineArr;
        [CoreHeader("Targeting")]
        public bool usingNavMesh = true;
        [Tooltip("Layers You Can Teleport On"), CoreHideIf("usingNavMesh")]
        public LayerMask teleportLayers;
        [Tooltip("The Maximum Slope You Can Teleport On")]
        public float maxSurfaceAngle = 45;

        [Tooltip("The distance multiplier of the teleport line"), Min(0)]
        public float distanceMultiplier = 1;

        [Tooltip("The \"gravity\" of the teleport line"), Min(0)]
        public float curveStrength = 1;

        [CoreHeader("Settings")]
        [Tooltip("Maximum Length of The Teleport Line")]
        public int lineSegments = 50;

        [Tooltip("Use Worldspace Must be True")]
        public Gradient canTeleportColor = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey() { color = Color.green, time = 0 } } };

        [Tooltip("Sets the line renderer teleport color to this")]
        public Gradient cantTeleportColor = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey() { color = Color.red, time = 0 } } };

        public bool islookingForTarget;


        [Tooltip("This GameObject will match the position of the teleport point when aiming")]
        public Vector3 rotateDirection = new Vector3(0, 90, 0);

        [Tooltip("Rotate Speed for indicator")]
        public float rotateSpeed = 5;
        [Tooltip("Move Speed for indicator")]
        public float lerpSpeed = 5;

        [CoreReadOnly]
        public bool isValid;
        Vector3 teleportPos;


        [Tooltip("When Teleport has started searching")]
        public UnityEvent onTeleportStart;
        [Tooltip("When Teleport is successful")]
        public UnityEvent onTeleport;
        [Tooltip("Fizzle means it failed to go anywhere")]
        public UnityEvent onTeleportFizzle;
        public void Awake()
        {
            lineArr = new Vector3[lineSegments];
        }

        public virtual void Start()
        {
            movementRef = gameObject.GetComponentIfNull<XRMovement>(movementRef);
            Init();
        }

        public virtual void Init()
        {
            if (lineStartPoint == null)
            {
                Debug.LogError("Line Start Point must be defined");
            }
        }

        public virtual void LineStart()
        {
            myLine.transform.position = lineStartPoint.position;
            myLine.transform.rotation = lineStartPoint.rotation;
            myLine.transform.rotation *= Quaternion.Euler(rotateDirection);
        }

        public virtual void _StartTeleport()
        {
            LineStart();

            onTeleportStart.Invoke();
            islookingForTarget = true;
            myLine.enabled = true;
        }
        public virtual void _CancelTeleport()
        {
            onTeleportFizzle.Invoke();
            islookingForTarget = false;
            myLine.enabled = false;
            indicator.gameObject.SetActive(false);
        }

        public virtual void _StopTeleport()
        {
            if (isValid)
            {
                Teleport();
                onTeleport.Invoke();
            }
            else
            {
                onTeleportFizzle.Invoke();
            }
            islookingForTarget = false;
            myLine.enabled = false;
            indicator.gameObject.SetActive(false);
        }

        public virtual void Update()
        {
            if (islookingForTarget)
            {
                myLine.transform.position = Vector3.MoveTowards(myLine.transform.position, lineStartPoint.position, lerpSpeed * Time.deltaTime);
                myLine.transform.rotation = Quaternion.Slerp(myLine.transform.rotation, lineStartPoint.rotation * Quaternion.Euler(rotateDirection), rotateSpeed * Time.deltaTime);
                CalculateTeleport(myLine);
            }
        }

        protected virtual void CalculateTeleport(LineRenderer line)
        {
            RaycastHit rayHit = new RaycastHit();
            NavMeshHit navHit = new NavMeshHit();

            line.colorGradient = cantTeleportColor;
            var lineList = new List<Vector3>();
            int i;
            for (i = 0; i < lineSegments; i++)
            {
                var time = i / 60f;
                lineArr[i] = line.transform.position;
                lineArr[i] += line.transform.forward * time * distanceMultiplier * 15;
                lineArr[i].y += curveStrength * (time - Mathf.Pow(9.8f * 0.5f * time, 2));
                lineList.Add(lineArr[i]);
                if (i != 0)
                {
                    var distance = Vector3.Distance(lineArr[i], lineArr[i - 1]);
                    if (usingNavMesh)
                    {
                        if (NavMesh.SamplePosition(lineArr[i - 1], out navHit, distance, 1 << NavMesh.GetAreaFromName("Walkable")))
                        {
                            if (Vector3.Angle(navHit.normal, Vector3.up) <= maxSurfaceAngle)
                            {
                                line.colorGradient = canTeleportColor;
                                lineList.Add(navHit.position);
                                isValid = true;
                                break;
                            }
                            break;
                        }
                    }
                    else if (Physics.Raycast(lineArr[i - 1], lineArr[i] - lineArr[i - 1], out rayHit, distance, teleportLayers))
                    {
                        //Makes sure the angle isnt too steep
                        if (Vector3.Angle(rayHit.normal, Vector3.up) <= maxSurfaceAngle)
                        {
                            line.colorGradient = canTeleportColor;
                            lineList.Add(rayHit.point);
                            isValid = true;
                            break;
                        }
                        break;
                    }
                }
            }
            line.positionCount = i;
            line.SetPositions(lineArr);
            if (usingNavMesh)
            {
                rayHit.normal = navHit.normal;
                rayHit.point = navHit.position;
            }
            DrawIndicator(rayHit);
        }

        protected virtual void DrawIndicator(RaycastHit aimHit)
        {
            if (indicator != null)
            {
                if (isValid)
                {
                    indicator.gameObject.SetActive(true);
                    indicator.transform.position = aimHit.point;
                    indicator.transform.up = aimHit.normal;
                    teleportPos = aimHit.point;
                }
                else
                {
                    indicator.gameObject.SetActive(false);
                }

            }
        }

        public virtual void Teleport()
        {
            if (isValid)
            {
                if (movementRef != null)
                    movementRef._MovePlayerSafely(teleportPos);
            }

        }
    }
}
