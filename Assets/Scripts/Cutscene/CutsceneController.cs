using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace Cutscenes {
    public class CutsceneController : MonoSingleton<CutsceneController> {
        
        [SerializeField] private RectTransform cutsceneTextbox;
        
        [SerializeField] private Image cutsceneImage;
        [SerializeField] private PlayableDirector director;

        [SerializeField] private RectTransform topStripe;
        [SerializeField] private RectTransform bottomStripe;
        
        public Cutscene CurrentCutscene { get; private set; }
        private List<GameObject> textboxes = new();
        
        public void DisplayCutscene(Cutscene cutscene) {
            Assert.IsNotNull(cutscene);

            if (CurrentCutscene != null) {
                Debug.LogWarning($"Cutscene ({CurrentCutscene.name}) stopped by cutscene {cutscene.name}");
                StopCutscene();
            }
            
            CurrentCutscene = cutscene;

            if (cutscene.TimeLine) {
                if (director.state == PlayState.Playing)
                    director.Stop();
                
                director.playableAsset = cutscene.TimeLine;
                director.Play();
            }

            foreach (Cutscene.CutsceneText text in cutscene.Texts)
                StartCoroutine(ShowText(text));

            if (cutscene.UseCinematicStripes)
                DOTween.Sequence()
                    .Append(topStripe.DOAnchorPosY(0, cutscene.FadeIn.Duration).SetEase(cutscene.FadeIn.Ease))
                    .Join(bottomStripe.DOAnchorPosY(0, cutscene.FadeIn.Duration).SetEase(cutscene.FadeIn.Ease))
                    .Play();
            
            StartCoroutine(ShowCutsceneImage(cutscene));
        }

        public void StopCutscene() {
            StopAllCoroutines();
            DOTween.Sequence()
                .Append(topStripe.DOAnchorPosY(topStripe.sizeDelta.y, CurrentCutscene.FadeOut.Duration).SetEase(CurrentCutscene.FadeOut.Ease))
                .Join(bottomStripe.DOAnchorPosY(-bottomStripe.sizeDelta.y, CurrentCutscene.FadeOut.Duration).SetEase(CurrentCutscene.FadeOut.Ease))
                .Play();
            
            CurrentCutscene = null;

            foreach (GameObject textbox in textboxes)
                if (textbox)
                    Destroy(textbox);
            textboxes.Clear();
        }

        private IEnumerator ShowCutsceneImage(Cutscene cutscene) {
            cutsceneImage.color = Color.white.WithAlpha(0f);
            Tween tween = null;
            if (cutscene.CutsceneImg) {
                cutsceneImage.gameObject.SetActive(true);
                tween = cutsceneImage
                    .DOFade(1f, cutscene.FadeIn.Duration)
                    .SetEase(cutscene.FadeIn.Ease)
                    .Play();
                cutsceneImage.sprite = cutscene.CutsceneImg;
            }

            if (tween.IsActive())
                yield return new WaitUntil(() => tween.IsPlaying());
            
            yield return new WaitForSeconds(cutscene.Duration);
            
            StopCutscene();
            cutsceneImage
                .DOFade(0f, cutscene.FadeOut.Duration)
                .SetEase(cutscene.FadeOut.Ease)
                .OnComplete(() => {
                    if (cutscene.Next != null)
                        DisplayCutscene(cutscene.Next);
                    else
                        cutsceneImage.gameObject.SetActive(false);
                })
                .Play();
        }
        
        private IEnumerator ShowText(Cutscene.CutsceneText text) {
            yield return new WaitForSeconds(text.Delay);
 
            RectTransform textbox = Instantiate(cutsceneTextbox, transform);
            TextMeshProUGUI cutsceneText = textbox.GetComponentInChildren<TextMeshProUGUI>();
            CanvasGroup graphic = textbox.GetComponent<CanvasGroup>();
            graphic.alpha = 0;
            
            Tween tween = graphic
                .DOFade(1f, text.FadeIn.Duration)
                .SetEase(text.FadeIn.Ease)
                .Play();
            
            cutsceneText.text = text.Text;
            cutsceneText.fontSize = text.FontSize;    
            
            textbox.anchoredPosition = text.Position;
            textbox.sizeDelta = text.Size;
            textboxes.Add(textbox.gameObject);

            yield return new WaitUntil(() => tween.IsActive() && tween.IsPlaying());
            yield return new WaitForSeconds(text.Duration);

            graphic
                .DOFade(0f, text.FadeOut.Duration)
                .SetEase(text.FadeOut.Ease)
                .OnComplete(() => Destroy(textbox.gameObject))
                .Play();
            
            textboxes.Remove(textbox.gameObject);
        }
    }
}
