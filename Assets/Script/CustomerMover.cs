using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _customerImage;

    [Header("Movement Settings")]
    [SerializeField] private RectTransform _offScreenRight;
    [SerializeField] private RectTransform _onScreenPoint;
    [SerializeField] private float _moveSpeed = 300f;
    [SerializeField] private float _fadeSpeed = 2f;

    public bool IsMoving { get; private set; }

    private RectTransform _customerRect;

    private void Awake()
    {
        _customerRect = _customerImage.GetComponent<RectTransform>();
    }

    public IEnumerator MoveIn()
    {
        Debug.Log("Customer MoveIn start");

        _customerRect.anchoredPosition = _offScreenRight.anchoredPosition;
        Color c = _customerImage.color;
        c.a = 0f;
        _customerImage.color = c;

        yield return StartCoroutine(MoveAndFade(_onScreenPoint.anchoredPosition, 1f));

        Debug.Log("Customer MoveIn end");
    }

    public IEnumerator MoveOut()
    {
        yield return StartCoroutine(MoveAndFade(_offScreenRight.anchoredPosition, 0f));
    }

    private IEnumerator MoveAndFade(Vector2 targetPos, float targetAlpha)
    {
        float timer = 0f;
        const float timeout = 5f;

        Color c = _customerImage.color;

        Debug.Log($"[MoveAndFade] start: from={_customerRect.anchoredPosition}, target={targetPos}, alpha={c.a}->{targetAlpha}");

        while ((Vector2.Distance(_customerRect.anchoredPosition, targetPos) > 1f ||
                Mathf.Abs(c.a - targetAlpha) > 0.01f) && timer < timeout)
        {
            // Move
            _customerRect.anchoredPosition = Vector2.MoveTowards(
                _customerRect.anchoredPosition,
                targetPos,
                _moveSpeed * Time.deltaTime);

            // Fade
            c.a = Mathf.MoveTowards(c.a, targetAlpha, _fadeSpeed * Time.deltaTime);
            _customerImage.color = c;

            timer += Time.deltaTime;
            yield return null;
        }

        if (timer >= timeout)
            Debug.LogWarning($"[MoveAndFade Timeout] TargetPos={targetPos}, TargetAlpha={targetAlpha}");
        else
            Debug.Log("[MoveAndFade] Done");
    }
}
