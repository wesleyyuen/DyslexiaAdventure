using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waitress : InteractablePedestrian
{
    [SerializeField] private RectTransform _receipt;
    [SerializeField] private Text _item1Price;
    [SerializeField] private Text _item2Price;
    [SerializeField] private RectTransform _money;
    [SerializeField] private RectTransform _change;
    private InteractablePhone _phone;
    private Animator _playerAnimator;
    private Vector3 _originalReceiptPos, _originalMoneyPos, _originalChangePos;

    private void Start()
    {
        _playerAnimator = Camera.main.GetComponentInChildren<Animator>();
        _phone = Camera.main.GetComponentInChildren<InteractablePhone>();

        _originalReceiptPos = _receipt.anchoredPosition;
        _originalMoneyPos = _money.anchoredPosition;
        _originalChangePos = _change.anchoredPosition;
    }

    public void DyslexiaMoment()
    {
        StartCoroutine(_DyslexiaMoment());
    }

    private IEnumerator _DyslexiaMoment()
    {
        _conversation.shouldStop = true;
        
        // Show Receipt
        Vector3 newReceiptPos = new Vector3(0f, -35f, 0f);
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.75f) {
            _receipt.anchoredPosition = Vector3.Lerp(_originalReceiptPos, newReceiptPos, t);
            yield return null;
        }
        _receipt.anchoredPosition = newReceiptPos;

        // Show Money
        Vector3 newMoneyPos = new Vector3(0f, 185f, 0f);
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.75f) {
            _money.anchoredPosition = Vector3.Lerp(_originalMoneyPos, newMoneyPos, t);
            yield return null;
        }
        _money.anchoredPosition = newMoneyPos;

        // Fade out price
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.25f) {
            _item1Price.color = new Color(_item1Price.color.r, _item1Price.color.g, _item1Price.color.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }
        _item1Price.color = new Color(1f, 1f, 1f, 0f);

        // Set dyslexic price
        _item1Price.text = "$95";

        // Fade in price
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.25f) {
            _item1Price.color = new Color(_item1Price.color.r, _item1Price.color.g, _item1Price.color.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }
        _item1Price.color = Color.black;

        yield return new WaitForSeconds(1f);

        // Fade out price
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.25f) {
            _item1Price.color = new Color(_item1Price.color.r, _item1Price.color.g, _item1Price.color.b, Mathf.Lerp(1f, 0f, t));
            _item2Price.color = new Color(_item2Price.color.r, _item2Price.color.g, _item2Price.color.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }
        _item1Price.color = new Color(1f, 1f, 1f, 0f);
        _item2Price.color = new Color(1f, 1f, 1f, 0f);

        // Set dyslexic price
        _item1Price.text = "$9+5";
        _item2Price.text = "$4+8";

        // Fade in price
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.25f) {
            _item1Price.color = new Color(_item1Price.color.r, _item1Price.color.g, _item1Price.color.b, Mathf.Lerp(0f, 1f, t));
            _item2Price.color = new Color(_item2Price.color.r, _item2Price.color.g, _item2Price.color.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }
        _item1Price.color = Color.black;
        _item2Price.color = Color.black;

        yield return new WaitForSeconds(1f);

        _conversation.shouldStop = false;
    }

    public void CalculateTotal()
    {
        StartCoroutine(_CalculateTotal());
    }

    private IEnumerator _CalculateTotal()
    {
        _conversation.shouldStop = true;

        // Fade out price
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.25f) {
            _item1Price.color = new Color(_item1Price.color.r, _item1Price.color.g, _item1Price.color.b, Mathf.Lerp(1f, 0f, t));
            _item2Price.color = new Color(_item2Price.color.r, _item2Price.color.g, _item2Price.color.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }
        _item1Price.color = new Color(1f, 1f, 1f, 0f);
        _item2Price.color = new Color(1f, 1f, 1f, 0f);

        // Set dyslexic price
        _item1Price.text = "$65";
        _item2Price.text = "$48";

        // Fade in price
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.25f) {
            _item1Price.color = new Color(_item1Price.color.r, _item1Price.color.g, _item1Price.color.b, Mathf.Lerp(0f, 1f, t));
            _item2Price.color = new Color(_item2Price.color.r, _item2Price.color.g, _item2Price.color.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }
        _item1Price.color = Color.black;
        _item2Price.color = Color.black;

        // Calculating
        _phone.RaisePhone((int)InteractablePhone.OpenedApp.Calculator);
        yield return new WaitForSeconds(1f);
        _playerAnimator.SetTrigger("StartPressing");
        yield return new WaitForSeconds(1f);
        _phone.SetCalculatorText("48");
        _playerAnimator.SetTrigger("Pressing");
        yield return new WaitForSeconds(1f);
        _phone.SetCalculatorText("113");
        _playerAnimator.SetTrigger("EndPressing");

        _conversation.shouldStop = false;
    }

    public void Change() {
        StartCoroutine(_Change());
    }

    private IEnumerator _Change() {
        _phone.HidePhone();

        // Hide Receipt and money
        Vector3 newReceiptPos = _receipt.anchoredPosition;
        Vector3 newMoneyPos = _money.anchoredPosition;
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.75f) {
            _receipt.anchoredPosition = Vector3.Lerp(newReceiptPos, _originalReceiptPos, t);
            _money.anchoredPosition = Vector3.Lerp(newMoneyPos, _originalMoneyPos, t);
            yield return null;
        }
        _receipt.anchoredPosition = _originalReceiptPos;
        _money.anchoredPosition = _originalMoneyPos;

        // Show Change
        Vector3 newChangePos = new Vector3(0f, 185f, 0f);
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.75f) {
            _change.anchoredPosition = Vector3.Lerp(_originalChangePos, newChangePos, t);
            yield return null;
        }
        _change.anchoredPosition = newChangePos;

        yield return new WaitForSeconds(3f);

        // Hide Change
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.75f) {
            _change.anchoredPosition = Vector3.Lerp(newChangePos, _originalChangePos, t);
            yield return null;
        }
        _change.anchoredPosition = _originalChangePos;
    }
}
