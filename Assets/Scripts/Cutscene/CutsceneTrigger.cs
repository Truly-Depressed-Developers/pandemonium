using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Cutscenes {
    public class CutsceneTrigger : MonoBehaviour {

        [SerializeField] private Cutscene cutscene;

        [SerializeField] private List<Vector2> spawnLocations;

        [SerializeField] private List<GameObject> toEnable;
        [SerializeField] private List<GameObject> toDisable;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                Debug.Log("Weszło do chyja");
                foreach (GameObject o in toEnable) 
                    o.SetActive(true);

                foreach (GameObject o in toDisable) 
                    o.SetActive(false);
                
                foreach (Vector2 spawnLocation in spawnLocations)
                    EnemyManager.I.SpawnGhost(spawnLocation);
                
                if (cutscene)
                    CutsceneController.I.DisplayCutscene(cutscene);
                
                gameObject.SetActive(false);
            }
        }
    }
}
