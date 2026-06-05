using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject _characterSelectPanel;
    [SerializeField] private Player _player;

    private void Start()
    {
        Time.timeScale = 0f;
        _characterSelectPanel.SetActive(true);
    }

    public void BTN_SelectDefault()
    {
        SelectCharacter(CharacterType.Default);
    }

    public void BTN_SelectTank()
    {
        SelectCharacter(CharacterType.Tank);
    }

    public void BTN_SelectSpeed()
    {
        SelectCharacter(CharacterType.Speed);
    }

    private void SelectCharacter(CharacterType characterType)
    {
        _player.SelectCharacter(characterType);
        _characterSelectPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}