using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public Animator animator;

    public IEnumerator Move(Vector3 start, Vector3 end, float speed, Action reached = null) {
        animator.SetBool("Walk", speed <= 1.5f);
        animator.SetBool("Run", speed > 1.5f);

        float duration = Vector3.Distance(start, end) / speed;
        transform.position = start;
        transform.rotation = Quaternion.LookRotation(end - start, Vector3.up);

        for (float t = 0f; t < 1f; t += Time.deltaTime / duration) {
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        ReachedDestination();
        reached?.Invoke();
    }

    private void ReachedDestination() {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
    }
}
