using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuCamera;
    public GameObject gameCamera;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    public Text curScoreTxt;
    public Text bestTxt;

    void Awake()
    {
        enemyList = new List<int>();
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

        if(PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
    }

    public void GameStart()
    {
        menuCamera.SetActive(false);
        gameCamera.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        overPanel.SetActive(true);
        gamePanel.SetActive(false);
        curScoreTxt.text = scoreTxt.text;
        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if(player.score > maxScore)
        {
            bestTxt.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void StageStart()
    {
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        player.transform.position = Vector3.up * 0.0f;

        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++;
    }

    IEnumerator InBattle()
    {
        if(stage % 5 == 0) // 5스테이지 마다 보스 소환
        {
            enemyCntD++;
            GameObject instantEnemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            boss = instantEnemy.GetComponent<Boss>();
        }
        else
        {
            for (int index = 0; index < stage; index++)
            {
                int ran = Random.Range(0, 3); // 몬스터 index
                enemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;

                }
            }

            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4); // 몬스터 위치
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform; // 타겟 트랜스폼 가져오기    
                enemy.manager = this;
                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(4f);
            }
        }
         
        while(enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(4f);

        boss = null;
        StageEnd();
    }

    private void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        // 상단 UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE " + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        // 플레이어 UI
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null)
            playerAmmoTxt.text = "- / " + player.ammo;
        else if(player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = "- / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.equipWeapon.maxAmmo;

        // 무기 UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        // 몬스터 숫자 UI
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();

        // 보스 체력 UI
        if(boss != null)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30;
            bossHealthBar.sizeDelta = new Vector2(800 * ((float)boss.curHealth / (float)boss.maxHealth), bossHealthBar.sizeDelta.y);
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 60;
        }
    }
}
