using UnityEngine;

namespace Morok {
    public class DieManager : MonoBehaviour {
        [SerializeField] private GameObject healthbar;
        public void OnDie() {
            healthbar.SetActive(false);
        }
    }
}
