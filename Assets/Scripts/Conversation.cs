using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public Action beforeFirstDialogue;
    public Action<int> beforeEachDialogue;
    public Action<int> afterEachDialogue;
    public Action afterAllDialogues;

    [SerializeField] private List<Dialogue> dialogues;
    private DialogueUI _ownDialogueUI;
    [HideInInspector] public bool shouldStop;

    private void Awake()
    {
        _ownDialogueUI = GetComponentInChildren<DialogueUI>();
        shouldStop = false;
    }

    public IEnumerator StartConversation() {
        beforeFirstDialogue?.Invoke();

        foreach (Dialogue dialogue in dialogues) {
            yield return new WaitUntil(() => !shouldStop);

            DialogueUI ui = dialogue.Speaker.GetComponentInChildren<DialogueUI>();
            if (ui == null) {
                ui = Camera.main.GetComponentInChildren<DialogueUI>();
            }
            bool isSelf = dialogue.Speaker.transform == transform;

            // Handle Audio
            dialogue.Speaker.clip = dialogue.Clip;
            if (dialogue.Clip != null) dialogue.Speaker?.Play();

            // Handle Animation
            dialogue.Speaker.GetComponent<Character>().animator.SetBool("Talking", true);

            // Handle UI
            ui.SetText(string.IsNullOrEmpty(dialogue.Name) ? dialogue.Speaker.name : dialogue.Name, dialogue.Sentence);
            if (!string.IsNullOrEmpty(dialogue.Sentence)) ui.FadeCanvas(0f, 1f, 0.3f);

            beforeEachDialogue?.Invoke(dialogues.IndexOf(dialogue));

            if (dialogue.Clip) {
                yield return new WaitUntil(() => !dialogue.Speaker.isPlaying);
            } else {
                yield return new WaitForSeconds(3);
            }

            // Handle Animation
            dialogue.Speaker.GetComponent<Character>().animator.SetBool("Talking", false);

            // Handle UI
            if (!string.IsNullOrEmpty(dialogue.Sentence)) ui.FadeCanvas(1f, 0f, 0.3f);

            afterEachDialogue?.Invoke(dialogues.IndexOf(dialogue));
            dialogue.Callback?.Invoke();

            yield return new WaitForSeconds(1);
        }

        afterAllDialogues?.Invoke();
    }
}
