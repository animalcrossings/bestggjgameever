using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("1. Inventory Strip (Masks)")]
    [SerializeField] private Transform _maskContainer;    // Horizontal Layout Group (Bottom Right)
    [SerializeField] private GameObject _maskSlotPrefab;  // Prefab with InventorySlot component
    private List<InventorySlot> _spawnedMaskSlots = new List<InventorySlot>();

    [Header("2. Key Ring (Keys)")]
    [SerializeField] private Transform _keyContainer;     // Horizontal Layout Group (Next to Masks)
    [SerializeField] private GameObject _keyIconPrefab;   // Simple Image Prefab
    // We map Key ID -> UI Object so we can remove them if needed
    private Dictionary<int, GameObject> _spawnedKeyIcons = new Dictionary<int, GameObject>();

    [Header("3. HUD Labels")]
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private GameObject _hudPanel;
    [SerializeField] private Image _screenFader;

    [Header("4. Menus")]
    [SerializeField] private GameObject _levelCompletePanel;
    [SerializeField] private GameObject _gameOverPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Initialize Screens
        _levelCompletePanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        _hudPanel.SetActive(true);

        // Fade In
        StartCoroutine(FadeRoutine(0f, 1f));
    }

    // ========================================================================
    // SECTION A: MASK INVENTORY (The "CS:GO Strip")
    // ========================================================================

    /// <summary>
    /// Rebuilds the strip. Call this when you pickup a NEW Mask.
    /// </summary>
    public void RefreshMaskStrip(List<MaskData> allMasks, MaskData equippedMask)
    {
        // 1. Clear existing slots
        foreach (Transform child in _maskContainer) Destroy(child.gameObject);
        _spawnedMaskSlots.Clear();

        // 2. Sort or Group? (Optional: Sort by ID or Name so they don't jump around)
        // allMasks.Sort((a, b) => a.displayName.CompareTo(b.displayName));

        // 3. Spawn new slots
        foreach (MaskData mask in allMasks)
        {
            GameObject newObj = Instantiate(_maskSlotPrefab, _maskContainer);
            InventorySlot slot = newObj.GetComponent<InventorySlot>();
            
            if (slot != null)
            {
                slot.Setup(mask);
                _spawnedMaskSlots.Add(slot);
            }
        }

        // 4. Update the highlight to match current state
        HighlightMask(equippedMask);
    }

    /// <summary>
    /// Updates visuals only (Opacities/Borders). Call this when Swapping (Q/E).
    /// </summary>
    public void HighlightMask(MaskData equippedMask)
    {
        foreach (var slot in _spawnedMaskSlots)
        {
            bool isTarget = (slot.GetData() == equippedMask);
            slot.SetEquipped(isTarget);
        }
    }

    // ========================================================================
    // SECTION B: KEY RING (Passive Items)
    // ========================================================================

    public void AddKeyIcon(int keyId)
    {
        // Prevent duplicates in UI
        if (_spawnedKeyIcons.ContainsKey(keyId)) return;

        if (_keyContainer != null && _keyIconPrefab != null)
        {
            GameObject newIcon = Instantiate(_keyIconPrefab, _keyContainer);
            
            // Tint based on ID (Visual grouping by color)
            Image img = newIcon.GetComponent<Image>();
            if (img != null)
            {
                img.color = GetColorForKeyId(keyId);
            }

            _spawnedKeyIcons.Add(keyId, newIcon);
        }
    }

    public void ClearKeys()
    {
        foreach (Transform child in _keyContainer) Destroy(child.gameObject);
        _spawnedKeyIcons.Clear();
    }

    // ========================================================================
    // SECTION C: GAME FLOW UI
    // ========================================================================

    public void UpdateLevelName(string name)
    {
        if (_levelNameText != null) _levelNameText.text = name;
    }

    public void ShowLevelComplete()
    {
        _hudPanel.SetActive(false);
        _levelCompletePanel.SetActive(true);
    }
    
    public void ShowGameOver()
    {
        _hudPanel.SetActive(false);
        _gameOverPanel.SetActive(true);
    }

    // Linked to Button OnClick() events in Inspector
    public void OnNextLevelClicked()
    {
        _levelCompletePanel.SetActive(false);
        _hudPanel.SetActive(true);
        GameManager.Instance.CompleteLevel(); 
    }

    public void OnRetryClicked()
    {
        _gameOverPanel.SetActive(false);
        _hudPanel.SetActive(true);
        LevelManager.Instance.ReloadCurrentLevel();
    }

    // ========================================================================
    // HELPERS
    // ========================================================================

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        if (_screenFader == null) yield break;

        _screenFader.raycastTarget = true;
        float start = _screenFader.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newA = Mathf.Lerp(start, targetAlpha, elapsed / duration);
            _screenFader.color = new Color(0, 0, 0, newA);
            yield return null;
        }

        _screenFader.color = new Color(0, 0, 0, targetAlpha);
        _screenFader.raycastTarget = targetAlpha > 0.1f;
    }

    private Color GetColorForKeyId(int id)
    {
        // Consistent colors for specific IDs
        switch (id % 5)
        {
            case 0: return new Color(1f, 0.4f, 0.4f); // Red
            case 1: return new Color(0.4f, 0.4f, 1f); // Blue
            case 2: return new Color(0.4f, 1f, 0.4f); // Green
            case 3: return new Color(1f, 1f, 0.4f);   // Yellow
            case 4: return new Color(1f, 0.4f, 1f);   // Purple
            default: return Color.white;
        }
    }
}