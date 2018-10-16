using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kulikovskoe {
	public class textOut : MonoBehaviour {
		//public MenuAnimator _MenuAnimator;
		private string JsonDataString;
		public string _barcode;
		public Toggle _AutoPlay;
		public Text _StatusText, _CurrentURL, StatusText_2;
		public GameObject _IPOBJ;
		private string _url;
		private Uri _uri;

		private Text _SucessText, checkResultText;
		//private bool _setActive = false;
		[HideInInspector] public bool _Request = true;
		private float _elapsed;

		void Awake () {
			LoadChanges ();
			_IPOBJ.SetActive (false);
			_CurrentURL.text = " CurrentURL: " + _url;
		}

		void Update () {
			if (Input.GetKeyUp (KeyCode.Escape) || Input.GetKeyUp (KeyCode.Tab)) {
				_MenuActive (_IPOBJ);
			}

			if (_elapsed >= 1.2f && _AutoPlay.isOn == false && _Request == true) {
				if (!Uri.TryCreate (_url, UriKind.Absolute, out _uri)) {
					_elapsed = 0;
					_StatusText.text = "Invalid URL. Example: 192.200.100.252:8088/api/external, or vk.com";
				} else {
					_Request = false;
					_StatusText.text = "Connect";
					_elapsed = 0;
					StartCoroutine (POST ());
				}
			} else if (_elapsed >= 1.2f)
				_elapsed = 0;
			_elapsed += Time.deltaTime;
		}

		public IEnumerator POST () {
			var Data = new WWWForm ();
			Data.AddField ("action", "check");
			Data.AddField ("deviceName", "north_entrance");
			Data.AddField ("barcode", _barcode);

			var Query = new WWW (_url, Data);
			yield return Query;

			if (Query.error != null) {
				_StatusText.text = "Server does not respond : " + Query.error;
				_Request = true;
			} else {
				_StatusText.text = "Server responded : " + Query.text;
				JSONNode jsonNode = Kulikovskoe.JSON.Parse (Query.text);
				_StatusText.text = jsonNode["success"].ToString ().ToUpper ();

				_SucessText.text = jsonNode["success"].ToString ().ToUpper ();
				checkResultText.text = jsonNode["checkResult"].ToString ().ToUpper ();

				StatusText_2.text = _SucessText.text + "      " + checkResultText.text;

				if (_SucessText.text == "true") {
					//_MenuAnimator._StartAnimation ();
				} else {
					_Request = true;
				}
			}
			Query.Dispose ();
		}

		private void _MenuActive (GameObject _panel) {
			if (_panel.activeInHierarchy == false)
				_IPOBJ.SetActive (true);
			else
				_IPOBJ.SetActive (false);
		}

		public void SaveChange () {
			InputField _IFfield = _IPOBJ.GetComponentInChildren<InputField> () as InputField;
			PlayerPrefs.SetInt ("_AutoPlay.isOn", _AutoPlay.isOn?1 : 0);
			PlayerPrefs.SetString ("_url", "http://" + _IFfield.text);

			SceneManager.LoadScene ("WebRequest");
		}
		private void LoadChanges () {
			_Request = true;
			_AutoPlay.isOn = PlayerPrefs.GetInt ("_AutoPlay.isOn") == 1 ? true : false;
			_url = PlayerPrefs.GetString ("_url", "http://" + _url);
		}
	}
}