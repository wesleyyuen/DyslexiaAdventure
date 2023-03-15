using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance {
        get { return _instance; }
    }
    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    [SerializeField] private List<GameObject> _pedestrianPrefabs;
    public List<GameObject> PedestrianPrefabs {
        get {
            return _pedestrianPrefabs;
        }
    }
}
