using UnityEngine;
using UnityEngine.SceneManagement;

namespace PauseMenu {
    public class PauseMenu : MonoSingleton<PauseMenu> {
        
        public bool IsPaused { get; private set; }

        protected override void Awake() {
            base.Awake();
            gameObject.SetActive(false);
        }

        public void Show() {
            if (IsPaused)
                return;

            gameObject.SetActive(true);            
            IsPaused = true;
            Time.timeScale = 0;
        }

        public void Hide() {
            if (!IsPaused)
                return;

            Time.timeScale = 1f;
            gameObject.SetActive(false);
            IsPaused = false;
        }


        #region Callbacs

        public void Callback_Resume() {
            Hide();
        }

        public void Callback_Exit() {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public void Callback_Switch() {
            if (IsPaused) Hide();
            else Show();
            Debug.Log("hgkft");
        }
        
        #endregion
    }
}
