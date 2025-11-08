using TMPro;
using UnityEngine;

public class TypingSpellController : MonoBehaviour
{
    [Header("UI References")] 
    [SerializeField] private CanvasGroup _typingPanel; 
    [SerializeField] private TextMeshProUGUI _targetText; 

    [Header("Gameplay References")] 
    [SerializeField] private SpelledChoiceHandler _ingredientInteractionHandler;

    [Header("Mantra Typing Config")]
    [Tooltip("Jika true, 'A' dianggap sama dengan 'a'")]
    [SerializeField] private bool _ignoreCase = false; 


    private string _targetMantra;
    private int _currentIndex;
    private bool _active;
    private bool _failed;

    private void Awake()
    {
        SetTypingPanelVisible(false);
    }

    void Start()
    {
        GameManager.Instance.OnAllIngredientsChosen.AddListener(BeginTypingPhase);
        GameManager.Instance.OnTimeout.AddListener(EndPhase);
    }

    private void BeginTypingPhase()
    {
        var order = _ingredientInteractionHandler.CurrentOrder  != null ? _ingredientInteractionHandler.CurrentOrder : null;
        if (order == null)
        {
            Debug.LogWarning("TypingSpellController: Order null, fase typing dibatalkan.");
            return;
        }

        Debug.Log("Begin Typing Spell Phase");

        _targetMantra = order.mantra ?? string.Empty;
        _currentIndex = 0;
        _active = true;
        _failed = false;
        RenderProgress();
        SetTypingPanelVisible(true);

        // Jika mantra kosong, auto sukses
        if (_targetMantra.Length == 0)
        {
            HandleSuccess();
        }
    }

    private void Update()
    {
        if (!_active || _failed) return;

        // Input.inputString dapat berisi beberapa karakter sekaligus
        string inputChars = Input.inputString;
        if (string.IsNullOrEmpty(inputChars)) return;

        foreach (char rawChar in inputChars)
        {
            // Abaikan newline / carriage return
            if (rawChar == '\n' || rawChar == '\r') continue;

            if (_currentIndex >= _targetMantra.Length)
            {
                // Sudah selesai sebelumnya
                return;
            }

            char expected = _targetMantra[_currentIndex];
            char typed = rawChar;

            bool match = _ignoreCase
                ? char.ToLowerInvariant(expected) == char.ToLowerInvariant(typed)
                : expected == typed;

            if (!match)
            {
                HandleFail(typed);
                break; // selesai karena gagal
            }

            _currentIndex++;
            RenderProgress();

            if (_currentIndex >= _targetMantra.Length)
            {
                HandleSuccess();
                break;
            }
        }
    }

    private void HandleFail(char wrongChar)
    {
        _failed = true;
        _active = false;
        RenderFail(wrongChar);
        GameManager.Instance.OnTypingFail?.Invoke();
        EndPhase();
    }

    private void HandleSuccess()
    {
        _active = false;
        GameManager.Instance.OnTypingSuccess?.Invoke();
        EndPhase();
    }

    public void EndPhase()
    {
        // Panel bisa disembunyikan langsung atau menunggu animasi (tambahkan coroutine jika perlu)
        SetTypingPanelVisible(false);
    }

    private void RenderProgress()
    {
        // Bangun teks dengan bagian yang sudah benar warna hijau
        if (_targetText == null) return;

        string correctPart = _currentIndex > 0 ? _targetMantra.Substring(0, _currentIndex) : string.Empty;
        string remainingPart = _currentIndex < _targetMantra.Length ? _targetMantra.Substring(_currentIndex) : string.Empty;
        _targetText.richText = true;
        _targetText.text = $"<color=#32C832>{EscapeRichText(correctPart)}</color>{EscapeRichText(remainingPart)}";
    }

    private void RenderFail(char wrongChar)
    {
        if (_targetText == null) return;
        _targetText.richText = true;
        // Tampilkan informasi posisi gagal
        string msg = $"Gagal di karakter '{wrongChar}' posisi {_currentIndex + 1}";
        string full = EscapeRichText(_targetMantra);
        _targetText.text = $"<color=red>{EscapeRichText(msg)}</color>\n<color=#888888>{full}</color>";
    }

    private void SetTypingPanelVisible(bool visible)
    {
        if (_typingPanel == null) return;
        _typingPanel.alpha = visible ? 1f : 0f;
        _typingPanel.blocksRaycasts = visible;
        _typingPanel.interactable = visible;
    }

    private string EscapeRichText(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        // TMP tidak butuh escape untuk tanda khusus kecuali < dan >
        return input.Replace("<", "<").Replace(">", ">");
    }
}
