using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputString : MonoBehaviour {
	public Text _gt;

	void Update () {
		foreach (char c in Input.inputString) {
			if (c == '\b') // has backspace/delete been pressed?
			{
				if (_gt.text.Length != 0) {
					_gt.text = _gt.text.Substring (0, _gt.text.Length - 1);
				}
			} else if ((c == '\n') || (c == '\r')) // enter/return
			{
				print ("User entered their name: " + _gt.text);
			} else {
				_gt.text += c;
			}
		}
	}
}