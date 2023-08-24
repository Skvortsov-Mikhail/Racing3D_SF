using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIRaceButton : UISelectableButton, IScriptableObjectProperty
{
    [SerializeField] private RaceInfo raceInfo;

    [SerializeField] private Image icon;
    [SerializeField] private Text title;
    [SerializeField] private GameObject locker;

    private void Start()
    {
        ApplyProperty(raceInfo);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (raceInfo == null) return;

        SceneManager.LoadScene(raceInfo.SceneName);
    }

    public void ApplyProperty(ScriptableObject property)
    {
        if (property == null) return;
        if (property is RaceInfo == false) return;

        raceInfo = property as RaceInfo;

        icon.sprite = raceInfo.Icon;
        title.text = raceInfo.Title;
    }

    public void LockRace()
    {
        locker.SetActive(true);
        Interactable = false;
    }

    public void UnlockRace()
    {
        locker.SetActive(false);
        Interactable = true;
    }
}
