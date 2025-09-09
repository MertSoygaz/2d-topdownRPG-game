using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Skill; 
public class SkillManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelInfoText1;
    [SerializeField] private TMP_Text levelInfoText2;
    [SerializeField] private TMP_Text levelInfoText3;
    
    [SerializeField] private Image skill4CooldownImage;
    [SerializeField] private GameObject redHitEffect;
    [SerializeField] private GameObject blackHitEffect;

    [Header("Skill Data (ScriptableObject)")]
    [SerializeField] private SkillData skillData;

    private Shooting _shooting;
    private bool _isSkill4OnCooldown;
    private int _skill1Level;
    private int _skill2Level;
    private int _skill3Level;

    private void Start()
    {
        _shooting = FindFirstObjectByType<Shooting>();

        UpdateSkillUI(SkillType.Skill1, levelInfoText1, _skill1Level);
        UpdateSkillUI(SkillType.Skill2, levelInfoText2, _skill2Level);
        UpdateSkillUI(SkillType.Skill3, levelInfoText3, _skill3Level);

        skill4CooldownImage.fillAmount = 1f;
    }

    public void UpgradeSkill1()
    {
        var info = skillData.GetSkill(SkillType.Skill1);
        TryUpgradeSkill(ref _skill1Level, info, levelInfoText1, () =>
        {
            _shooting.ShootInterval = Mathf.Max(0.1f, _shooting.ShootInterval - 0.1f);
        });
    }

    public void UpgradeSkill2()
    {
        var info = skillData.GetSkill(SkillType.Skill2);
        TryUpgradeSkill(ref _skill2Level, info, levelInfoText2, () =>
        {
            _shooting.ArrowSpeed += 1f;
        });
    }

    public void UpgradeSkill3()
    {
        var info = skillData.GetSkill(SkillType.Skill3);
        TryUpgradeSkill(ref _skill3Level, info, levelInfoText3, () =>
        {
            _shooting.ArrowCount = _skill3Level + 1;
        });
    }

    public void UseSkill4()
    {
        var info = skillData.GetSkill(SkillType.Skill4);

        if (_isSkill4OnCooldown)
        {
            Debug.Log("Skill4 is on cooldown.");
            return;
        }

        if (CoinManager.Instance.CoinCount < info.coinCost)
        {
            Debug.Log("Not enough coins for Skill4.");
            return;
        }

        CoinManager.Instance.CoinCount -= info.coinCost;
        CoinManager.Instance.UpdateCoinUI();
        StartCoroutine(Skill4Routine(info.cooldownTime));
    }
    
    private IEnumerator Skill4Routine(float cooldownTime)
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

    private void TryUpgradeSkill(ref int currentLevel, SkillInfo info, TMP_Text uiText, System.Action onUpgrade)
    {
        if (currentLevel >= info.maxLevel)
        {
            Debug.Log($"{info.skillType} is already at max level.");
            return;
        }

        if (CoinManager.Instance.CoinCount >= info.coinCost)
        {
            CoinManager.Instance.CoinCount -= info.coinCost;
            CoinManager.Instance.UpdateCoinUI();

            currentLevel++;
            onUpgrade?.Invoke();
            UpdateSkillUI(info.skillType, uiText, currentLevel);
        }
        else
        {
            Debug.Log($"Not enough coins to upgrade {info.skillType}.");
        }
    }

    private void UpdateSkillUI(SkillType type, TMP_Text text, int level)
    {
        var info = skillData.GetSkill(type);
        text.text = level >= info.maxLevel ? "MAX" : level.ToString();
    }
}
