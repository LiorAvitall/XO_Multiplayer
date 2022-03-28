using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Protocol
    /*
     * This is a proposal only
     Message = playerID [space] positionID

     int that represents position of the player's step.
     [0][1][2]
     [3][4][5]
     [6][7][8]
     */
    #endregion
    public int playernum = 0;
    public int player1Port = 40000;
    public int player2Port = 40001;
    public bool MyTurn = true;
    public NetworkManager networkManager; //don't forget to drag in inspector

    [Header("Input Settings")]
    [SerializeField] private LayerMask boxLayermask;
    [SerializeField] private float touchRadius;

    [Header("Mark Sprites")]
    [SerializeField] private Sprite spritex;
    [SerializeField] private Sprite spriteo;

    [Header("Mark Color")]
    [SerializeField] private Color colorx;
    [SerializeField] private Color coloro;
    Box box1;
    public Mark[] marks;
    private Camera cam;
    private Mark currentMark;

    public void AssignPlayer(int index)
    {
        if (index == 1)
        {
            playernum = index;
            networkManager.ListeningPort = player1Port;
            networkManager.SendingPort = player2Port;
            MyTurn = true;
        }
        else if (index == 2)
        {
            playernum = index;
            networkManager.ListeningPort = player2Port;
            networkManager.SendingPort = player1Port;
            MyTurn = false;
        }
        networkManager.StartUDP();
    }



    public void GotNetworkMessage(string message)
    {
        Debug.Log("got network message: " + message);

        string catchNumbers = new string(message.Where(char.IsDigit).ToArray());
        if (playernum == 1)
        {
            box1.SetMarked(spritex, currentMark, colorx);
        }
        else box1.SetMarked(spriteo, currentMark, coloro);


    }
    public void PositionClicked(int position)
    {
        //draw the shape on the UI

        //update the other player about the shape
        networkManager.SendMessage($"{position}");// your job to finish it
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            networkManager.SendMessage("A was sent");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            networkManager.SendMessage("B was sent");
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapCircle(touchPosition, touchRadius, boxLayermask);
            if (hit)
            {
                HitBox(hit.GetComponent<Box>());
            }
        }

    }

    public void HitBox(Box box)
    {
        if (!box.isMarked)
        {
            marks[box.index] = currentMark;
            box.SetMarked(GetSprite(), currentMark, GetColor());
            SwitchPlayer();
        }
    }

    private void SwitchPlayer()
    {
        currentMark = (currentMark == Mark.X) ? Mark.O : Mark.X;
    }
    private Color GetColor()
    {
        return (currentMark == Mark.X) ? colorx : coloro;
    }
    private Sprite GetSprite()
    {
        return (currentMark == Mark.X) ? spritex : spriteo;
    }
    private void Start()
    {
        cam = Camera.main;
        currentMark = Mark.X;
        marks = new Mark[9];
    }

}