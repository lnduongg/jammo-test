using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float flightDuration = 1.2f;
    public float arcHeight = 3.5f;
    public GameObject confettiExplosion;
    private bool isFlying = false;

    public void ShootTo(Vector3 targetPosition, Action onGoalReached)
    {
        if (isFlying) return;
        StartCoroutine(FlyRoutine(targetPosition, onGoalReached));
    }

    private IEnumerator FlyRoutine(Vector3 targetPosition, Action onGoalReached)
    {
        isFlying = true;
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < flightDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flightDuration;
            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * arcHeight;
            transform.Rotate(Vector3.right * 720f * Time.deltaTime);
            transform.position = currentPos;

            yield return null;
        }
        transform.position = targetPosition;
        isFlying = false;

        if (confettiExplosion != null)
        {
            GameObject fx = Instantiate(confettiExplosion, targetPosition + Vector3.up * 2f, Quaternion.identity);

            ParticleSystem[] allParticles = fx.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in allParticles)
            {
                ps.Clear();
                ps.Play();
            }

            Destroy(fx, 4f);
        }

        onGoalReached?.Invoke();
    }

    public bool IsFlying()
    {
        return isFlying;
    }
}
    