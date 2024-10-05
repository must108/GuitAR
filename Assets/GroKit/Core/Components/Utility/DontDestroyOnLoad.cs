using UnityEngine;

namespace Core3lb
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public void Awake()
        {
             DontDestroyOnLoad(gameObject);
        }
    }
}
