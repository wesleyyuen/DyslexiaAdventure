using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationSequence : MonoBehaviour
{
    [SerializeField] private List<Conversation> _conversations;
    private int _currIndex = 0;

    private void Awake()
    {
        _currIndex = 0; 
    }

    public void StartSequence()
    {
        // Begin first interactions
        if (_conversations[0] != null && _conversations[0].TryGetComponent<IInteractable>(out IInteractable i)) {
            InputManager.Instance.EnablePlayerInput(false);
            i.Begin();
            _conversations[0].afterAllDialogues += NextNPCAfterDelay;
        }
    }

    private void NextNPCAfterDelay()
    {
        StartCoroutine(_NextNPC(1f));
    }
    
    private IEnumerator _NextNPC(float delay)
    {
        yield return new WaitForSeconds(delay);

        _currIndex++;

        // No more interaction, giving control back to player
        if (_currIndex >= _conversations.Count) {
            InputManager.Instance.EnablePlayerInput(true);
        } else {
            if (_conversations[_currIndex] != null && _conversations[_currIndex].TryGetComponent<IInteractable>(out IInteractable i)) {
                i.Begin();
                _conversations[_currIndex].afterAllDialogues += NextNPCAfterDelay;
            }
        }
    }
}
