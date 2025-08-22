using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelInfoText1;
    [SerializeField] private TMP_Text levelInfoText2;
    [SerializeField] private TMP_Text levelInfoText3;
    [SerializeField] private Image skill4CooldownImage;

    private Shooting _shooting;
    private bool _isSkill4OnCooldown;

    private readonly Dictionary<string, SkillData> _skills = new Dictionary<string, SkillData>();

    private void Start()
    {
        _shooting = FindFirstObjectByType<Shooting>();

        _skills["Skill1"] = new SkillData(0, 19, 20);     // Attack speed
        _skills["Skill2"] = new SkillData(0, 15, 25);     // Arrow speed
        _skills["Skill3"] = new SkillData(0, 3, 150);     // Arrow count

        UpdateSkillUI("Skill1", levelInfoText1);
        UpdateSkillUI("Skill2", levelInfoText2);
        UpdateSkillUI("Skill3", levelInfoText3);

        skill4CooldownImage.fillAmount = 1f;
    }

    public void UpgradeSkill1()
    {
        TryUpgradeSkill("Skill1", levelInfoText1, () =>
        {
            _shooting.shootInterval = Mathf.Max(0.1f, _shooting.shootInterval - 0.1f);
        });
    }

    public void UpgradeSkill2()
    {
        TryUpgradeSkill("Skill2", levelInfoText2, () =>
        {
            _shooting.arrowSpeed += 1f;
        });
    }

    public void UpgradeSkill3()
    {
        TryUpgradeSkill("Skill3", levelInfoText3, () =>
        {
            _shooting.arrowCount = _skills["Skill3"].Level + 1;
        });
    }

    public void UseSkill4()
    {
        if (_isSkill4OnCooldown)
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

        // Cooldown ba�lat (60 saniye)
        _isSkill4OnCooldown = true;
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
        _isSkill4OnCooldown = false;
    }
    private void TryUpgradeSkill(string key, TMP_Text infoText, System.Action onUpgrade)
    {
        SkillData skill = _skills[key];

        if (skill.Level >= skill.MaxLevel)
        {
            Debug.Log($"{key} is already at max level.");
            return;
        }

        if (CoinManager.Instance.coinCount >= skill.Cost)
        {
            CoinManager.Instance.coinCount -= skill.Cost;
            CoinManager.Instance.SendMessage("UpdateCoinUI");

            skill.Level++;
            _skills[key] = skill;

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
        SkillData skill = _skills[key];
        text.text = skill.Level >= skill.MaxLevel ? "MAX" : skill.Level.ToString();
    }

    private struct SkillData
    {
        public int Level;
        public readonly int MaxLevel;
        public readonly int Cost;

        public SkillData(int level, int maxLevel, int cost)
        {
            this.Level = level;
            this.MaxLevel = maxLevel;
            this.Cost = cost;
        }
    }
}
