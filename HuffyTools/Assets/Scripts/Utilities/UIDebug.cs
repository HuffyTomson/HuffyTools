using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Huffy.Utilities
{
    public class UIDebug : SingletonBehaviour<UIDebug>
    {
        public static bool useDebugLog = true;
        public static bool useUIDebug = false;

        public Color color = Color.white;
        private string printString = "";
        private List<string> printList = new List<string>();

        void Awake()
        {
            DontDestroy();
#if !UNITY_EDITOR
            useUIDebug = false;
#endif
        }

        public static void Log(string _message)
        {
            if (useDebugLog)
                Debug.Log(_message);

            Instance.StartCoroutine(Instance.AddPrint(_message));
        }

        IEnumerator AddPrint(string _message)
        {
            printList.Add(_message);
            yield return new WaitForSeconds(8.0f);
            printList.Remove(_message);
        }

        void OnGUI()
        {
            GUI.color = color;

            // order by newest on top
            printString = "";
            for (int i = printList.Count - 1; i >= 0; i--)
            {
                printString += printList[i] + "\n";
            }

            if (useUIDebug)
            {
                GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height - 40), printString);
            }
        }
    }
}