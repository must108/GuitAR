using UnityEngine;

namespace Core3lb
{
    public class DoBounce : MonoBehaviour
    {

        Vector3 holder;
        public float amount = 3;
        public float timeScale = 5;
        public bool randomTimeScale;
        // Use this for initialization
        public virtual void Start()
        {
            if (randomTimeScale)
            {
                timeScale += Random.Range(-2, 2);
            }
        }

        // Update is called once per frame
        public virtual void FixedUpdate()
        {
            holder = transform.position;
            holder.y += Mathf.Sin(Time.time * timeScale) * amount;
            transform.position = holder;
        }
    }
}
