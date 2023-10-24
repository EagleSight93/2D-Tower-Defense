using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text playerGoldText;
    public TMP_Text waveText;
    public GameObject rewardPanel;
    public GameObject handGameObject;

    public static PlayerUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }


    private void OnEnable()
    {
        GameManager.startGame += UpdateGold;
        GameManager.startGame += ShowWaveText;
        WaveManager.OnCompletedWave += OpenRewardPanel;
        Card.OnPickedReward += CloseRewardPanel;
        Card.OnPickedReward += ShowWaveText;
    }

    private void OnDisable()
    {
        GameManager.startGame -= UpdateGold;
        GameManager.startGame -= ShowWaveText;
        WaveManager.OnCompletedWave -= OpenRewardPanel;
        Card.OnPickedReward -= CloseRewardPanel;
        Card.OnPickedReward -= ShowWaveText;
    }



    public void SetGold(int gold)
    {
        playerGoldText.text = gold.ToString();  
    }

    public void UpdateGold()
    {
        playerGoldText.text = GameManager.instance.gold.ToString();
    }


    public void ShowWaveText()
    {
        StartCoroutine(ShowWave());
    }

    public IEnumerator ShowWave()
    {
        waveText.enabled = true;
        waveText.text = "Wave " + WaveManager.instance.currentWave;
        yield return new WaitForSeconds(2.5f);

        waveText.enabled = false;
    }


    public void OpenRewardPanel()
    {
        rewardPanel.SetActive(true);
    }
    public void CloseRewardPanel()
    {
        rewardPanel.SetActive(false);
    }
}
