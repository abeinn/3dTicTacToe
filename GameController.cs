using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public string[,,] board = new string[3,3,3] {
    {{"","",""}, {"","",""}, {"","",""}},
    {{"","",""}, {"","",""}, {"","",""}},
    {{"","",""}, {"","",""}, {"","",""}}
    };

    public List <(string, string, string)> wins = new List<(string, string, string)>();

    public int turn;
    public Sprite[] xoIcons;
    public Button[] buttonBoard;
    public Text[] scoreTexts;
    public Text winMessage;
    public Button playAgain;
    public Image turnImage;
    public GameObject turnScreen;
    public Sprite UIMask;

    private int[] iScores = new int[2] {0,0};
    private int[] oScores = new int[2] {0,0};

    private Color[] xoColors = new Color[2] {Color.red, Color.blue}; 


 
    // Start is called before the first frame update
    void Start()
    {
        System.Array.Clear(oScores,0,oScores.Length);
        GameSetup();
    }

    void GameSetup()
    {

        System.Array.Clear(iScores,0,iScores.Length);

        var r = new System.Random();
        turn = r.Next(0,2);
        turnImage.sprite = xoIcons[turn];

        board = new string[3,3,3] {
        {{"","",""}, {"","",""}, {"","",""}},
        {{"","",""}, {"","",""}, {"","",""}},
        {{"","",""}, {"","",""}, {"","",""}}
        };

        wins.Clear();

        turnScreen.SetActive(true);

        playAgain.gameObject.SetActive(false);
        winMessage.text = "";

        for(int i=0; i<buttonBoard.Length; i++) {
            if (i == 13) {
                buttonBoard[i].interactable = false;
            } else {
                buttonBoard[i].image.sprite = UIMask;
                buttonBoard[i].interactable = true;
                
                ColorBlock cb = buttonBoard[i].colors;
                cb.colorMultiplier = 1.2f;
                buttonBoard[i].colors = cb;
            }    
        } 

    }

    public void TicTacToeButton(int num)
    {
        buttonBoard[num].image.sprite = xoIcons[turn];
        buttonBoard[num].interactable = false;

        var c = NameToPos(buttonBoard[num].name);

        board[c.i,c.j,c.k] = turnToString(turn);
        
        StartCoroutine(winCoroutine());

        
    }

    private IEnumerator winCoroutine()
    {
        ColorBlock cb;
        float delay = 0.2f;

        for(int i=0;i<buttonBoard.Length;i++){
            buttonBoard[i].interactable = false;
        }

        var win = checkWin();
        
        while (win.winner != "") {
            
            var pos1 = win.pos1;
            var pos2 = win.pos2;
            var pos3 = win.pos3;

            for(int i=0;i<3;i++){

                cb = buttonBoard[posToNum(pos1)].colors;
                cb.colorMultiplier = 1.2f;
                buttonBoard[posToNum(pos1)].colors = cb;

                cb = buttonBoard[posToNum(pos2)].colors;
                cb.colorMultiplier = 1.2f;
                buttonBoard[posToNum(pos2)].colors = cb;

                cb = buttonBoard[posToNum(pos3)].colors;
                cb.colorMultiplier = 1.2f;
                buttonBoard[posToNum(pos3)].colors = cb;

                yield return new WaitForSeconds(delay);

                cb = buttonBoard[posToNum(pos1)].colors;
                cb.colorMultiplier = 5;
                buttonBoard[posToNum(pos1)].colors = cb;

                cb = buttonBoard[posToNum(pos2)].colors;
                cb.colorMultiplier = 5;
                buttonBoard[posToNum(pos2)].colors = cb;
                
                cb = buttonBoard[posToNum(pos3)].colors;
                cb.colorMultiplier = 5;
                buttonBoard[posToNum(pos3)].colors = cb;

                yield return new WaitForSeconds(delay);
            }

            int n = stringToTurn(win.winner);
            iScores[n]++;
            if(iScores[n] >= 2){
                oScores[n]++;
                winMessage.color = xoColors[n];
                winMessage.text = win.winner + " wins";
                playAgain.gameObject.SetActive(true);
                turnScreen.SetActive(false);
                for(int i=0;i<buttonBoard.Length;i++){
                    buttonBoard[i].interactable = false;
                }
            }

            win = checkWin();

        }

        if(winMessage.text == ""){
            for(int i=0;i<buttonBoard.Length;i++){
                if (buttonBoard[i].image.sprite == UIMask){
                    buttonBoard[i].interactable = true;
                }
            }
        }

        scoreTexts[0].text = "" + oScores[0];
        scoreTexts[1].text = "" + oScores[1];

        if(checkTie()){
            winMessage.color = Color.magenta;
            winMessage.text = "tie";
            playAgain.gameObject.SetActive(true);
            turnScreen.SetActive(false);
            for(int i=0;i<buttonBoard.Length;i++){
                buttonBoard[i].interactable = false;
            }
        }

        if (turn == 0){
            turn = 1; 
        } else {
            turn = 0;
        }

        turnImage.sprite = xoIcons[turn];
    }


    private bool checkTie(){
         for(int i=0;i<3;i++){
            for(int j=0;j<3;j++){
                for(int k=0;k<3;k++){
                    if (board[i,j,k] == ""){
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void playAgainOnClick(){
        GameSetup();
    }

    public static (int i, int j, int k) NameToPos (string name) 
    {
        int i = int.Parse(name.Substring(0,1));
        int j = int.Parse(name.Substring(1,1));
        int k = int.Parse(name.Substring(2,1));

        return (i,j,k);
    }

    public static int posToNum (string pos)
    {
        var c = NameToPos(pos);
        int output = c.k * 9 + c.i * 3 + c.j;
        return output;
    }

    public static string turnToString(int turn) 
    {
        if(turn == 0){
            return "x";
        } else{
            return "o";
        }
    }

    public static int stringToTurn(string s)
    {
        if(s == "x"){
            return 0;
        } else{
            return 1;
        }
    }

    
    
    private (string winner, string pos1, string pos2, string pos3) checkWin()
    {
        for(int i=0;i<3;i++) {

            for (int j=0;j<3;j++) {

                //check horizontal and vertical top, mid, and bot boards
                //outerloop = k, innerloop = i
                //this also checks the horizontal of each side board
                if (board[j,0,i] != "" && board[j,0,i] == board[j,1,i] && board[j,0,i] == board[j,2,i] && newWin(j+"0"+i, j+"1"+i, j+"2"+i)){
                    wins.Add((j+"0"+i, j+"1"+i, j+"2"+i));
                    return (board[j,0,i].Substring(0,1), j+"0"+i, j+"1"+i, j+"2"+i);
                }
                if (board[0,j,i] != "" && board[0,j,i] == board[1,j,i] && board[0,j,i] == board[2,j,i] && newWin("0"+j+i, "1"+j+i, "2"+j+i)){
                    wins.Add(("0"+j+i, "1"+j+i, "2"+j+i));
                    return (board[0,j,i].Substring(0,1), "0"+j+i, "1"+j+i, "2"+j+i);
                }

                //checks verticals of side boards
                //outerloop = i, innerloop = j
                if (board[i,j,0] != "" && board[i,j,0] == board[i,j,1] && board[i,j,0] == board[i,j,2] && newWin(""+i+j+"0", ""+i+j+"1", ""+i+j+"2")){
                    wins.Add((""+i+j+"0", ""+i+j+"1", ""+i+j+"2"));
                    return (board[i,j,0], ""+i+j+"0", ""+i+j+"1", ""+i+j+"2");
                }


            }

            //checks diagonals of top, mid, and bot boards
            //outerloop = k
            if (board[0,0,i] != "" && board[0,0,i] == board[1,1,i] && board[0,0,i] == board[2,2,i] && newWin("00"+i, "11"+i, "22"+i)) {
                wins.Add(("00"+i, "11"+i, "22"+i));
                return (board[0,0,i], "00"+i, "11"+i, "22"+i);
            }
            if (board[0,2,i] != "" && board[0,2,i] == board[1,1,i] && board[0,2,i] == board[2,0,i] && newWin("02"+i, "11"+i, "20"+i)) {
                wins.Add(("02"+i, "11"+i, "20"+i));
                return (board[0,2,i], "02"+i, "11"+i, "20"+i);
            }

            //check diagonals of i-axis side boards
            //outerloop = i
            if (board[i,0,0] != "" && board[i,0,0] == board[i,1,1] && board[i,0,0] == board[i,2,2] && newWin(i+"00", i+"11", i+"22")) {
                wins.Add((i+"00", i+"11", i+"22"));
                return (board[i,0,0],i+"00", i+"11", i+"22");
            }
            if (board[i,2,0] != "" && board[i,2,0] == board[i,1,1] && board[i,2,0] == board[i,0,2] && newWin(i+"20", i+"11", i+"02")) {
                wins.Add((i+"20", i+"11", i+"02"));
                return (board[i,2,0],i+"20", i+"11", i+"02");
            }

            //checks diagonals of j-axis side boards
            //outerloop = j
            if (board[0,i,0] != "" && board[0,i,0] == board[1,i,1] && board[0,i,0] == board[2,i,2] && newWin("0"+i+"0", "1"+i+"1", "2"+i+"2")) {
                wins.Add(("0"+i+"0", "1"+i+"1", "2"+i+"2"));
                return (board[0,i,0], "0"+i+"0", "1"+i+"1", "2"+i+"2");
            }
            if (board[2,i,0] != "" && board[2,i,0] == board[1,i,1] && board[2,i,0] == board[0,i,2] && newWin("2"+i+"0", "1"+i+"1", "0"+i+"2")) {
                wins.Add(("2"+i+"0", "1"+i+"1", "0"+i+"2"));
                return (board[2,i,0], "2"+i+"0", "1"+i+"1", "0"+i+"2");
            }
        }

        return ("", "", "", "");
    }

    private bool newWin(string pos1, string pos2, string pos3)
    {
        (string, string, string) x = (pos1, pos2, pos3);
        for(int i=0;i<wins.Count;i++){
            if(x == wins[i]){
                return false;
            }
        }

        return true;
    }

    public void quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
