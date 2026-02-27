using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour, IItem
{
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    [Header("Speed Settings")]
    public float speedMultiplier = 2f;
    public float duration = 3f;

    public void Use(GameObject player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.StartCoroutine(ApplySpeed(controller));
        }

        Destroy(gameObject);
    }

    private IEnumerator ApplySpeed(PlayerController controller)
    {
        float originalSpeed = controller.moveSpeed;
        controller.moveSpeed *= speedMultiplier; // apply boost

        // Visual effect: make player glow yellow
        SpriteRenderer[] renderers = controller.GetComponentsInChildren<SpriteRenderer>();
        Color[] originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].color;

        // Apply glow/tint
        foreach (SpriteRenderer sr in renderers)
        {
            Color c = sr.color;
            sr.color = new Color(1f, 1f, 0f, c.a); // yellow tint, keep alpha
        }

        yield return new WaitForSeconds(duration);

        // Reset speed
        controller.moveSpeed = originalSpeed;

        // Reset colors
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].color = originalColors[i];
    }    
}
