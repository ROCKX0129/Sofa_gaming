using UnityEngine;
using System.Collections;

public class GhostOrb : MonoBehaviour, IItem
{
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    [Header("Ghost Settings")]
    public float duration = 3f;
    public LayerMask playerLayer;

    public void Use(GameObject player)
    {
        player.GetComponent<MonoBehaviour>().StartCoroutine(GhostMode(player));
        Destroy(gameObject);
    }

    private IEnumerator GhostMode(GameObject player)
    {
        int playerLayerIndex = player.layer;

        // Get all SpriteRenderers in the player, including children
        SpriteRenderer[] renderers = player.GetComponentsInChildren<SpriteRenderer>();

        // Store original colors to restore later
        Color[] originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].color;

        Debug.Log("Ghost mode started");

        // Make all sprites semi-transparent
        foreach (SpriteRenderer sr in renderers)
        {
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 0.4f); // 40% visible
        }

        // Disable collisions between this player and all players on the same layer
        Physics2D.IgnoreLayerCollision(playerLayerIndex, playerLayerIndex, true);

        // Wait for ghost duration
        yield return new WaitForSeconds(duration);

        // Restore collisions
        Physics2D.IgnoreLayerCollision(playerLayerIndex, playerLayerIndex, false);

        // Restore original colors
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].color = originalColors[i];

        Debug.Log("Ghost mode ended");
    }
}
