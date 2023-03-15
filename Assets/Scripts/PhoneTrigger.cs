using System.Collections;
using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _wrongMap;
    [SerializeField] private InteractablePhone _phone;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("MainCamera")) {
            StartCoroutine(BlurMap());
        }
    }

    private IEnumerator BlurMap()
    {
        yield return new WaitForSeconds(0.5f);

        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.5f) {
            _wrongMap.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);

        _phone.RaisePhone((int)InteractablePhone.OpenedApp.Map);
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("MainCamera")) {
            _phone.HidePhone();
            StopAllCoroutines();
            _wrongMap.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
