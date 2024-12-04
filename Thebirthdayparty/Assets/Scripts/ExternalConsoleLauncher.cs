using System.Diagnostics;
using UnityEngine;

public class ExternalConsoleLauncher : MonoBehaviour
{
    private Process externalProcess;

    public string exePath = "path/to/your/executable.exe"; // 外部程序路径

    void Start()
    {
        LaunchExternalConsole();
    }

    void OnDestroy()
    {
        CloseExternalConsole();
    }

    private void LaunchExternalConsole()
    {
        if (string.IsNullOrEmpty(exePath))
        {
            UnityEngine.Debug.LogError("Executable path is not set!");
            return;
        }
        System.Diagnostics.Process.Start(Application.dataPath+"/"+exePath);
    }

    private void CloseExternalConsole()
    {
        if (externalProcess != null && !externalProcess.HasExited)
        {
            externalProcess.Kill(); // 强制关闭进程
            externalProcess.Dispose();
            UnityEngine.Debug.Log("External console closed.");
        }
    }
}

