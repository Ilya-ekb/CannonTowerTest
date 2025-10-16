using UnityEngine;

/// <summary>
/// Simple reusable cooldown timer.
/// </summary>
[System.Serializable]
public class Cooldown
{
    private readonly float interval;
    private float timer;

    public Cooldown(float interval)
    {
        this.interval = interval;
        timer = interval;
    }

    public bool Ready => timer >= interval;

    public void Reset() => timer = 0f;

    public void Update() => timer += Time.deltaTime;
}