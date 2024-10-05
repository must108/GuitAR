using UnityEngine;

namespace Core3lb
{
    public class BasePositioner : MonoBehaviour
    {
        [CoreToggleHeader("Random Rotation")]
        public bool randomizeRotation;
        [CoreShowIf("randomizeRotation")]
        public bool yOnlyRotation;
        [CoreToggleHeader("Random Position")]
        public bool randomizePosition;

        public enum ePositionType { Sphere, Dome, Circle, None };
        [CoreShowIf("randomizePosition")]
        public ePositionType spreadType = ePositionType.None;
        [CoreShowIf("randomizePosition")]
        public float radius = 0;

        [CoreShowIf("randomizePosition")]
        public bool showGizmos = false;
        [CoreShowIf("showGizmos")]
        public Transform where;



        public virtual Vector3 WhatPosition(Transform whereNow = null)
        {
            if (whereNow == null)
            {
                return (CalculateSpread(transform.position));
            }
            return CalculateSpread(whereNow.position);
        }


        public virtual Quaternion WhatRotation(Transform whereNow = null)
        {
            Quaternion holder = Quaternion.identity;

            if (whereNow)
            {
                holder = whereNow.rotation;
            }
            else
            {
                holder = transform.rotation;
            }
            if (randomizeRotation)
            {
                holder = Random.rotation;
            }
            if (yOnlyRotation)
            {
                holder = Quaternion.Euler(new Vector3(whereNow.rotation.x, Random.Range(0, 360), whereNow.rotation.z));
            }
            return holder;
        }


        public virtual Vector3 CalculateSpread(Vector3 whereTo)
        {
            if (spreadType == ePositionType.None)
            {
                return whereTo;
            }
            Vector3 pos = whereTo;
            if (spreadType == ePositionType.Dome)
            {
                var holder = Random.insideUnitSphere;
                holder.y = Mathf.Abs(holder.y);
                pos = holder * radius + whereTo;
            }
            else
            {
                pos = Random.insideUnitSphere * radius + whereTo;
            }
            if (spreadType == ePositionType.Circle)
            {
                pos.y = whereTo.y;
            }
            return pos;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (showGizmos && spreadType != ePositionType.None)
            {
                if(where == null)
                {
                    where = transform;
                }
                Gizmos.color = Color.yellow;
                UnityEditor.Handles.color = Color.yellow;
                if (spreadType == ePositionType.Sphere)
                {

                    Gizmos.DrawWireSphere(where.position, radius);
                }
                if (spreadType == ePositionType.Circle)
                {
                    UnityEditor.Handles.DrawWireDisc(where.position, Vector3.up, radius);
                }
                if (spreadType == ePositionType.Dome)
                {
                    DrawDome(transform.position, radius);
                }
            }
        }
        void DrawDome(Vector3 position, float radius)
        {
            int longitudeLines = 20;
            int latitudeLines = 10;
            Gizmos.color = Color.yellow;
            float stepLong = 2 * Mathf.PI / longitudeLines;
            float stepLat = Mathf.PI / 2 / latitudeLines; // Only top half

            for (int lat = 0; lat <= latitudeLines; lat++)
            {
                float theta = lat * stepLat;
                float cosTheta = Mathf.Cos(theta);
                float sinTheta = Mathf.Sin(theta);

                // Draw horizontal circle at this latitude
                Vector3 prevPoint = position + new Vector3(radius * Mathf.Cos(0) * sinTheta, radius * cosTheta, radius * Mathf.Sin(0) * sinTheta);
                Vector3 firstPoint = prevPoint;
                for (int lon = 1; lon <= longitudeLines; lon++)
                {
                    float phi = lon * stepLong;
                    Vector3 point = position + new Vector3(radius * Mathf.Cos(phi) * sinTheta, radius * cosTheta, radius * Mathf.Sin(phi) * sinTheta);
                    Gizmos.DrawLine(prevPoint, point);
                    prevPoint = point;
                    if (lon == longitudeLines)
                    {
                        Gizmos.DrawLine(point, firstPoint);
                    }
                }
            }

            // Draw vertical lines
            for (int lon = 0; lon < longitudeLines; lon++)
            {
                float phi = lon * stepLong;
                Vector3 lowerPoint = position + new Vector3(radius * Mathf.Cos(phi) * Mathf.Sin(0), radius * Mathf.Cos(0), radius * Mathf.Sin(phi) * Mathf.Sin(0));
                for (int lat = 0; lat <= latitudeLines; lat++)
                {
                    float theta = lat * stepLat;
                    Vector3 point = position + new Vector3(radius * Mathf.Cos(phi) * Mathf.Sin(theta), radius * Mathf.Cos(theta), radius * Mathf.Sin(phi) * Mathf.Sin(theta));
                    Gizmos.DrawLine(lowerPoint, point);
                    lowerPoint = point;
                }
            }
        }
#endif
    }
}
