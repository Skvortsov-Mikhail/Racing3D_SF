using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioClip click;

    private AudioSource audioSource;

    private UIButton[] uIButtons;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        uIButtons = GetComponentsInChildren<UIButton>(true);

        for (int i = 0; i < uIButtons.Length; i++)
        {
            uIButtons[i].PointerEnter += OnPointerEnter;
            uIButtons[i].PointerClick += OnPointerClick;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < uIButtons.Length; i++)
        {
            uIButtons[i].PointerEnter -= OnPointerEnter;
            uIButtons[i].PointerClick -= OnPointerClick;
        }
    }

    private void OnPointerEnter(UIButton button)
    {
        audioSource.PlayOneShot(hover);
    }

    private void OnPointerClick(UIButton button)
    {
        audioSource.PlayOneShot(click);
    }
}
