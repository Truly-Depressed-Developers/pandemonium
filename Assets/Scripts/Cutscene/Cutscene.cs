using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Cutscenes {
    [CreateAssetMenu(menuName = "TDD/Cutscene", order = 0)]
    public class Cutscene : ScriptableObject {
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public Sprite CutsceneImg { get; private set; }
        
        [field: SerializeField] public Fade FadeIn { get; private set; }
        [field: SerializeField] public Fade FadeOut { get; private set; }

        [field: SerializeField] public List<CutsceneText> Texts { get; private set; } = new();

        [field: SerializeField, ] public Cutscene Next { get; private set; }
        
        [Serializable]
        public class CutsceneText {
            [field: SerializeField] public string Text { get; private set; }
            [field: SerializeField] public float Delay { get; private set; }
            [field: SerializeField] public float Duration { get; private set; }

            [field: SerializeField] public int FontSize { get; private set; } = 36;
            
            [field: SerializeField] public Vector2Int Position { get; private set; } 
            [field: SerializeField] public Vector2Int Size { get; private set; } 
            
            [field: SerializeField] public Fade FadeIn { get; private set; }
            [field: SerializeField] public Fade FadeOut { get; private set; }
        }
        
        [Serializable]
        public class Fade {
            [field: SerializeField] public Ease Ease { get; private set; } = Ease.InOutQuad;
            [field: SerializeField] public float Duration { get; private set; } = 0.3f;
        }
    }
}
