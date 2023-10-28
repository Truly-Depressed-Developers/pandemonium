using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Cutscenes {
    public class CutsceneController : MonoSingleton<CutsceneController> {
        
        [SerializeField] private RectTransform cutsceneTextbox;
        
        [SerializeField] private Image cutsceneImage;

        private Cutscene currentCutscene;
        private List<GameObject> textboxes = new();
        
        public void DisplayCutscene(Cutscene cutscene) {
            Assert.IsNotNull(cutscene);

            if (currentCutscene != null) {
                Debug.LogWarning($"Cutscene ({currentCutscene.name}) stopped by cutscene {cutscene.name}");
                StopCutscene();
            }
            
            currentCutscene = cutscene;

            foreach (Cutscene.CutsceneText text in cutscene.Texts)
                StartCoroutine(ShowText(text));
            
            if (cutscene.CutsceneImg) {
                cutsceneImage.gameObject.SetActive(true);
                cutsceneImage.sprite = cutscene.CutsceneImg;
                StartCoroutine(ShowCutsceneImage(cutscene));
            }
        }

        public void StopCutscene() {
            StopAllCoroutines();
            currentCutscene = null;

            foreach (GameObject textbox in textboxes)
                if (textbox)
                    Destroy(textbox);
            textboxes.Clear();
        }

        private IEnumerator ShowCutsceneImage(Cutscene cutscene) {
            yield return new WaitForSeconds(cutscene.Duration);
            
            if (cutscene.Next != null)
                DisplayCutscene(cutscene.Next);
            else
                cutsceneImage.gameObject.SetActive(false);
            
            StopCutscene();
        }
        
        private IEnumerator ShowText(Cutscene.CutsceneText text) {
            yield return new WaitForSeconds(text.Delay);
 
            RectTransform textbox = Instantiate(cutsceneTextbox, transform);
            TextMeshProUGUI cutsceneText = textbox.GetComponentInChildren<TextMeshProUGUI>();
                
            cutsceneText.text = text.Text;
            cutsceneText.fontSize = text.FontSize;    
            
            textbox.anchoredPosition = text.Position;
            textbox.sizeDelta = text.Size;
            textboxes.Add(textbox.gameObject);
            
            yield return new WaitForSeconds(text.Duration);
            
            Destroy(textbox.gameObject);
            textboxes.Remove(textbox.gameObject);
        }
    }
}
