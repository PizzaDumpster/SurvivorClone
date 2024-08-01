using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class WeaponSelectionUI : MonoBehaviour
{
    public GameObject selectionPanel;
    public Button weaponButtonPrefab;
    public Transform buttonContainer;

    private PlayerController playerController;
    public List<WeaponBase> availableWeapons = new List<WeaponBase>();

    private float buttonSpacing = 200f; // Adjust this value to change the space between buttons
    private float currentYPosition = 0f;
    private Vector2 buttonSize = new Vector2(200f, 50f);

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene. Please ensure it exists.");
        }

        if (selectionPanel == null)
        {
            Debug.LogError("Selection panel is not assigned in the inspector. Please assign it.");
        }

        if (weaponButtonPrefab == null)
        {
            Debug.LogError("Weapon button prefab is not assigned in the inspector. Please assign it.");
        }

        if (buttonContainer == null)
        {
            Debug.LogError("Button container is not assigned in the inspector. Please assign it.");
        }
        
        SetupVerticalLayoutGroup();

        selectionPanel?.SetActive(false);

        LoadAvailableWeapons();
    }

    private void LoadAvailableWeapons()
    {
        WeaponBase[] loadedWeapons = Resources.LoadAll<WeaponBase>("Weapons");
        if (loadedWeapons.Length == 0)
        {
            Debug.LogWarning("No weapons found in the Resources/Weapons folder. Please ensure weapons are placed there.");
        }
        availableWeapons.AddRange(loadedWeapons);
    }

    public void ShowWeaponSelection()
    {
        if (playerController == null || selectionPanel == null || weaponButtonPrefab == null || buttonContainer == null)
        {
            Debug.LogError("Some required components are missing. Please check the console for previous error messages.");
            return;
        }
        Time.timeScale = 0; // Pause the game
        selectionPanel.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

       

        List<WeaponOption> upgradeOptions = new List<WeaponOption>();
        List<WeaponOption> newWeaponOptions = new List<WeaponOption>();

        // Add upgrade options for existing weapons
        if (playerController.weapons != null)
        {
            for (int i = 0; i < playerController.weapons.Count; i++)
            {
                WeaponBase weapon = playerController.weapons[i];
                if (weapon != null)
                {
                    int index = i;
                    upgradeOptions.Add(new WeaponOption($"Upgrade {weapon.weaponName}", () => UpgradeWeapon(index)));
                }
            }
        }
        // Add experience gem upgrade option
        upgradeOptions.Add(new WeaponOption("Upgrade Experience Gems", UpgradeExperienceGems));
        // Add new weapon options
        foreach (WeaponBase weapon in availableWeapons.Where(w => !playerController.weapons.Any(pw => pw != null && pw.GetType() == w.GetType())))
        {
            newWeaponOptions.Add(new WeaponOption($"New: {weapon.weaponName}", () => SelectWeapon(weapon)));
        }

        // Combine and shuffle options
        List<WeaponOption> allOptions = upgradeOptions.Concat(newWeaponOptions).ToList();
        allOptions = allOptions.OrderBy(x => Random.value).ToList();
         // Reset the current Y position
                currentYPosition = 0f;
        // Create buttons for options (limit to 3 options)
        int optionsToShow = Mathf.Min(allOptions.Count, 3);
        for (int i = 0; i < optionsToShow; i++)
        {
            WeaponOption option = allOptions[i];
            CreateWeaponButton(option.Name, option.Action);
        }
    }

    void CreateWeaponButton(string buttonText, UnityEngine.Events.UnityAction onClick)
    {
        Button weaponButton = Instantiate(weaponButtonPrefab, buttonContainer);
        // Set the button's position
        RectTransform rectTransform = weaponButton.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
        rectTransform.anchoredPosition = new Vector2(0, currentYPosition);
        rectTransform.sizeDelta = buttonSize; // Set the size of the button

        // Update the Y position for the next button
        currentYPosition -= (buttonSize.y + buttonSpacing);

        TMP_Text buttonTextComponent = weaponButton.GetComponentInChildren<TMP_Text>();
        if (buttonTextComponent != null)
        {
            buttonTextComponent.text = buttonText;
        }
        else
        {
            Debug.LogWarning("TMP_Text component not found on button. Falling back to legacy Text component.");
            Text legacyTextComponent = weaponButton.GetComponentInChildren<Text>();
            if (legacyTextComponent != null)
            {
                legacyTextComponent.text = buttonText;
            }
        }
        weaponButton.onClick.AddListener(onClick);
    }

    void SelectWeapon(WeaponBase weaponPrefab)
    {
        WeaponBase newWeapon = Instantiate(weaponPrefab, playerController.transform);
        playerController.AddWeapon(newWeapon);
        newWeapon.transform.SetParent(playerController.gameObject.transform);
        CloseSelectionPanel();
    }
    void UpgradeExperienceGems()
    {
        ExperienceGem.UpgradeExperienceValue();
        CloseSelectionPanel();
    }
    void UpgradeWeapon(int index)
    {
        playerController.UpgradeWeapon(index);
        CloseSelectionPanel();
    }

    void CloseSelectionPanel()
    {
        selectionPanel.SetActive(false);
        Time.timeScale = 1; // Resume the game
    }

    private class WeaponOption
    {
        public string Name { get; set; }
        public UnityEngine.Events.UnityAction Action { get; set; }

        public WeaponOption(string name, UnityEngine.Events.UnityAction action)
        {
            Name = name;
            Action = action;
        }
    }

    void SetupVerticalLayoutGroup()
    {
        VerticalLayoutGroup layoutGroup = buttonContainer.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup == null)
        {
            layoutGroup = buttonContainer.gameObject.AddComponent<VerticalLayoutGroup>();
        }
        layoutGroup.childAlignment = TextAnchor.UpperCenter;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childControlWidth = false;  // Allow children to keep their own width
        layoutGroup.childControlHeight = false; // Allow children to keep their own height
        layoutGroup.spacing = 10; // Adjust this value to change spacing between buttons
    }
}