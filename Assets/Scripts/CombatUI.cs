using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Joaquin.Manager;

/// <summary>
/// Controla todos los elementos visuales de la pantalla de combate.
/// </summary>
public class CombatUI : MonoBehaviour
{
    // Panel jugador
    [Header("Panel Jugador")]
    [SerializeField] private TextMeshProUGUI txtPlayerName;
    [SerializeField] private TextMeshProUGUI txtPlayerHP;
    [SerializeField] private TextMeshProUGUI txtPlayerDamage;
    [SerializeField] private Slider sliderPlayerHP;

    // Panel enemigo
    [Header("Panel Enemigo")]
    [SerializeField] private TextMeshProUGUI txtEnemyName;
    [SerializeField] private TextMeshProUGUI txtEnemyHP;
    [SerializeField] private Slider sliderEnemyHP;

    // Log de combate
    [Header("Log de Combate")]
    [SerializeField] private TextMeshProUGUI txtCombatLog;
    [SerializeField] private ScrollRect scrollLog;

    // Botones de accion
    [Header("Botones de Accion")]
    [SerializeField] private Button btnAttack;
    [SerializeField] private Button btnUseItem;
    [SerializeField] private Button btnFlee;

    // Panel de seleccion de items
    [Header("Panel de Items")]
    [SerializeField] private GameObject panelItems;
    [SerializeField] private Transform itemListContainer;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Button btnCloseItems;

    // Panel de resultado
    [Header("Panel de Resultado")]
    [SerializeField] private GameObject panelResult;
    [SerializeField] private TextMeshProUGUI txtResultTitle;
    [SerializeField] private TextMeshProUGUI txtResultBody;
    [SerializeField] private Button btnResultContinue;

    // Estado interno
    private string fullLog = string.Empty;

    // Eventos
    public event System.Action OnAttackPressed;
    public event System.Action OnFleePressed;
    public event System.Action<Item> OnItemSelected;
    public event System.Action OnResultContinuePressed;

    private void Awake()
    {
        btnAttack.onClick.AddListener(() => OnAttackPressed?.Invoke());
        btnFlee.onClick.AddListener(() => OnFleePressed?.Invoke());
        btnUseItem.onClick.AddListener(ShowItemPanel);
        btnCloseItems.onClick.AddListener(HideItemPanel);
        btnResultContinue.onClick.AddListener(() => OnResultContinuePressed?.Invoke());

        panelItems.SetActive(false);
        panelResult.SetActive(false);
    }

    /// <summary>
    /// CombatController llama este metodo cada vez que el estado cambia.
    /// </summary>
    public void Refresh(CombatUIData data)
    {
        // Jugador
        txtPlayerName.text = data.PlayerName;
        txtPlayerHP.text = $"HP: {data.PlayerCurrentHP} / {data.PlayerMaxHP}";
        txtPlayerDamage.text = $"Dano: {data.PlayerDamage}";
        sliderPlayerHP.maxValue = data.PlayerMaxHP;
        sliderPlayerHP.value = data.PlayerCurrentHP;

        // Enemigo
        txtEnemyName.text = data.EnemyName;
        txtEnemyHP.text = $"HP: {data.EnemyCurrentHP} / {data.EnemyMaxHP}";
        sliderEnemyHP.maxValue = data.EnemyMaxHP;
        sliderEnemyHP.value = data.EnemyCurrentHP;

        // Log
        if (!string.IsNullOrEmpty(data.LastLogLine))
        {
            AppendLog(data.LastLogLine);
        }

        // Botones activos solo si el combate sigue
        SetActionButtonsInteractable(data.CombatActive);
    }

    public void AppendLog(string line)
    {
        fullLog += $"\n> {line}";
        txtCombatLog.text = fullLog;
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollLog.verticalNormalizedPosition = 0f;
    }

    public void ClearLog()
    {
        fullLog = string.Empty;
        txtCombatLog.text = string.Empty;
    }

    public void PopulateItemPanel(List<Item> usableItems)
    {
        // Limpia botones anteriores
        foreach (Transform child in itemListContainer)
        {
            Destroy(child.gameObject);
        }

        if (usableItems == null || usableItems.Count == 0)
        {
            AppendLog("No tienes items usables.");
            return;
        }

        foreach (Item item in usableItems)
        {
            // item capturado por valor para el closure
            Item captured = item;

            GameObject go = Instantiate(itemButtonPrefab, itemListContainer);
            go.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{item.Name}\n<size=70%>{item.Description}</size>";

            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnItemSelected?.Invoke(captured);
                HideItemPanel();
            });
        }
    }

    public void ShowItemPanel() => panelItems.SetActive(true);
    public void HideItemPanel() => panelItems.SetActive(false);

    /// <summary>
    /// Muestra el resultado del combate con titulo y cuerpo descriptivo.
    /// </summary>
    public void ShowResult(string title, string body, string continueLabel = "Continuar")
    {
        SetActionButtonsInteractable(false);
        panelResult.SetActive(true);
        txtResultTitle.text = title;
        txtResultBody.text = body;
        btnResultContinue.GetComponentInChildren<TextMeshProUGUI>().text = continueLabel;
    }

    public void HideResult() => panelResult.SetActive(false);

    private void SetActionButtonsInteractable(bool value)
    {
        btnAttack.interactable = value;
        btnUseItem.interactable = value;
        btnFlee.interactable = value;
    }
}