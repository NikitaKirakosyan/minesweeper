using UnityEngine;
using UnityEngine.UI;

namespace NikitaKirakosyan.Minesweeper
{
    public class BoardHeader : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Counter _bombsCounter;
        [SerializeField] private Timer _timerCounter;


        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClick);
        }


        public void SetBombsCounter(int bombsCount)
        {
            _bombsCounter.SetValue(bombsCount);
        }

        public void LaunchTimer()
        {
            _timerCounter.Launch();
        }

        public void StopTimer()
        {
            _timerCounter.Stop();
        }
        

        private void OnRestartButtonClick()
        {
            Game.Instance.StartGame();
        }
    }
}