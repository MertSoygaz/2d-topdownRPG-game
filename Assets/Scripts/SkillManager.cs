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
    [SerializeField] private GameObject redHitEffect;
    [SerializeField] private GameObject blackHitEffect;

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
            _shooting.ShootInterval = Mathf.Max(0.1f, _shooting.ShootInterval - 0.1f);
        });
    }

    public void UpgradeSkill2()
    {
        TryUpgradeSkill("Skill2", levelInfoText2, () =>
        {
            _shooting.ArrowSpeed += 1f;
        });
    }

    public void UpgradeSkill3()
    {
        TryUpgradeSkill("Skill3", levelInfoText3, () =>
        {
            _shooting.ArrowCount = _skills["Skill3"].Level + 1;
        });
    }

    public void UseSkill4()
    {
        if (_isSkill4OnCooldown)
        {
            Debug.Log("Skill4 is on cooldown.");
            return;
        }

        if (CoinManager.Instance.CoinCount < 350)
        {
            Debug.Log("Not enough coins for Skill4.");
            return;
        }

        CoinManager.Instance.CoinCount -= 350;
        CoinManager.Instance.UpdateCoinUI();
        StartCoroutine(Skill4Routine());
    }
    
    private IEnumerator Skill4Routine()
    {
        yield return new WaitForSeconds(0.5f);

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var strongEnemies = GameObject.FindGameObjectsWithTag("StrongEnemy");

        foreach (var e in enemies)
        {
            if (redHitEffect is not null)
            {
                var effect = Instantiate(redHitEffect, e.transform.position, Quaternion.identity);
                Destroy(effect, 1.5f);
            }
            Destroy(e);
        }

        foreach (var e in strongEnemies)
        {
            if (blackHitEffect is not null)
            {
                var effect = Instantiate(blackHitEffect, e.transform.position, Quaternion.identity);
                Destroy(effect, 1.5f);
            }
            Destroy(e);
        }

        _isSkill4OnCooldown = true;
        skill4CooldownImage.fillAmount = 0f;

        const float cooldownTime = 60f;
        var elapsed = 0f;

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
        var skill = _skills[key];

        if (skill.Level >= skill.MaxLevel)
        {
            Debug.Log($"{key} is already at max level.");
            return;
        }

        if (CoinManager.Instance.CoinCount >= skill.Cost)
        {
            CoinManager.Instance.CoinCount -= skill.Cost;
            CoinManager.Instance.UpdateCoinUI();

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
        var skill = _skills[key];
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
