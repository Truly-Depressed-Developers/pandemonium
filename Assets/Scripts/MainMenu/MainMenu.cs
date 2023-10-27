using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu {
    public class MainMenu : MonoBehaviour {
        
        public static bool HasBeenLoaded { get; private set; }

        private void Start() {
            HasBeenLoaded = true;
        }

        #region Callback

        public void Callback_NewGame() {
            SceneManager.LoadScene(sceneBuildIndex: 2);
        }

        public void Callback_Exit() {
            Utils.ApplicationQuit("Main Menu Exit");
        }
        
        #endregion
    }
}
