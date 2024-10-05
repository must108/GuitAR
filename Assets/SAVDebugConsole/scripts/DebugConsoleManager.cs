//SAV Debug Console - Copyright 2024 ShadowAndVVWorkshop

using ShadowAndVVWorkshop.SAVFormattedInputField;
using System;
using System.Collections.Concurrent;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace ShadowAndVVWorkshop.SAVDebugConsole
{
    public class DebugConsoleManager : MonoBehaviour
    {
        [SerializeField]
        public int _maxConsoleEntries = 100;
        private int MaxConsoleEntries { get { return _maxConsoleEntries; } set { _maxConsoleEntries = value; } }
        [SerializeField] protected int MaxLogCharacters = 1024;
        [SerializeField] protected int MaxStackCharacters = 1024;
        [SerializeField] protected GameObject ContentHolder = null;
        [SerializeField] protected GameObject ContentMessagePrefab = null;
        [SerializeField] protected GameObject ContentStackPrefab = null;
        [SerializeField] protected GameObject ContentListEmptyPrefab = null;
        [SerializeField] protected Text FilterButton = null;
        [SerializeField] protected InputFieldWithStringFormat FilterText = null;
        [SerializeField] protected GameObject OfText = null;
        [SerializeField] protected DebugConsoleManager_LogCatProvider LogCatProvider = null;


        protected class inbound_conesoleentry
        {
            public DateTime eventdatetime = DateTime.Now;
            public StringBuilder logString = new StringBuilder();
            public StringBuilder stackTrace = new StringBuilder();
            public LogType type = LogType.Warning;
        }
        protected ConcurrentQueue<inbound_conesoleentry> inbound_consoleEntries = new ConcurrentQueue<inbound_conesoleentry>();

        protected class display_conesoleentry
        {
            public DateTime eventdatetime = DateTime.Now;
            public GameObject logMessage = null;
            public GameObject stackMessage = null;
            public LogType type = LogType.Warning;
        }
        protected ConcurrentQueue<display_conesoleentry> display_consoleEntries = new ConcurrentQueue<display_conesoleentry>();

        protected RectTransform ConsoleCanvas_RectTransform = null;
        protected GameObject DebugConsole = null;
        protected ScrollRect ConsoleScrollWindow = null;
        protected bool _showDetail = true;
        //protected bool _newEntry = false;
        protected bool _size_flipflop = true;
        protected bool _details_flipflop = true;
        protected bool _pinned = false;
        protected bool _filter_flipflop = false;
        protected DebugConsoleSettingsForKeyboard debugConsoleKeyboard;

        void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            try
            {
                //AddAwakeConsoleEntry("BASE START", LogType.Log);

                Debug.Assert(_maxConsoleEntries > 0, "_maxConsoleEntries < 0");
                Debug.Assert(MaxLogCharacters > 0, "MaxLogCharacters < 0");
                Debug.Assert(MaxStackCharacters > 0, "MaxStackCharacters < 0");
                Debug.Assert(ContentHolder != null, "ContentHolder - Null");
                Debug.Assert(ContentMessagePrefab != null, "ContentMessagePrefab - Null");
                Debug.Assert(ContentStackPrefab != null, "ContentStackPrefab - Null");
                Debug.Assert(ContentListEmptyPrefab != null, "ContentListEmptyPrefab - Null");

                ConsoleScrollWindow = ContentHolder.transform.parent.GetComponentInParent<ScrollRect>();
                ConsoleCanvas_RectTransform = ConsoleScrollWindow.transform.parent.GetComponent<RectTransform>();
                DebugConsole = ConsoleCanvas_RectTransform.transform.parent.transform.parent.gameObject;

                Debug.Assert(ConsoleScrollWindow != null, "ConsoleScrollWindow - Null");
                Debug.Assert(DebugConsole != null, "DebugConsole - Null");

                debugConsoleKeyboard = DebugConsole.GetComponent<DebugConsoleSettingsForKeyboard>();

                DebugConsoleSettings debugConsoleSettings = DebugConsole.GetComponent<DebugConsoleSettings>();
                if (debugConsoleSettings != null)
                    _showDetail = debugConsoleSettings._showDetail;

                if (!_showDetail)
                {
                    _showDetail = true;
                    DetailsOnClick();
                }

                if (FilterText != null)
                {
                    FilterText.gameObject.SetActive(false);
                    FilterText.OnSavValueChanged.AddListener(FilterValueChange);
                    FilterText.text = string.Empty;
                    if (OfText != null)
                        OfText.gameObject.SetActive(false);
                }
            }
            catch (Exception e)
            {
                HandleLog(e.Message, e.StackTrace, LogType.Exception);
                // should log error to console
                // will show if debug console is initialized
                throw (e);
            }
        }

        void OnEnable()
        {
            HandleEnable();
        }
        protected virtual void HandleEnable()
        {
            if (LogCatProvider != null)
            {
#if !UNITY_EDITOR
                LogCatProvider.LogCatEntry += LogCatProvider_LogCatEntry;
                LogCatProvider.Initialize(_maxConsoleEntries);
#else
                Debug.Log("Unity Editor detected. Switching to Unity logging.");
                Application.logMessageReceivedThreaded += HandleLog;
#endif
            }
            else
                Application.logMessageReceivedThreaded += HandleLog;
        }

        void OnDisable()
        {
            HandleDisable();
        }
        protected virtual void HandleDisable()
        {
            if (LogCatProvider != null)
                LogCatProvider.LogCatEntry -= LogCatProvider_LogCatEntry;
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        private void LogCatProvider_LogCatEntry(object sender, string e)
        {
            HandleLogCat(e);
        }

        protected void HandleLog(string logString, string stackTrace, LogType type)
        {
            inbound_conesoleentry newconesoleentry = new inbound_conesoleentry();
            newconesoleentry.eventdatetime = DateTime.Now;

            Debug.Assert(MaxLogCharacters > 0, "MaxLogCharacters < 0");
            Debug.Assert(MaxStackCharacters > 0, "MaxStackCharacters < 0");

            // truncate to user defineable length
            logString = logString.Length <= MaxLogCharacters ? logString : logString.Substring(0, MaxLogCharacters) + "...";
            stackTrace = stackTrace.Length <= MaxStackCharacters ? stackTrace : stackTrace.Substring(0, MaxStackCharacters) + "...";

            // best done here
            if (stackTrace.Length > 0 && stackTrace.Substring(stackTrace.Length - 1, 1) == "\n")
                stackTrace = stackTrace.Substring(0, stackTrace.Length - 1);

            newconesoleentry.type = type;
            newconesoleentry.logString.Append(logString);
            newconesoleentry.stackTrace.Append(stackTrace);

            inbound_consoleEntries.Enqueue(newconesoleentry);
        }

        protected void HandleLogCat(string logString)
        {
            inbound_conesoleentry newconesoleentry = new inbound_conesoleentry();
            newconesoleentry.type = LogType.Log;
            newconesoleentry.logString.Append(logString);
            newconesoleentry.stackTrace.Append("");
            inbound_consoleEntries.Enqueue(newconesoleentry);
        }

        protected display_conesoleentry HandleLogEntry(inbound_conesoleentry _inbound_conesoleentry)
        {
            GameObject[] toremove = GameObject.FindGameObjectsWithTag("ListEmptyEntry");
            foreach (GameObject go in toremove)
            {
                GameObject.DestroyImmediate(go);
            }

            Debug.Assert(ContentMessagePrefab != null, "ContentMessagePrefab - Null");
            Debug.Assert(ContentStackPrefab != null, "ContentStackPrefab - Null");

            display_conesoleentry display_Conesoleentry = new display_conesoleentry();
            display_Conesoleentry.eventdatetime = _inbound_conesoleentry.eventdatetime;
            display_Conesoleentry.type = _inbound_conesoleentry.type;

            display_Conesoleentry.logMessage = Instantiate(ContentMessagePrefab);
            display_Conesoleentry.logMessage.transform.SetParent(ContentHolder.transform, false);

            Text logtext = display_Conesoleentry.logMessage.GetComponent<Text>();
            if (logtext != null)
            {
                logtext.text = display_Conesoleentry.eventdatetime.ToString("HH:mm:ss.fffffff");
                logtext.text += " ";
                switch (display_Conesoleentry.type)
                {
                    case LogType.Assert:
                    case LogType.Error:
                    case LogType.Exception:
                        logtext.text += ("<color=red>");
                        logtext.text += _inbound_conesoleentry.logString;
                        logtext.text += "</color>";
                        break;
                    case LogType.Warning:
                        logtext.text += ("<color=yellow>");
                        logtext.text += _inbound_conesoleentry.logString;
                        logtext.text += "</color>";
                        break;
                    default:
                        logtext.text += _inbound_conesoleentry.logString;
                        break;
                }
            }

            display_Conesoleentry.stackMessage = Instantiate(ContentStackPrefab);
            display_Conesoleentry.stackMessage.transform.SetParent(ContentHolder.transform, false);

            Text stacktext = display_Conesoleentry.stackMessage.GetComponent<Text>();
            if (stacktext != null)
                stacktext.text = _inbound_conesoleentry.stackTrace.ToString();

            if (!_details_flipflop)
                display_Conesoleentry.stackMessage.GetComponent<Text>().enabled = false;

            return display_Conesoleentry;
        }

        // called by button
        public void PinOnClick()
        {
            HandlePinClick();
        }

        protected virtual void HandlePinClick()
        {
            _pinned = !_pinned;
        }

        // called by button
        public void ClearOnClick()
        {
            display_conesoleentry remove_display_conesoleentry;
            while (display_consoleEntries.TryDequeue(out remove_display_conesoleentry))
            {
                GameObject.DestroyImmediate(remove_display_conesoleentry.logMessage);
                GameObject.DestroyImmediate(remove_display_conesoleentry.stackMessage);
            }

            Debug.Assert(ContentListEmptyPrefab != null, "ContentListEmptyPrefab - Null");

            foreach (Transform transform in ContentHolder.transform)
            {
                UnityEngine.Object.Destroy(transform.gameObject);
            }

            if (LogCatProvider != null)
            {
                LogCatProvider.ClearLogCat();
            }
            else
            {
                GameObject newconesoleobject = Instantiate(ContentListEmptyPrefab);
                newconesoleobject.transform.SetParent(ContentHolder.transform, false);
            }
        }

        // called by button
        public void SizeOnClick()
        {
            HandleSizeClick();
        }

        protected virtual void HandleSizeClick()
        {
            _size_flipflop = !_size_flipflop;

            Vector2 canvas_currentsize = ConsoleCanvas_RectTransform.sizeDelta;
            if (_size_flipflop)
            {
                ConsoleCanvas_RectTransform.sizeDelta = new Vector2(canvas_currentsize.x, canvas_currentsize.y * .5f);
            }
            else
            {
                ConsoleCanvas_RectTransform.sizeDelta = new Vector2(canvas_currentsize.x, canvas_currentsize.y * 2f);
            }
        }

        // called by button
        public void DetailsOnClick()
        {
            Debug.Assert(ContentHolder != null, "ContentHolder - Null");

            _details_flipflop = !_details_flipflop;
            if (_details_flipflop)
            {
                foreach (display_conesoleentry display_Conesoleentry in display_consoleEntries)
                {
                    // expand value
                    display_Conesoleentry.stackMessage.GetComponent<Text>().enabled = true;
                }
            }
            else
            {
                foreach (display_conesoleentry display_Conesoleentry in display_consoleEntries)
                {
                    // shirnk value
                    display_Conesoleentry.stackMessage.GetComponent<Text>().enabled = false;
                }
            }
            RectTransform rectransform = ContentHolder.GetComponent<RectTransform>();
            if (rectransform != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectransform);
        }

        // called by button
        public void ScrollBottomOnClick()
        {
            Debug.Assert(ConsoleScrollWindow != null, "ScrollBottomOnClick - Null Scroll Rect");

            ConsoleScrollWindow.normalizedPosition = new Vector2(0, 0);
        }

        public void FilterOnClick()
        {
            HandleFilterClick();
        }

        protected virtual void HandleFilterClick()
        {
            Debug.Assert(ContentHolder != null, "ContentHolder - Null");

            _filter_flipflop = !_filter_flipflop;
            // change /O to X etc.
            // show or hide input field and "of" text
            if (!_filter_flipflop)
            {
                FilterText.text = "";
                HandleFilterValueChange(true);  // force change becuase we flipped _filter_flipflop already
                FilterButton.text = "  O\r\n  /";
                FilterButton.color = new Color32(50, 50, 50, 255);
                FilterText.gameObject.SetActive(false);
                OfText.gameObject.SetActive(false);
            }
            else
            {
                FilterButton.text = "  X";
                FilterButton.color = Color.red;
                FilterText.gameObject.SetActive(true);
                OfText.gameObject.SetActive(true);
                OfText.GetComponentInChildren<Text>().text = display_consoleEntries.Count.ToString() + " of " + display_consoleEntries.Count.ToString();
            }
        }

        protected void FilterValueChange(string value, double valueasdouble)
        {
            HandleFilterValueChange();
        }

        protected virtual void HandleFilterValueChange(bool force = false)
        {
            // debug logging in this routine is bad
            //Debug.Log("HandleFilterValueChange");

            // use threads

            if (!force && !_filter_flipflop)
                return;

            string value = FilterText.text;

            int entrycount = 0;
            string totalentrycount = " of " + display_consoleEntries.Count.ToString();
            bool dontaddtoentrycount = false;
            foreach (display_conesoleentry display_Conesoleentry in display_consoleEntries)
            {
                //if (display_Conesoleentry.logMessage.GetComponent<Text>().text.Contains(value))
                if (display_Conesoleentry.logMessage.GetComponent<Text>().text.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    display_Conesoleentry.logMessage.GetComponent<Text>().enabled = true;
                    entrycount++;
                    dontaddtoentrycount = true;
                }
                else
                    display_Conesoleentry.logMessage.GetComponent<Text>().enabled = false;

                // stackmsg
                //if (display_Conesoleentry.stackMessage.GetComponent<Text>().text.Contains(value))
                if (display_Conesoleentry.stackMessage.GetComponent<Text>().text.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // expand LOGMESSAGE value
                    display_Conesoleentry.logMessage.GetComponent<Text>().enabled = true;
                    if (!dontaddtoentrycount)
                        entrycount++;
                    if (_details_flipflop)   // details shown
                        display_Conesoleentry.stackMessage.GetComponent<Text>().enabled = true;
                }
                else
                {
                    if (_details_flipflop)   // details shown
                        display_Conesoleentry.stackMessage.GetComponent<Text>().enabled = false;
                }
            }

            OfText.GetComponentInChildren<Text>().text = entrycount + totalentrycount;

            RectTransform rectransform = ContentHolder.GetComponent<RectTransform>();
            if (rectransform != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectransform);
        }

        void Update()
        {
            //if (_newEntry || inbound_consoleEntries.Count > 0)
            if (inbound_consoleEntries.Count > 0)
            {
                int buffermax = 0;
                while (buffermax < 10)
                {
                    buffermax++;
                    inbound_conesoleentry inbound_Conesoleentry = null;
                    if (inbound_consoleEntries.TryDequeue(out inbound_Conesoleentry))
                    {
                        display_conesoleentry display_Conesoleentry = HandleLogEntry(inbound_Conesoleentry);
                        if (display_Conesoleentry != null)
                        {
                            while (display_consoleEntries.Count > MaxConsoleEntries)
                            {
                                display_conesoleentry remove_display_conesoleentry;
                                if (display_consoleEntries.TryDequeue(out remove_display_conesoleentry))
                                {
                                    GameObject.DestroyImmediate(remove_display_conesoleentry.logMessage);
                                    GameObject.DestroyImmediate(remove_display_conesoleentry.stackMessage);
                                }
                            }
                            display_consoleEntries.Enqueue(display_Conesoleentry);
                        }
                    }
                    else
                        break;
                }

                HandleFilterValueChange();

                // fixes redraw bug with the content holder
                RectTransform transform = ContentHolder.GetComponent<RectTransform>();
                if (transform != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
            }

            HandleUpdate();
            if (debugConsoleKeyboard != null)
            {
                //if (Input.GetKeyDown(debugConsoleKeyboard._trackingOnOff))
                //{
                //    HandlePinClick();
                //}
                if (Input.GetKeyDown(debugConsoleKeyboard._sizeBigSmall))
                {
                    HandleSizeClick();
                }
                if (Input.GetKeyDown(debugConsoleKeyboard._listDetailOnOff))
                {
                    DetailsOnClick();
                }
                if (Input.GetKey(debugConsoleKeyboard._listScrollUp))
                {
                    if (ConsoleScrollWindow != null)
                    {
                        float contentHeight = ConsoleScrollWindow.content.sizeDelta.y;
                        float contentShift = 500 * 1 * Time.deltaTime;
                        ConsoleScrollWindow.verticalNormalizedPosition += contentShift / contentHeight;
                    }
                }
                if (Input.GetKey(debugConsoleKeyboard._listScrollDown))
                {
                    if (ConsoleScrollWindow != null)
                    {
                        float contentHeight = ConsoleScrollWindow.content.sizeDelta.y;
                        float contentShift = 500 * -1 * Time.deltaTime;
                        ConsoleScrollWindow.verticalNormalizedPosition += contentShift / contentHeight;
                    }
                }
                if (Input.GetKeyDown(debugConsoleKeyboard._listClear))
                {
                    ClearOnClick();
                }
            }
        }

        protected virtual void HandleUpdate()
        {
            return;
        }

    }
}
