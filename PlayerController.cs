// PlayerController.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static Cinemachine.DocumentationSortingAttribute;
using TMPro;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float currentMoveSpeed;
    private Vector2 movement;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;

    public int experience = 0;
    public int level = 1;
    public int experienceToNextLevel = 100;

    public TMP_Text levelText;
    public Image experienceBar;

    public List<WeaponBase> weapons = new List<WeaponBase>();
    public WeaponSelectionUI weaponSelectionUI;

    private Coroutine speedBuffCoroutine;
    [SerializeField] private GameObject swordPrefab; // Assign this in the inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        currentMoveSpeed = moveSpeed;
        if (weapons == null)
        {
            weapons = new List<WeaponBase>();
        }
        InitializeStartingWeapon();
        UpdateUI();

    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        foreach (WeaponBase weapon in weapons)
        {
            weapon.Fire();
        }
        //UpdateUI();
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        newPosition = MapBoundary.Instance.ClampPosition(newPosition);
        rb.MovePosition(newPosition);

        Vector2 aimDirection = InputManager.Instance.GetAimDirection();
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void InitializeStartingWeapon()
    {
        if (swordPrefab != null)
        {
            GameObject swordObject = Instantiate(swordPrefab, transform);
            MeleeWeapon swordWeapon = swordObject.GetComponent<MeleeWeapon>();

            if (swordWeapon != null)
            {
                weapons.Add(swordWeapon);
                // You might want to position the sword properly here
                swordObject.transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.LogError("Sword prefab does not have a MeleeWeapon component!");
            }
        }
        else
        {
            Debug.LogError("Sword prefab is not assigned in the PlayerController!");
        }
    }
    public void CollectExperience(int amount)
    {
        experience += amount;
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = (int)(experienceToNextLevel * 1.5f);
        moveSpeed += 0.2f;
        currentMoveSpeed = moveSpeed;
        playerHealth.maxHealth += 10;
        playerHealth.Heal(playerHealth.maxHealth);

        if (weaponSelectionUI != null)
        {
            weaponSelectionUI.ShowWeaponSelection();
        }
        else
        {
            Debug.LogError("WeaponSelectionUI is not assigned to PlayerController!");
        }

        UpdateUI();
    }
    void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
        if (experienceBar != null)
        {
            experienceBar.fillAmount =   experience/(float)experienceToNextLevel;
            Debug.Log(experienceBar.fillAmount);
            
        }
    }
    public void ApplySpeedBuff(float multiplier, float duration)
    {
        if (speedBuffCoroutine != null)
        {
            StopCoroutine(speedBuffCoroutine);
        }
        speedBuffCoroutine = StartCoroutine(SpeedBuffCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBuffCoroutine(float multiplier, float duration)
    {
        float originalSpeed = currentMoveSpeed;
        currentMoveSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        currentMoveSpeed = originalSpeed;
        speedBuffCoroutine = null;
    }
    public void AddWeapon(WeaponBase weaponPrefab)
    {
        
        weaponPrefab.Initialize(this.gameObject.transform);
        weapons.Add(weaponPrefab);
    }

    public void UpgradeWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            weapons[index].Upgrade();
        }
    }
}