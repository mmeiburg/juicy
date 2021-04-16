namespace TinyTools.Juicy
{
    internal static class JuicyUtils
    {
        public static float Time(bool ignoreTimeScale = false) =>
            ignoreTimeScale ? UnityEngine.Time.unscaledTime : UnityEngine.Time.time;
        
        public static float DeltaTime(bool ignoreTimeScale = false) =>
            ignoreTimeScale ? UnityEngine.Time.unscaledDeltaTime : UnityEngine.Time.deltaTime;
    }
}