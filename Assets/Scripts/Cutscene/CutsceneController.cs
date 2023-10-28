using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Cutscenes {
    public class CutsceneController : MonoSingleton<CutsceneController> {

        [SerializeField] private Image cutsceneImage;
        [SerializeField] private TextMeshProUGUI cutsceneText;

        private Coroutine currentCutsceneCoroutine;
        
        public void DisplayCutscene(Cutscene cutscene) {
            Assert.IsNotNull(cutscene);
            
            if (currentCutsceneCoroutine != null) {
                Debug.LogWarning($"Cutscene stopped by cutscene {cutscene.name}");
                StopCoroutine(currentCutsceneCoroutine);
            }
            
            currentCutsceneCoroutine = StartCoroutine(PlayCutscene(cutscene));
        }

        public void StopCutscene() {
            if (currentCutsceneCoroutine != null) {
                StopCoroutine(currentCutsceneCoroutine);
                currentCutsceneCoroutine = null;
            }
        }

        private IEnumerator PlayCutscene(Cutscene cutscene) {
            if (cutscene.CutsceneImg) {
                cutsceneImage.gameObject.SetActive(true);
                cutsceneImage.sprite = cutscene.CutsceneImg;
            }

            foreach (Cutscene.CutsceneText text in cutscene.Texts) {
                yield return new WaitForSeconds(text.Delay);
 
                RectTransform parent = (RectTransform)cutsceneText.rectTransform.parent;
                if (string.IsNullOrEmpty(text.Text)) {
                    parent.gameObject.SetActive(false);    
                } else {
                    cutsceneText.text = text.Text;
                    
                    parent.anchoredPosition = text.Position;
                    parent.sizeDelta = text.Size;
                    parent.gameObject.SetActive(true);
                }
                
                yield return new WaitForSeconds(text.Duration);
                parent.gameObject.SetActive(false);
            }
            
            currentCutsceneCoroutine = null;
            if (cutscene.Next != null)
                DisplayCutscene(cutscene.Next);
            else
                cutsceneImage.gameObject.SetActive(false);
        }
    }
}
