using System.Collections;
using System.IO;
using UnityEngine;

namespace Kulikovskoe {
    public class MenuAnimator : MonoBehaviour {
        [SerializeField] private textOut _textOut;
        [SerializeField] private int m_FrameRate = 29;
        [SerializeField] private Material _SkyboxMaterial;
        [SerializeField] private Sprite[] m_Sprite;

        private WaitForSeconds m_FrameRateWait;

        private void Awake () {

            m_FrameRateWait = new WaitForSeconds (1f / m_FrameRate);
        }
        void Start () {
            _SkyboxMaterial.SetTexture ("_Tex", m_Sprite[0].texture);
            Screen.SetResolution (2560, 1440, true);

            if (_textOut._AutoPlay.isOn == true) {
                SA ();
            }
        }

        public void SA () {
            StartCoroutine ("StartAnimation");
        }

        private IEnumerator StartAnimation () {
            yield return new WaitForSeconds (2f);
            StartCoroutine ("PlayTextures");
        }

        private IEnumerator PlayTextures () {
            int i = 0;
            for (i = 0; i < m_Sprite.Length; i++) {
                _textOut._Request = false;
                _SkyboxMaterial.SetTexture ("_Tex", m_Sprite[i].texture);
                if (i == m_Sprite.Length - 1) {
                    StopAllCoroutines ();
                    if (_textOut._AutoPlay.isOn == true) {
                        StartCoroutine ("StartAnimation");
                    } else {
                        _textOut._Request = true;
                    }
                }
                yield return m_FrameRateWait;
            }
        }
    }
}