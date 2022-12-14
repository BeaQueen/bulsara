// Copyright (c) 2022 Jonathan Lang

using UnityEditor;
using UnityEngine;

namespace Baracuda.Monitoring.Editor
{
    internal class MenuItemLayout : MonoBehaviour
    {
        [MenuItem("Tools/Runtime Monitoring/Settings", priority = 2405)]
        private static void OpenMonitorSettingsWindow()
        {
            MonitoringSettingsWindow.Open();
        }

        [MenuItem("Tools/Runtime Monitoring/Filter Window", priority = 2406)]
        private static void OpenFiltersWindow()
        {
            MonitoringFilterWindow.Open();
        }
    }
}
