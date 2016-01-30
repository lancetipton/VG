using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFader : MonoBehaviour {
	#region Public Properties

	[System.Serializable]
	public class Events {
		public UnityEvent fadingOut;
		public UnityEvent fadedOut;
		public UnityEvent fadingIn;
		public UnityEvent fadedIn;
	}

	[Tooltip("If true, set interactable and blocksRaycast according to current alpha")]
	public bool updateInteractable = true;

	public Events events;

	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties
	enum Mode {
		FadingIn,
		FadingOut,
		Done
	}

	float startTime;
	float duration;
	Mode mode = Mode.Done;

	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Update() {
		if (mode == Mode.Done) return;

		if (mode == Mode.FadingOut) {
			float t = (Time.time - startTime) / duration;
			if (t >= 1) {
				SetAlpha(0);
				mode = Mode.Done;
				events.fadedOut.Invoke();
			} else {
				SetAlpha(1 - t);
			}
		}

		if (mode == Mode.FadingIn) {
			float t = (Time.time - startTime) / duration;
			if (t >= 1) {
				SetAlpha(1);
				mode = Mode.Done;
				events.fadedIn.Invoke();
			} else {
				SetAlpha(t);
			}
		}

	}

	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods
	public void FadeOut(float fadeTime) {
		events.fadingOut.Invoke();
		startTime = Time.time;
		duration = fadeTime;
		mode = Mode.FadingOut;
	}

	public void FadeIn(float fadeTime) {
		events.fadingIn.Invoke();
		startTime = Time.time;
		duration = fadeTime;
		mode = Mode.FadingIn;
	}
	
	public void SetAlpha(float alpha) {
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.alpha = alpha;
		if (updateInteractable) {
			cg.interactable = (alpha > 0.01f);
			cg.blocksRaycasts = cg.interactable;
		}
	}
	#endregion
}
