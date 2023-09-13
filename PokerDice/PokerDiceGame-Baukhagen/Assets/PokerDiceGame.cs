//////////////////////////////////////////////
//Assignment/Lab/Project: Poker Dice Project
//Name: Zebulun Baukhagen
//Section: 2023SP.SGD.213.2172
//Instructor: Brian Sowers
//Date: 01/15/2023
/////////////////////////////////////////////

using TMPro;
using UnityEngine;

public class PokerDiceGame : MonoBehaviour
{
    // import all of our UI assets
    [SerializeField] TextMeshProUGUI playerDice1;
    [SerializeField] TextMeshProUGUI playerDice2;
    [SerializeField] TextMeshProUGUI playerDice3;
    [SerializeField] TextMeshProUGUI playerDice4;
    [SerializeField] TextMeshProUGUI playerDice5;

    [SerializeField] TextMeshProUGUI computerDice1;
    [SerializeField] TextMeshProUGUI computerDice2;
    [SerializeField] TextMeshProUGUI computerDice3;
    [SerializeField] TextMeshProUGUI computerDice4;
    [SerializeField] TextMeshProUGUI computerDice5;

    [SerializeField] TextMeshProUGUI playerHandInfoBox;
    [SerializeField] TextMeshProUGUI computerHandInfoBox;
    [SerializeField] TextMeshProUGUI endMessage;
    [SerializeField] TextMeshProUGUI rollsLeftNum;

    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject endPanel;

    int[] playerHand = new int[5]; // store the players dice
    int[] playerHandValues = new int[6]; // store the valueArray for determining scores
    int[] playerScore = new int[3]; // store the players score in a 3 element array where score[handType, highValue, lowValue]

    int playerRolls = 3; // track the rolls
    bool playerDone = false;
    bool computerDone = false;

    int[] computerHand = new int[5]; // store the computers dice
    int[] computerHandValues = new int[6]; // store the valueArray for determining scores
    int[] computerScore = new int[3]; // store the computer's score

    bool? winner = null; // false for player, true for computer, null for tie

