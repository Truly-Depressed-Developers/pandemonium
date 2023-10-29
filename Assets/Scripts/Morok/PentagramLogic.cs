using UnityEngine;

namespace Morok {
    public class PentagramLogic : MonoBehaviour {
        [SerializeField] private GameObject contactZone;
        
        public void EnableDamageZone() {
            contactZone.SetActive(true);
        }

        public void DisableDamageZone() {
            contactZone.SetActive(false);
        }

        public void DestroyPentagram() {
            Destroy(gameObject);
        }
    }
}
