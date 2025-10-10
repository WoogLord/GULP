using UnityEngine;
using TMPro;

public class UpdateUI : MonoBehaviour
{
    public TextMeshProUGUI gulpedMassText;
    public Player player;

    void Update()
    {
        UpdateLabel();
    }

    void UpdateLabel()
    {
        gulpedMassText.text = $"gulpedMass: {player.gulpedMass}";
    }
}