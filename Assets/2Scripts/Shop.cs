using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public Animator anim;

    public GameObject[] itemObject;
    public int[] itemPrice;
    public Transform[] itemPos;
    public string[] talkData;
    public Text talkText;

    Player enterPlayer;

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero; // 화면 정중앙으로 이동
    }

    public void Exit()
    {
        anim.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000f;
    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if(price > enterPlayer.coin)
        {
            StopCoroutine(Talk()); // 여러번 상호작용으로 인해 코루틴이 꼬이는 것을 방지
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.coin -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);

        Instantiate(itemObject[index], itemPos[index].position + ranVec, itemPos[index].rotation);
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
