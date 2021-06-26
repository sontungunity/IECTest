using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;

public class FireBaseManager : MonoBehaviour {
    static protected FireBaseManager s_Instance;

    public DatabaseReference DBreference;
    void Awake() {
        if(s_Instance != null) {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        DBreference = FirebaseDatabase.GetInstance("https://first-project-54f58-default-rtdb.firebaseio.com/").RootReference;
    }

    private void OnApplicationQuit() {
        var DBTask = DBreference.Child("User").Child(SystemInfo.deviceUniqueIdentifier).Child("Coin").SetValueAsync(PlayerData.instance.coins).ContinueWith(task=>{
            if(task.IsCompleted){
                Debug.Log("successfully added data to firebase");
            }else{
                Debug.Log("not successfull");
            }
        });
    }
}
