using UnityEngine;
using System.Collections;

public class HighlightController : MonoBehaviour {

	public Sprite SelectedOn;
	public Sprite SelectedOff;
	
	public void SelectedChanged(bool selected) {
		SpriteRenderer sr = (SpriteRenderer)renderer;
		sr.sprite = selected ? SelectedOn : SelectedOff;
	}
}
