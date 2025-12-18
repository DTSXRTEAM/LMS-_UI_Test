using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UISearchFilter : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField searchInput;
    public List<GameObject> items;   // Parent UI objects (buttons, panels, etc.)

    void Start()
    {
        searchInput.onValueChanged.AddListener(FilterItems);
    }

    void FilterItems(string searchText)
    {
        searchText = searchText.ToLower();

        foreach (GameObject item in items)
        {
            // Find the TextMeshPro text inside this item
            TextMeshProUGUI itemText = item.GetComponentInChildren<TextMeshProUGUI>();

            if (itemText == null)
            {
                item.SetActive(false);
                continue;
            }

            // Check if the text matches the search
            bool isMatch = itemText.text.ToLower().Contains(searchText);

            // Show or hide item
            item.SetActive(isMatch);
        }
    }
}
