//SAV Debug Console - Copyright 2024 ShadowAndVVWorkshop

using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;

namespace ShadowAndVVWorkshop.SAVDebugConsole
{
    public class DebugConsoleManager_LogCatProvider : MonoBehaviour
    {
        public event EventHandler<String> LogCatEntry;

        [SerializeField] protected string LogCatArguments = "";

        bool livelogging = true;
        int consolEentryLimit = 1024;
        protected ConcurrentQueue<string> inbound_logcatEntries = new ConcurrentQueue<string>();

        public void Initialize(int consoleentrylimit = 1024)
        {
            if (consoleentrylimit > 0)
            {
                consolEentryLimit = consoleentrylimit;
            }

            StartCoroutine("HandleLogCat");
        }

        void OnDisable()
        {
            livelogging = false;
        }

        IEnumerator HandleLogCat()
        {
            using (AndroidJavaClass runtime = new AndroidJavaClass("java.lang.Runtime"))
            {
                using (AndroidJavaObject run = runtime.CallStatic<AndroidJavaObject>("getRuntime"))
                {
                    using (AndroidJavaObject proc = run.Call<AndroidJavaObject>("exec", "logcat " + LogCatArguments))
                    {
                        using (AndroidJavaObject proc_stream = proc.Call<AndroidJavaObject>("getInputStream"))
                        {
                            using (AndroidJavaObject isr = new AndroidJavaObject("java.io.InputStreamReader", proc_stream))
                            {
                                using (AndroidJavaObject buff = new AndroidJavaObject("java.io.BufferedReader", isr))
                                {
                                    LogCatEntry(this, "Loading - Clear to reduce load time.");
                                    string readline = "";
                                    while (livelogging)
                                    {
                                        int cnt = 0;
                                        while (livelogging && cnt < consolEentryLimit + 1)
                                        {
                                            cnt++;
                                            if (buff.Call<bool>("ready"))
                                            {
                                                readline = buff.Call<string>("readLine");
                                                if (readline != null && LogCatEntry != null)
                                                {
                                                    if (inbound_logcatEntries.Count > consolEentryLimit)
                                                    {
                                                        inbound_logcatEntries.TryDequeue(out string result); 
                                                        cnt--;
                                                    }
                                                    inbound_logcatEntries.Enqueue(readline);
                                                    //LogCatEntry(this, readline);
                                                }
                                                else
                                                    break;
                                            }
                                            else
                                                break;
                                            //yield return null;
                                        }
                                        //if (inbound_logcatEntries.Count > 0)
                                        //{
                                        //    LogCatEntry(this, inbound_logcatEntries.Count.ToString());
                                        //}
                                        while (inbound_logcatEntries.Count > 0)
                                        {
                                            inbound_logcatEntries.TryDequeue(out string result);
                                            LogCatEntry(this, result);
                                        }
                                        inbound_logcatEntries.Clear();
                                        yield return null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public void ClearLogCat()
        {
            using (AndroidJavaClass runtime = new AndroidJavaClass("java.lang.Runtime"))
            {
                using (AndroidJavaObject run = runtime.CallStatic<AndroidJavaObject>("getRuntime"))
                {
                    using (AndroidJavaObject proc = run.Call<AndroidJavaObject>("exec", "logcat -c"))
                    {
                    }
                }
            }
        }

    }
}