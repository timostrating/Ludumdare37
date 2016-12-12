using UnityEngine;
using System.Collections;

public class EndGoal : MonoBehaviour {
    public static  EndGoal instance = null;

    private void Awake() {
         if (instance != null && instance != this)
             Destroy(this.gameObject);
 
         instance = this;
    }

    public void Inform() {
        GM.instance.NextLevel();
    }
}
