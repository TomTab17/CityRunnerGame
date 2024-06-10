using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Transform Player;
    public SwipeControls Controls;

    public bool isStumbling = false;

    private bool Lane1 = false;
    private bool Lane2 = true;
    private bool Lane3 = false;
    static public bool canMove = false;
    private bool isGrounded = true;
    public GameObject playerObject;

    private void Start()
    {
        Player = GetComponent<Transform>();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 5, Space.World);
        if (canMove == true)
        {
            if (Lane3 == true)
            {
                Player.position = new Vector3(Mathf.MoveTowards(Player.position.x, 1.4f, 10.5f * Time.deltaTime), Player.position.y, Player.position.z);
            }
            else if (Lane1 == true)
            {
                Player.position = new Vector3(Mathf.MoveTowards(Player.position.x, -1.4f, 10.5f * Time.deltaTime), Player.position.y, Player.position.z);
            }
            else if (Lane2 == true)
            {
                Player.position = new Vector3(Mathf.MoveTowards(Player.position.x, 0, 10.5f * Time.deltaTime), Player.position.y, Player.position.z);
            }
        }

        #region ChangeBools
        if (Controls.swiperight == true && Lane3 == false && Lane1 == true)
        {
            playerObject.GetComponent<Animator>().StopPlayback();
            Lane2 = true;
            Lane1 = false;
            Lane3 = false;
            playerObject.GetComponent<Animator>().Play("Standing Dodge Right");
        }
        else if (Controls.swipeleft == true && Lane2 == true && Player.position.x <= 0.2f)
        {
            playerObject.GetComponent<Animator>().StopPlayback();
            Lane1 = true;
            Lane2 = false;
            Lane3 = false;
            playerObject.GetComponent<Animator>().Play("Standing Dodge Left");
        }
        else if (Controls.swiperight == true && Lane2 == true && Player.position.x >= -0.2f)
        {
            playerObject.GetComponent<Animator>().StopPlayback();
            Lane3 = true;
            Lane1 = false;
            Lane2 = false;
            playerObject.GetComponent<Animator>().Play("Standing Dodge Right");
        }
        else if (Controls.swipeleft == true && Lane1 == false && Lane3 == true)
        {
            playerObject.GetComponent<Animator>().StopPlayback();
            Lane2 = true;
            Lane1 = false;
            Lane3 = false;
            playerObject.GetComponent<Animator>().Play("Standing Dodge Left");
        }
        #endregion

        if (playerObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Standing Dodge Left") && playerObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            playerObject.GetComponent<Animator>().Play("fastrun");
        }
        if (playerObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Standing Dodge Right") && playerObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            playerObject.GetComponent<Animator>().Play("fastrun");
        }

        if (Controls.swipeup == true && canMove == true && isGrounded == true)
        {
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        isGrounded = false;
        float originalY = Player.position.y;
        float jumpHeight = 1.5f;
        float jumpDuration = 0.8f;
        float jumpProgress = 0f;

        playerObject.GetComponent<Animator>().Play("Jump");

        while (jumpProgress < 1f)
        {
            jumpProgress += Time.deltaTime / jumpDuration;
            float yOffset = Mathf.Sin(jumpProgress * Mathf.PI) * jumpHeight;
            Player.position = new Vector3(Player.position.x, originalY + yOffset, Player.position.z);
            yield return null;
            if (isStumbling)
            {
                while (Player.position.y > originalY)
                {
                    float returnSpeed = 5f;
                    Player.position = Vector3.MoveTowards(Player.position, new Vector3(Player.position.x, originalY, Player.position.z), Time.deltaTime * returnSpeed);
                    yield return null;
                }
                yield break;
            }
        }

        Player.position = new Vector3(Player.position.x, originalY, Player.position.z);
        isGrounded = true;

        playerObject.GetComponent<Animator>().Play("fastrun");
    }
}
