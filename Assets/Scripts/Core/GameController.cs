using Cutscenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core {
    public class GameController : MonoSingleton<GameController> {
        
        [SerializeField] private Cutscene startCutscene;
        
        private void Start() {
            if (!MainMenu.MainMenu.HasBeenLoaded) {
                SceneManager.LoadScene(0);
                return;
            }
            
            Debug.Log("GameStart");
                
            CutsceneController.I.DisplayCutscene(startCutscene);
        }
    }
}
