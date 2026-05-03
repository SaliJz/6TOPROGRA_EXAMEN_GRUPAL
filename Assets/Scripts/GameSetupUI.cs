using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSetupUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputPlayerName;
    [SerializeField] private Button btnStart;
    [SerializeField] private SituationManager situationManager;

    private void Awake()
    {
        btnStart.onClick.AddListener(OnStartGame);
    }

    private void OnStartGame()
    {
        string name = inputPlayerName.text.Trim();
        if (string.IsNullOrEmpty(name)) name = "Aventurero";
        situationManager.InitPlayer(name, maxHealth: 100, damage: 10);
        gameObject.SetActive(false);
    }
}