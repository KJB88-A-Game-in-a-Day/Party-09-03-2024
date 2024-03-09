using System;
using System.Reflection;
using UnityEditor;

/*
 * Original Code by Minchuilla on the Unity Forums
 * Found here: https://forum.unity.com/threads/shortcut-key-for-lock-inspector.95815/#post-3613249
 * Posted on: 28/08/2018. Implemented here 09/03/2024.
 */
namespace EditorImprovements
{
    public class EditorUtilities
    {
        [MenuItem("Tools/Clear Console &c")] // Clear Console - Alt + C Hotkey
        static void ClearConsole()
        {
            var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");                                                            // This simply does "LogEntries.Clear()" the long way:
            var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }

        [MenuItem("Tools/Toggle Inspector Lock (shortcut) &e")] // Locking Editor with Mouse Over - Alt + E
        static void SelectLockableInspector()
        {
            EditorWindow inspectorToBeLocked = EditorWindow.mouseOverWindow;                                                            // "EditorWindow.focusedWindow" can be used instead
            if (inspectorToBeLocked != null && inspectorToBeLocked.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
                PropertyInfo propertyInfo = type.GetProperty("isLocked");
                bool value = (bool)propertyInfo.GetValue(inspectorToBeLocked, null);
                propertyInfo.SetValue(inspectorToBeLocked, !value, null);

                inspectorToBeLocked.Repaint();
            }
        }

        [MenuItem("Tools/Toggle Inspector Mode &d")] // Change Inspector Mode - Alt + D
        static void ToggleInspectorDebug()
        {
            EditorWindow targetInspector = EditorWindow.mouseOverWindow;                                                                    // "EditorWindow.focusedWindow" can be used instead
            if (targetInspector != null && targetInspector.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");   //Get the type of the inspector window to find out the variable/method from
                FieldInfo field = type.GetField("m_InspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);            //get the field we want to read, for the type (not our instance)

                InspectorMode mode = (InspectorMode)field.GetValue(targetInspector);                                                       //read the value for our target inspector
                mode = (mode == InspectorMode.Normal ? InspectorMode.Debug : InspectorMode.Normal);                           //toggle the value
                //Debug.Log("New Inspector Mode: " + mode.ToString());

                MethodInfo method = type.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);             //Find the method to change the mode for the type
                method.Invoke(targetInspector, new object[] { mode });                                                                                  //Call the function on our targetInspector, with the new mode as an object[]

                targetInspector.Repaint();                                                                                                                                //refresh inspector
            }
        }
    }
}
