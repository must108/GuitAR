using UnityEngine;


namespace Core3lb
{
    public class XRPlayerController : Singleton<XRPlayerController>
    {
        [CoreRequired]
        public Camera headCamera;
        [Tooltip("Actual Tracked Anchor")]
        public Transform trackedLeftHand;
        [Tooltip("Actual Tracked Anchor")]
        public Transform trackedRightHand;
        [CoreRequired]
        public XRHand leftXRHand;
        [CoreRequired]
        public XRHand rightXRHand;
        [CoreHeader("Optional")]
        [CoreEmphasize]
        public XRMovement movementSystem;
        [CoreEmphasize]
        public XRHandData handTrackingData;

        [CoreHeader("Default Player Stuff")]
        public string playerName;
        public GameObject playerUI;
        //If you need add things specific to your application create a new singleton called the {YourGame}PlayerManager 

        //Rig Mover
    }
}
