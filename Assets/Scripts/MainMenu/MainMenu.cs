using UnityEngine;

namespace MainMenu {
    public class MainMenu : MonoBehaviour {
        #region Callback

        public void Callback_NewGame() {
            Debug.Log("TODO: New Game");
        }

        public void Callback_Exit() {
            Utils.ApplicationQuit("Main Menu Exit");
        }
        
        #endregion
    }
}
