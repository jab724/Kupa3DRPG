public static class Debug
{
    //Project Settings 내 Scripting Define Symbols를 이용해 로그 포함 여부를 조정 가능.
    //Conditional을 사용하여 로그를 사용하지 않을 경우 자원 소모 최소화
    //Debug 기능 중 Log와 LogError를 주로 사용하여 두가지만 적용. 추후 다른 기능 사용 시 추가 필요

    [System.Diagnostics.Conditional("ON_DEBUG_LOG")]
    public static void Log(object msg) => UnityEngine.Debug.Log(msg);
    [System.Diagnostics.Conditional("ON_DEBUG_LOG")]
    public static void Log(object msg, UnityEngine.Object obj) => UnityEngine.Debug.Log(msg, obj);
    [System.Diagnostics.Conditional("ON_DEBUG_LOG")]
    public static void LogError(object msg) => UnityEngine.Debug.LogError(msg);
    [System.Diagnostics.Conditional("ON_DEBUG_LOG")]
    public static void LogError(object msg, UnityEngine.Object obj) => UnityEngine.Debug.LogError(msg, obj);
}