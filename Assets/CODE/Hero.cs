using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Hero : MonoBehaviour {
    public static  Hero instance = null;

    [Header("Audio")]
    public AudioSource itemAudioSource;
    public AudioSource grassAudioSource;
    
    [Header("Wait")]
    [Range(0.1f, 5f)]
    public float waitTimeCommand = 0.5f;
    [Range(0.1f, 5f)]
    public float moveTime = 1f;

    private Queue commandQueue = new Queue();
    private GM gm;
    private bool canMove = true;


    void Awake() {
        if (instance != null && instance != this)
            Destroy(this.gameObject);

        instance = this;
    }

    void Start() {
        gm = GM.instance;
    }

    void OnTriggerEnter( Collider other ) {
        if (other.tag.Equals("Grass")) {
            gm.LaunchBattle();
            grassAudioSource.Play();
        } 
        else if (other.tag.Equals("Item")) {
            gm.EndThisLevel();
            itemAudioSource.Play();
        } 
        else { Debug.LogWarning("This should not happen"); }

        canMove = false;
    }

    private IEnumerator CallRegisteredCommands() {
        while (commandQueue.Count > 0 && canMove) {
            PlayerCommands curCommand = (PlayerCommands)commandQueue.Dequeue();
            switch (curCommand) {
                case PlayerCommands.moveForward:
                    StartCoroutine(MoveToPosition(Vector3.forward));
                    yield return new WaitForSeconds(moveTime + waitTimeCommand);
                    break;
                case PlayerCommands.moveRight:
                    StartCoroutine(MoveToPosition(Vector3.right));
                    yield return new WaitForSeconds(moveTime + waitTimeCommand);
                    break;
                case PlayerCommands.moveDown:
                    StartCoroutine(MoveToPosition(Vector3.back));
                    yield return new WaitForSeconds(moveTime + waitTimeCommand);
                    break;
                case PlayerCommands.moveLeft:
                    StartCoroutine(MoveToPosition(Vector3.left));
                    yield return new WaitForSeconds(moveTime + waitTimeCommand);
                    break;
            }
        }

        // OUR COMMANDS DO NOT LEAD US TO THE END OF THE LEVEL
        gm.ReloadLevel();
        yield return null;
    }

    public void Inform() {
        Debug.Log("Player::Inform");
        commandQueue.Clear();
        canMove = true;
    }
    public void RunRegisterdCommands() {
        Debug.Log("Player::RunRegisterdCommands");
        StartCoroutine(CallRegisteredCommands());
    }

    public void moveUp() { commandQueue.Enqueue(PlayerCommands.moveForward); }
    public void moveRight() { commandQueue.Enqueue(PlayerCommands.moveRight); }
    public void moveDown() { commandQueue.Enqueue(PlayerCommands.moveDown); }
    public void moveLeft() { commandQueue.Enqueue(PlayerCommands.moveLeft); }


    private IEnumerator MoveToPosition( Vector3 direction ) {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        while (elapsedTime < moveTime) {
            transform.position = Vector3.Lerp(startingPos, startingPos + direction, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }


        private void Move( Vector3 direction ) {
            transform.position += transform.TransformDirection(direction);
        }
    
        private bool CanMove( Vector3 direction ) {
            Vector3 fwd = transform.TransformDirection(direction);
            Debug.DrawRay(transform.position, fwd, Color.red, 1f);
            return Physics.Raycast(transform.position, fwd, 1);
        }
}

public enum PlayerCommands { moveForward, moveRight, moveDown, moveLeft }