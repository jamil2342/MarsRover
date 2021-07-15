using System;
using UnityEngine;

/// <summary>
/// Script for exception handling. 
/// </summary>
public class ExceptionHandler : MonoBehaviour
{
    public void OnEnable()
    {
        Application.logMessageReceived += this.HandleExceptionInLog;
    }

    public void OnDisable()
    {
        Application.logMessageReceived -= this.HandleExceptionInLog;
    }

    public void HandleExceptionInLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            SceneManager.ExitGame();
        }
    }
}
