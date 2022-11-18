using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoss : MonoBehaviour
{
	[SerializeField] private AudioSource win;
	[SerializeField] private AudioSource loss;

	public void Play(bool passed)
	{
		if (passed)
		{
			PlayWin();
		}
		else
		{
			PlayLoss();
		}
	}

	private void PlayWin()
	{
		win.Play();
	}

	private void PlayLoss()
	{
		loss.Play();
	}
}
