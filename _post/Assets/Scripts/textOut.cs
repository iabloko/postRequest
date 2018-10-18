using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kulikovskoe {
	public class textOut : MonoBehaviour {
		//public MenuAnimator _MenuAnimator;
		public InputString _inputString;
		private string JsonDataString;
		public Toggle _AutoPlay;
		public Text _StatusText, _StatusText2, _SuccesText, _passAllowedText, _entryAllowedText, _CurrentURL;
		public GameObject _IPOBJ;
		private string _url;
		private Uri _uri;
		[HideInInspector] public bool _Request = true;

		void Awake () {

			LoadChanges ();
			_IPOBJ.SetActive (false);
			_CurrentURL.text = " CurrentURL: " + _url;
		}

		void Update () {
			if (Input.GetKeyUp (KeyCode.Escape) || Input.GetKeyUp (KeyCode.Tab)) {
				_MenuActive (_IPOBJ);
			}
		}

		public void Start_Post () {
			if (_AutoPlay.isOn == false && _Request == true) {
				if (!Uri.TryCreate (_url, UriKind.Absolute, out _uri)) {
					_StatusText.text = "Invalid URL. Example: 192.200.100.252:8088/api/external, or vk.com";
				} else {
					_Request = false;
					_StatusText.text = "Connect";
					StartCoroutine (POST ());
				}
			}
		}

		public IEnumerator POST () {
			var Data = new WWWForm ();
			Data.AddField ("action", "check");
			Data.AddField ("deviceName", "north_entrance");
			Data.AddField ("barcode", _inputString._BarcodeSave.text);

			var Query = new WWW (_url, Data);
			yield return Query;

			if (Query.error != null) {
				_StatusText.text = "Server does not respond : " + Query.error;
				_Request = true;
			} else {
				_StatusText.text = "Server responded : " + Query.text;
				JSONNode jsonNode = Kulikovskoe.JSON.Parse (Query.text);
				_StatusText.text = "success= " + jsonNode["success"].ToString ().ToUpper () + " checkResult= " + jsonNode["checkResult"].ToString ().ToUpper ();

				_SuccesText.text = jsonNode["success"].ToString ().ToUpper ();
				_passAllowedText.text = jsonNode["checkResult"]["passAllowed"].ToString ().ToUpper ();
				_entryAllowedText.text = jsonNode["checkResult"]["entryAllowed"].ToString ().ToUpper ();

				_StatusText2.text = _SuccesText.text + _passAllowedText.text + _entryAllowedText.text;
				//entryAllowed //exitAllowed //passAllowed
				if (_StatusText2.text=="TRUETRUETRUE") {
					_StatusText2.text = "StartAnimation";
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