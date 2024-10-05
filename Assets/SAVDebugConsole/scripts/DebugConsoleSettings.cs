//SAV Debug Console - Copyright 2024 ShadowAndVVWorkshop

using UnityEngine;

namespace ShadowAndVVWorkshop.SAVDebugConsole
{
    public class DebugConsoleSettings : MonoBehaviour
    {

        [Tooltip("When selected log stack details will be displayed")]
        [SerializeField]
        public bool _showDetail = true;
        private bool ShowDetail { get { return _showDetail; } set { _showDetail = value; } }

    }
}