using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes {
    [CreateAssetMenu(menuName = "TDD/Cutscene", order = 0)]
    public class Cutscene : ScriptableObject {
        [field: SerializeField] public Sprite CutsceneImg { get; private set; }

        [field: SerializeField] public List<CutsceneText> Texts { get; private set; } = new();

        [field: SerializeField, ] public Cutscene Next { get; private set; }
        
        [Serializable]
        public struct CutsceneText {
            [field: SerializeField] public string Text { get; private set; }
            [field: SerializeField] public float Delay { get; private set; }
            [field: SerializeField] public float Duration { get; private set; }
            
            [field: SerializeField] public Vector2Int Position { get; private set; } 
            [field: SerializeField] public Vector2Int Size { get; private set; } 
        }
    }
}
