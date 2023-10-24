using System.Collections;

using UnityEngine;

public class Deck : MonoBehaviour
{

    public CardData[] cardDataArray;
    public GameObject cardPrefab;
    public GameObject handGameObject;
    public GameObject rewardGameObject;
    public int handSize;
    public int rewardSize;

    // Start is called before the first frame update
    void OnEnable()
    {
        //Subscribe to events
        EventManager.onGameStart += DrawCard;
        GameManager.startGame += DrawHand;
        WaveManager.OnCompletedWave += DrawReward;
    }

    private void OnDisable()
    {
        //Subscribe to events
        GameManager.drawCard -= DrawCard;
        GameManager.startGame -= DrawHand;
        WaveManager.OnCompletedWave -= DrawReward;
    }

    public void DrawCard()
    {
        //get select random CardData
        int index = Random.Range(0, cardDataArray.Length);
        //Instantiate prefab
        GameObject newCard = Instantiate(cardPrefab, handGameObject.transform,false);
        //Write data to card;
        Card cardComponent = newCard.GetComponent<Card>();
        cardComponent.Reward = false;
        cardComponent.CardData = cardDataArray[index];
        cardComponent.RenderData();

    }

    public void DrawcardReward()
    {
        //get select random CardData
        int index = Random.Range(0, cardDataArray.Length);
        //Instantiate prefab
        GameObject newCard = Instantiate(cardPrefab, rewardGameObject.transform, false);
        //Write data to card;
        Card cardComponent = newCard.GetComponent<Card>();
        cardComponent.Reward = true;
        cardComponent.CardData = cardDataArray[index];
        cardComponent.RenderData();

    }

    public void DrawHand()
    {
        for (int i = 0; i < handSize; i++)
            DrawCard();
    }

    public void PayDraw()
    {
       if(GameManager.instance.gold >= 50 && handGameObject.transform.childCount < handSize)
        {
            DrawCard();
            GameManager.instance.gold -= 50;
            PlayerUI.instance.UpdateGold();
        }
        
    }


    public void DrawReward()
    {
        //destroy last rewards
        foreach(Transform reward in rewardGameObject.transform)
        {
            Destroy(reward.gameObject);
        }
        //draw more rewards
        for (int i = 0; i < rewardSize; i++)
            DrawcardReward();
    }


}