using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public UnityEngine.UI.Slider slider;
    public Color low;
    public Color high;
    private Vector3 offset = new Vector3(0, 40, 0);
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position) + offset;
    }

    public void SetHealth(float health, float maxHealth)
    {
        Debug.Log("Slider GameObject Path: " + slider.gameObject.transform.GetHierarchyPath());
        Debug.Log("SetHealth called with health: " + health + " and maxHealth: " + maxHealth);
        slider.gameObject.SetActive(health < maxHealth);
        slider.maxValue = maxHealth;
        Debug.Log("Slider BEFORE setting value: maxValue = " + slider.maxValue);
        slider.value = health;
        Debug.Log("Slider value: " + slider.value + " and maxValue: " + slider.maxValue);
        slider.fillRect.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }
}


public static class TransformExtensions
{
    public static string GetHierarchyPath(this Transform current)
    {
        string path = current.name;
        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }
        return path;
    }
}


