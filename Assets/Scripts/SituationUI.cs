using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SituationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtSituationText;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private GameObject optionButtonPrefab;

    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private Button btnRetry;

    [SerializeField] private GameObject panelEnding;
    [SerializeField] private TextMeshProUGUI txtEndingTitle;
    [SerializeField] private TextMeshProUGUI txtEndingMessage;
    [SerializeField] private Button btnRestart;

    public void DisplaySituation(SituationData data, System.Action<SituationOption> onSelect)
    {
        txtSituationText.text = LocalizationManager.GetText(data.descriptionES, data.descriptionEN);
        ClearOptions();
        foreach (var opt in data.options)
        {
            var captured = opt;
            var go = Instantiate(optionButtonPrefab, optionsContainer);
            go.GetComponentInChildren<TextMeshProUGUI>().text =
                LocalizationManager.GetText(opt.labelES, opt.labelEN);
            go.GetComponent<Button>().onClick.AddListener(() => onSelect(captured));
        }
    }

    public void DisplayGameOver(System.Action onRetry)
    {
        panelGameOver.SetActive(true);
        btnRetry.onClick.RemoveAllListeners();
        btnRetry.onClick.AddListener(() =>
        {
            panelGameOver.SetActive(false);
            onRetry();
        });
    }

    public void HideGameOver() => panelGameOver.SetActive(false);

    public void DisplayEnding(EndingType type, Player player, System.Action onRestart)
    {
        panelEnding.SetActive(true);

        switch (type)
        {
            case EndingType.Good:
                txtEndingTitle.text = LocalizationManager.GetText("Final Bueno", "Good Ending");
                txtEndingMessage.text = LocalizationManager.GetText(
                    $"El oráculo te reconoce, {player.PlayerName}. Compartes su sabiduría con el mundo.",
                    $"The oracle recognizes you, {player.PlayerName}. You share its wisdom with the world.");
                break;
            case EndingType.Neutral:
                txtEndingTitle.text = LocalizationManager.GetText("Final Neutral", "Neutral Ending");
                txtEndingMessage.text = LocalizationManager.GetText(
                    $"El oráculo guarda silencio, {player.PlayerName}. Regresas con las manos vacías.",
                    $"The oracle stays silent, {player.PlayerName}. You return empty-handed.");
                break;
            case EndingType.Bad:
                txtEndingTitle.text = LocalizationManager.GetText("Final Malo", "Bad Ending");
                txtEndingMessage.text = LocalizationManager.GetText(
                    $"El oráculo te maldice, {player.PlayerName}. Las ruinas colapsan a tu alrededor.",
                    $"The oracle curses you, {player.PlayerName}. The ruins collapse around you.");
                break;
        }

        btnRestart.onClick.RemoveAllListeners();
        btnRestart.onClick.AddListener(() => onRestart());
    }

    public void HideEnding() => panelEnding.SetActive(false);

    private void ClearOptions()
    {
        foreach (Transform child in optionsContainer) Destroy(child.gameObject);
    }
}