    // Start is called before the first frame update
    void Start()
    {
        // set all of our panels
        titlePanel.SetActive(true);
        gamePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    int[] DetermineScoreOfHandValues(int[] handValues)
    {
        // take the handValue arrays and determine which pokerhand they correlate to
        // outputs a 3 element array where score[handType, highValue, lowValue] such as score[2,3,0] which indicates a pair of 3's

        int[] score = new int[3];
        // these variables just make the code more easily read
        int handType = 0;
        int highValue = 1;
        int lowValue = 2;
        int fiveOfAKind = 6;
        int fourOfAKind = 5;
        int fullHouse = 4;
        int threeOfAKind = 3;
        int twoPairs = 2;
        int onePair = 1;
        ///////////////////////////////////////////////////////

        for (int i = 0; i < 6; i++) // iterate through the handValues array provided from large to small
            if (handValues[i] == 5) // check for five of a kind
            {
                score[handType] = fiveOfAKind;
                score[highValue] = i + 1;
                return score;
            }
            else if (handValues[i] == 4) // check for four of a kind
            {
                Debug.Log("Found four of a kind...");
                score[handType] = fourOfAKind;
                score[highValue] = i + 1;
                Debug.Log("Score array after determination: " + ConvertArrayToString(score));
                return score;
            }
            else if (handValues[i] == 3) // check for three of a kind, full house is possible
            {
                Debug.Log("Found three of a kind...");
                score[handType] = threeOfAKind;
                score[highValue] = i + 1;

                foreach (int element in handValues) // check for full house, change hand type and assign value of pair to lowValue if so
                {
                    if (handValues[element] == 2)
                    {
                        Debug.Log("Found full house...");
                        score[handType] = fullHouse;
                        score[lowValue] = handValues[element];
                    }
                }
                Debug.Log("Score array after determination: " + ConvertArrayToString(score));
                return score;
            }
            else if (handValues[i] == 2) // check for one pair or two pairs
            {
                Debug.Log("Found one pair...");
                int firstPair = i + 1;
                int secondPair;
                score[handType] = onePair;
                score[highValue] = firstPair;

                for (int j = i + 1; j < 6; j++) // continue iteration where we left off to search the rest of the array for another pair or a full house
                {
                    if (handValues[j] == 2) // if we find one, we have a two pair hand
                    {
                        Debug.Log("Found two pairs...");
                        score[handType] = twoPairs;
                        secondPair = j + 1;
                        score[highValue] = secondPair;
                        score[lowValue] = firstPair;
                        Debug.Log("Second pair value is..." + secondPair.ToString());
                    }
                    else if (handValues[j] == 3) // if we find a three of a kind, we have a full house
                    {
                        Debug.Log("Found full house with pair first...");
                        score[handType] = fullHouse;
                        secondPair = firstPair; // shift the values to reflect the full house
                        firstPair = j + 1;
                        score[highValue] = firstPair;
                        score[lowValue] = secondPair;
                        Debug.Log("Full house with pair value is..." + firstPair.ToString() + secondPair.ToString());
                    }
                }
                Debug.Log("Score array after determination: " + ConvertArrayToString(score));
                return score;
            }
        return score;
    }

    bool? DetermineWinningHand(int[] playerScore, int[] computerScore)
    {
        // takes two score[0, 0, 0] parameters and determines which is the winning hand
        bool? winningHand = null; // null for tie, false for player win, true for computer win
        Debug.Log("Determining winner...");
        if (playerScore[0] == computerScore[0]) // test for tie
        {
            Debug.Log("Hand is a tie...");
            if (playerScore[1] == computerScore[1]) // test for second tie
            {
                Debug.Log("First pair is a tie...");
                if (playerScore[2] == computerScore[2]) // test for third tie
                {
                    Debug.Log("Complete tie...");
                    return winningHand; // return null if total tie
                }
                else if (playerScore[2] > computerScore[2])
                {
                    Debug.Log("Second pair player wins...");
                    winningHand = false;
                    return winningHand; // player win
                }
                else if (playerScore[2] < computerScore[2])
                {
                    Debug.Log("Second pair player loses...");
                    winningHand = true;
                    return winningHand; // computer win
                }
            }
            else if (playerScore[1] > computerScore[1])
            {
                Debug.Log("First pair player wins...");
                winningHand = false;
                return winningHand; // player win
            }
            else if (playerScore[1] < computerScore[1])
            {
                Debug.Log("First pair player loses...");
                winningHand = true;
                return winningHand; // computer win
            }
        }
        else if (playerScore[0] > computerScore[0])
        {
            Debug.Log("Player hand is higher...");
            winningHand = false;
            return winningHand; // player win
        }
        else if (playerScore[0] < computerScore[0])
        {
            Debug.Log("Computer hand is higher...");
            winningHand = true;
            return winningHand; // computer win
        }
        return winningHand;
    }

    int[] ConvertHandToValues(int[] hand)
    {
        // takes the raw dice hand and converts them into an array where the index indicates the number value, and the value indicates the number of dice that had that number value
        int[] valueArray = new int[6]; // could be 5<
        Debug.Log("ValueArray before filling :" + ConvertArrayToString(valueArray));
        foreach (int element in hand)
        {
            if (element == 1)
            {
                valueArray[0]++;
            }
            else if (element == 2)
            {
                valueArray[1]++;
            }
            else if (element == 3)
            {
                valueArray[2]++;
            }
            else if (element == 4)
            {
                valueArray[3]++;
            }
            else if (element == 5)
            {
                valueArray[4]++;
            }
            else if (element == 6)
            {
                valueArray[5]++;
            }
        }
        Debug.Log("valueArray after filling :" + ConvertArrayToString(valueArray));
        return valueArray;
    }

    public void OnClickRollButton()
    {
        // when the player clicks the roll button, roll a new hand, decrement the roll counter, convert the hand to values and determine their score
        if (playerRolls > 0 && playerDone == false)
        {
            Debug.Log("Player hand before roll: " + ConvertArrayToString(playerHand));

            playerHand = RollNewHand();

            Debug.Log("Player hand after roll: " + ConvertArrayToString(playerHand));

            playerRolls--;

            Debug.Log("Player hand before conversion to values: " + ConvertArrayToString(playerHand));
            playerHandValues = ConvertHandToValues(playerHand);

            Debug.Log("Player hand values after conversion to values: " + ConvertArrayToString(playerHandValues));

            playerScore = DetermineScoreOfHandValues(playerHandValues);

            Debug.Log("playerScore array after determination: " + ConvertArrayToString(playerScore));
        }
        UpdateText();
    }

    public void OnClickEndTurnButton()
    {
        // end player turn and execute computer turn
        playerDone = true;
        ComputerTurn();
    }

    public void OnClickStartButton()
    {
        // move to the game panel
        titlePanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void OnClickPlayAgainButton()
    {
        // refresh everything and close the end panel
        Setup();
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    int[] RollNewHand()
    {
        // returns a fresh array of length 5 with integers between 1 and 6 inclusive
        int[] newArray = new int[5];
        for (int i = 0; i < 5; i++)
        {
            int diceRoll = Random.Range(1, 7);
            newArray[i] = diceRoll;
        }
        Debug.Log("Hand rolled: " + ConvertArrayToString(newArray));
        return newArray;
    }

    string ConvertArrayToString(int[] array)
    {
        // debugging tool
        // convert any given array to a string, return string
        string newString = "";
        foreach (int item in array)
            newString += item.ToString();
        return newString;
    }

    void Setup()
    {
        // refresh everything by reseting values and then updating the text
        playerHand = new int[5];
        playerHandValues = new int[6];
        playerScore = new int[3];
        playerRolls = 3;
        playerDone = false;
        computerDone = false;

        computerHand = new int[5];
        computerHandValues = new int[6];
        computerScore = new int[3];

        winner = null;

        endMessage.text = "";
        playerHandInfoBox.text = "";
        computerHandInfoBox.text = "";
        rollsLeftNum.text = playerRolls.ToString();

        UpdateText();

        endPanel.SetActive(false);
    }

    void ComputerTurn()
    {
        // same as OnClickRollButton() but for the computer turn // This could have all been a single method
        if (computerDone == false)
        {
            Debug.Log("Computer hand before roll: " + ConvertArrayToString(computerHand));

            computerHand = RollNewHand();

            Debug.Log("Computer hand after roll: " + ConvertArrayToString(computerHand));

            Debug.Log("Computer hand before conversion to values: " + ConvertArrayToString(computerHand));
            computerHandValues = ConvertHandToValues(computerHand);
            Debug.Log("Computer hand values after conversion to values: " + ConvertArrayToString(computerHandValues));

            computerScore = DetermineScoreOfHandValues(computerHandValues);
            Debug.Log("computerScore array after determination: " + ConvertArrayToString(computerScore));
        }

        computerDone = true;
        winner = DetermineWinningHand(playerScore, computerScore);
        UpdateText();

        EndGame();
    }

    void EndGame()
    {
        // activate end game panel
        endPanel.SetActive(true);
    }

    void UpdateText()
    {
        // refresh all text fields and make sure everything is up to date

        // "You have..." messages
        string[] youHave = new string[] { "Nothing!", "One pair!", "Two pairs!", "Three of a kind!", "A full house!", "Four of a kind!", "Five of a kind!" };

        playerDice1.text = playerHand[0].ToString(); // These could have been arrays at the top
        playerDice2.text = playerHand[1].ToString();
        playerDice3.text = playerHand[2].ToString();
        playerDice4.text = playerHand[3].ToString();
        playerDice5.text = playerHand[4].ToString();

        rollsLeftNum.text = playerRolls.ToString();

        for (int i = 6; i > -1; i--)
        {
            if (playerScore[0] == i)
            {
                playerHandInfoBox.text = youHave[i];
            }
        }


        computerDice1.text = computerHand[0].ToString();
        computerDice2.text = computerHand[1].ToString();
        computerDice3.text = computerHand[2].ToString();
        computerDice4.text = computerHand[3].ToString();
        computerDice5.text = computerHand[4].ToString();

        for (int i = 6; i > -1; i--)
        {
            if (computerScore[0] == i)
            {
                computerHandInfoBox.text = youHave[i];
            }
        }

        if (winner == false)
        {
            endMessage.text = "The hairless monkey with anxiety!";
        }
        else if (winner == true)
        {
            endMessage.text = "The organized pile of rocks!";
        }
        else
        {
            endMessage.text = "Nobody wins. Just like real life!";
        }
    }
}
