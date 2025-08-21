using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public TMP_Text levelInfoText1;
    public TMP_Text levelInfoText2;
    public TMP_Text levelInfoText3;
    public Image skill4CooldownImage;

    private Shooting shooting;
    private bool isSkill4OnCooldown = false;

    private Dictionary<string, SkillData> skills = new Dictionary<string, SkillData>();

    private void Start()
    {
        shooting = FindFirstObjectByType<Shooting>();

        skills["Skill1"] = new SkillData(0, 19, 20);     // Attack speed
        skills["Skill2"] = new SkillData(0, 15, 25);     // Arrow speed
        skills["Skill3"] = new SkillData(0, 3, 150);     // Arrow count

        UpdateSkillUI("Skill1", levelInfoText1);
        UpdateSkillUI("Skill2", levelInfoText2);
        UpdateSkillUI("Skill3", levelInfoText3);

        skill4CooldownImage.fillAmount = 1f;
    }

    public void UpgradeSkill1()
    {
        TryUpgradeSkill("Skill1", levelInfoText1, () =>
        {
            shooting.shootInterval = Mathf.Max(0.1f, shooting.shootInterval - 0.1f);
        });
    }

    public void UpgradeSkill2()
    {
        TryUpgradeSkill("Skill2", levelInfoText2, () =>
        {
            shooting.arrowSpeed += 1f;
        });
    }

    public void UpgradeSkill3()
    {
        TryUpgradeSkill("Skill3", levelInfoText3, () =>
        {
            shooting.arrowCount = skills["Skill3"].level + 1;
        });
    }

    public void UseSkill4()
    {
        if (isSkill4OnCooldown)
        {
            Debug.Log("Skill4 is on cooldown.");
            return;
        }

        if (CoinManager.Instance.coinCount < 350)
        {
            Debug.Log("Not enough coins for Skill4.");
            return;
        }

        CoinManager.Instance.coinCount -= 350;
        CoinManager.Instance.SendMessage("UpdateCoinUI");

        StartCoroutine(Skill4Routine());
    }

    public GameObject redHitEffect;
    public GameObject blackHitEffect;

    private IEnumerator Skill4Routine()
    {
        // 1 saniye beklemeden sonra yok etme iþlemi baþlasýn
        yield return new WaitForSeconds(0.5f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] strongEnemies = GameObject.FindGameObjectsWithTag("StrongEnemy");

        foreach (GameObject e in enemies)
        {
            if (redHitEffect != null)
            {
                GameObject effect = Instantiate(redHitEffect, e.transform.position, Quaternion.identity);
                Destroy(effect, 1.5f);
            }
            Destroy(e);
        }

        foreach (GameObject e in strongEnemies)
        {
            if (blackHitEffect != null)
            {
                GameObject effect = Instantiate(blackHitEffect, e.transform.position, Quaternion.identity);
                Destroy(effect, 1.5f);
            }
            Destroy(e);
        }

        // Cooldown baþlat (60 saniye)
        isSkill4OnCooldown = true;
        skill4CooldownImage.fillAmount = 0f;

        float cooldownTime = 60f;
        float elapsed = 0f;

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            skill4CooldownImage.fillAmount = elapsed / cooldownTime;
            yield return null;
        }

        skill4CooldownImage.fillAmount = 1f;
        isSkill4OnCooldown = false;
    }
    private void TryUpgradeSkill(string key, TMP_Text infoText, System.Action onUpgrade)
    {
        SkillData skill = skills[key];

        if (skill.level >= skill.maxLevel)
        {
            Debug.Log($"{key} is already at max level.");
            return;
        }

        if (CoinManager.Instance.coinCount >= skill.cost)
        {
            CoinManager.Instance.coinCount -= skill.cost;
            CoinManager.Instance.SendMessage("UpdateCoinUI");

            skill.level++;
            skills[key] = skill;

            onUpgrade?.Invoke();
            UpdateSkillUI(key, infoText);
        }
        else
        {
            Debug.Log($"Not enough coins to upgrade {key}.");
        }
    }

    private void UpdateSkillUI(string key, TMP_Text text)
    {
        SkillData skill = skills[key];
        text.text = skill.level >= skill.maxLevel ? "MAX" : skill.level.ToString();
    }

    private struct SkillData
    {
        public int level;
        public int maxLevel;
        public int cost;

        public SkillData(int level, int maxLevel, int cost)
        {
            this.level = level;
            this.maxLevel = maxLevel;
            this.cost = cost;
        }
    }
}
