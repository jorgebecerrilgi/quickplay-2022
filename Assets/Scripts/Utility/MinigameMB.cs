using System;
using UnityEngine;

public abstract class MinigameMB : MonoBehaviour
{
    public event Action<bool, float> OnEnd;

    protected void InvokeOnEnd(bool hasWon, float remainingSeconds)
    {
        OnEnd?.Invoke(hasWon, remainingSeconds);
    }

    public abstract void BeginMinigame(float remainingSeconds);
}
