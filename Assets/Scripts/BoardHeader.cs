using UnityEngine;
using UnityEngine.UI;

namespace NikitaKirakosyan.Minesweeper
{
    public class BoardHeader : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;


        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClick);
        }


        private void OnRestartButtonClick()
        {
            Game.Instance.StartGame();
        }
    }
}