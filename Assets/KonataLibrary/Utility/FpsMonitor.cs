using UnityEngine;

public class FpsMonitor
{
    public float MaxFps { get; private set; }
    public float MinFps { get; private set; }
    
    private float accum;    // フレーム時間の累積
    private float timeleft; // 更新までの残り時間
    private int frameCount;
    
    private static readonly float updateInterval = 0.5f;

    public FpsMonitor()
    {
        MaxFps = 0;
        MinFps = 9999;
        timeleft = updateInterval;
    }

    public void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frameCount++;

        if (timeleft <= 0f)
        {
            var fps = accum / frameCount;
            
            if (MaxFps < fps)
            {
                MaxFps = fps;
            }

            if (MinFps > fps)
            {
                MinFps = fps;
            }

            Reset();
        }
    }

    private void Reset()
    {
        timeleft = updateInterval;
        accum = 0;
        frameCount = 0;
    }
}
