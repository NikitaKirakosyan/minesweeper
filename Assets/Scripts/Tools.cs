using UnityEngine;
using UnityEngine.UI;

namespace NikitaKirakosyan.Minesweeper
{
    public class Tools : MonoBehaviour
    {
        [SerializeField] private Button _zoomInButton;
        [SerializeField] private Button _zoomOutButton;
        [SerializeField] private Button _changeGenerationButton;
        [SerializeField] private Button _hintButton;


        private void Awake()
        {
            _zoomInButton.onClick.AddListener(OnZoomInButtonClick);
            _zoomOutButton.onClick.AddListener(OnZoomOutButtonClick);
            _changeGenerationButton.onClick.AddListener(OnChangeGenerationButtonClick);
            _hintButton.onClick.AddListener(OnHintButtonClick);
        }


        private void OnZoomInButtonClick()
        {
            ZoomController.Zoom(ZoomType.In);
        }
        
        private void OnZoomOutButtonClick()
        {
            ZoomController.Zoom(ZoomType.Out);
        }
        
        private void OnChangeGenerationButtonClick()
        {
            
        }
        
        private void OnHintButtonClick()
        {
            Game.Instance.ShowHint();
        }
    }
}