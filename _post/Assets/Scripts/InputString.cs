using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Kulikovskoe {
	public class InputString : MonoBehaviour {
		public textOut _textOut;
		public Text _gt;
		public Text _BarcodeSave;

		void Update () {
			foreach (char c in Input.inputString) {
				_gt.text += c;
				if (_gt.text.Length >= 20) {
					_BarcodeSave.text = _gt.text;
					_textOut.Start_Post ();
					_gt.text = "";
				}
			}
		}
	}
}
//10012000000003476908