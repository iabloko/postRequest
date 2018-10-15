using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kulikovskoe {
	public class textOut : MonoBehaviour {
		private string JsonDataString;
		public Toggle _AutoPlay;
		public Text _Text, _CurrentURL;
		public GameObject _IPOBJ;
		public string _url;
		private int _i = 1;
		[HideInInspector] public bool _Request = true;

		private float _elapsed;

		void Awake () {
			LoadChanges ();
			_IPOBJ.SetActive (false);

			_CurrentURL.text = " CurrentURL: " + " " + _url;
		}

		void Update () {
			if (Input.GetKeyUp (KeyCode.Escape) || Input.GetKeyUp (KeyCode.Tab)) {
				_MenuActive (_IPOBJ);
			}

			if (_elapsed >= 1.2f && _AutoPlay.isOn == false && _Request == true) {
				_Request = false;
				_Text.text = "Connect";
				_elapsed = 0;
				StartCoroutine (POST ());
			} else if (_elapsed >= 5)
				_elapsed = 0;
			_elapsed += Time.deltaTime;
		}

		public IEnumerator POST () {
			var Data = new WWWForm ();
			Data.AddField ("action", "check");
			Data.AddField ("deviceName", "north_entrance");
			var Query = new WWW (_url, Data);
			yield return Query;
			_Request = true;

			if (Query.error != null) {
				_Text.text = "Server does not respond : " + Query.error;
			} else {
				_Text.text = "Server responded : " + Query.text;
				JSONNode jsonNode = Kulikovskoe.JSON.Parse (Query.text);
				_Text.text = jsonNode["success"].ToString ().ToUpper ();
				if (_Text.text == "true") {
					//запуск видео
				}
			}
			Query.Dispose ();
		}

		private void _MenuActive (GameObject _InputField) {
			if (_InputField.activeInHierarchy == false)
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