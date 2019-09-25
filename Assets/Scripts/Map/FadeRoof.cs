using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class FadeRoof : MonoBehaviour
{
    //add gameobject roof in inspector
    public GameObject roof;

    private SpriteRenderer sprite;
    private Task oneShot = null;
    private LightingCollider2D lightCollider = null;

    private void Awake()
    {
        sprite = roof.GetComponent<SpriteRenderer>();
        lightCollider = GetComponent<LightingCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (oneShot == null)
            {
                oneShot = new Task(FadeTo(sprite, .1f, .1f));
            }
            else
            {
                oneShot.Stop();
                oneShot = new Task(FadeTo(sprite, .1f, .1f));
            }
        }

        if(lightCollider != null)
        {
            lightCollider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (oneShot == null)
            {
                oneShot = new Task(FadeTo(sprite, 1f, .1f));
            }
            else
            {
                oneShot.Stop();
                oneShot = new Task(FadeTo(sprite, 1f, .1f));
            }
        }

        if (lightCollider != null)
        {
            lightCollider.enabled = true;
        }
    }

    // Define an enumerator to perform our fading.
    // Pass it the material to fade, the opacity to fade to (0 = transparent, 1 = opaque),
    // and the number of seconds to fade over.
    IEnumerator FadeTo(SpriteRenderer sprite, float targetOpacity, float duration)
    {

        // Cache the current color of the material, and its initiql opacity.
        Color color = sprite.color;
        float startOpacity = color.a;

        // Track how many seconds we've been fading.
        float t = 0;

        while (t < duration)
        {
            // Step the fade forward one frame.
            t += Time.deltaTime;
            // Turn the time into an interpolation factor between 0 and 1.
            float blend = Mathf.Clamp01(t / duration);

            // Blend to the corresponding opacity between start & target.
            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            // Apply the resulting color to the material.
            sprite.color = color;

            // Wait one frame, and repeat.
            yield return null;
        }
    }
}

