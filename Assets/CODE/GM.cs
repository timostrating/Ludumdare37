using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour {
    public static  GM instance = null;

    private void Awake() {
         if (instance != null && instance != this)
             Destroy(this.gameObject);
 
         instance = this;
    }


    [Header("References")]
    public Text levelText;
    public InputField codeText;
    public GameObject player;
    public GameObject item;

    [Header("Effects")]
    public Material screenEffect;

    [Header("Settings")]
    public LevelSettings[] levels;
    private int curLevel = 0;



    private void Start() {
        LoadLevel(0);
        screenEffect.SetFloat("_Cutoff", 0);
    }

    public void EndThisLevel() {
        StartCoroutine( IEEndThisLevel() );
    }

    private IEnumerator IEEndThisLevel() {
        yield return new WaitForSeconds( 1.5f );
        NextLevel();
        yield return null;
    }

    public void ReloadLevel() { LoadLevel(curLevel); }
    public void NextLevel() { curLevel++; LoadLevel(curLevel); }
    public void LoadLevel(int i) {
        levelText.text = levels[i].levelName;
        codeText.text = levels[i].code;

        player.transform.position = levels[i].playerPosition.position;
        item.transform.position = levels[i].itemPosition.position;

        foreach (GameObject t in levels[i].setActive) {
            t.SetActive( true );
        }
        foreach (GameObject t in levels[i].setDisabled) {
            t.SetActive( false );
        }
    }

    public void LaunchBattle() {
        StartCoroutine( IELaunchBattle() );
    }

    private IEnumerator IELaunchBattle() {
        float elapsedTime = 0;
        while (elapsedTime < 1f) {
            screenEffect.SetFloat("_Cutoff", elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        screenEffect.SetFloat("_Cutoff", 1);
        yield return new WaitForEndOfFrame();
        screenEffect.SetFloat("_Cutoff", 0);
        ReloadLevel();
        yield return null;
    }

    float Remap(float value, float a1, float a2, float b1, float b2) {
        return b1 + (value-a1)*(b2-b1)/(a2-a1);
    }
}

[System.Serializable]
public class LevelSettings {
    public string levelName = "";
    [TextArea]
    public string code = "";
    public Transform playerPosition;
    public Transform itemPosition;
    public GameObject[] setActive;
    public GameObject[] setDisabled;
}