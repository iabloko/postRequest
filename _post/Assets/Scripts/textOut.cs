using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kulikovskoe {
	public class textOut : MonoBehaviour {
		//public MenuAnimator _MenuAnimator;
		private string JsonDataString;
		public Toggle _AutoPlay;
		public Text _Text, _CurrentURL;
		public GameObject _IPOBJ;
		public string _url;

		private int _i = 1;
		public bool _Request = true;
		[HideInInspector] public bool MenuActive = false;

		//

		void Awake () {
			LoadChanges ();
			_IPOBJ.SetActive (false);
		}

		void Start () {

		}

		void Update () {
			if (Input.GetKeyUp (KeyCode.Q)) {
				StartCoroutine (POST ());
			}

			if (Input.GetKeyUp (KeyCode.Escape) || Input.GetKeyUp (KeyCode.Tab)) {
				_MenuActive (_IPOBJ);
			}

			if ((_i % 301 == 0) && _AutoPlay.isOn == false && _Request == true) {
				_i = 1;
				_Request = false;
				StartCoroutine (POST ());
			} else if (_i == 50000) {
				_i = 1;
			}
			_i++;
		}

		public IEnumerator POST () {
			_i = 1;
			var Data = new WWWForm ();
			Data.AddField ("action", "check");
			Data.AddField ("deviceName", "north_entrance");
			var Query = new WWW (_url, Data);
			yield return Query;

			if (Query.error != null) {
				Debug.Log ("Server does not respond : " + Query.error);
				_Text.text = Query.error;
				_Request = true;
			} else {
				if (Query.text == "Testss") { //ответ сервера
					Debug.Log ("Server responded correctly");
					_Request = true;
				} else {
					Debug.Log ("Server responded : " + Query.text);
					_Text.text = Query.text;
					JSONNode jsonNode = Kulikovskoe.JSON.Parse (Query.text);
					_Text.text = jsonNode["success"].ToString ().ToUpper ();
					if (_Text.text == "true") {
						//_MenuAnimator.SA ();
					} else {
						_Request = true;
					}
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
			//_url =_IFfield.text;

			PlayerPrefs.SetString ("_CurrentURL", _url);
			PlayerPrefs.SetInt ("_AutoPlay.isOn", _AutoPlay.isOn?1 : 0);
			PlayerPrefs.SetString ("_url", "http://"+_IFfield.text);
			SceneManager.LoadScene ("Kulikovskoe360-1");
		}
		private void LoadChanges () {
			_Request = true;

			_CurrentURL.text = "CurrentURL:" + PlayerPrefs.GetString ("_CurrentURL", _CurrentURL.text);
			_AutoPlay.isOn = PlayerPrefs.GetInt ("_AutoPlay.isOn") == 1 ? true : false;
			_url = PlayerPrefs.GetString ("_url", "http://"+_url);
		}

	}
}