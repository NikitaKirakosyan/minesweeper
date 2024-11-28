using UnityEngine;
using UnityEngine.UI;

namespace NikitaKirakosyan.Minesweeper
{
    public class FlipButton : MonoBehaviour
    {
        [SerializeField] private Sprite _flipButtonNormal;
        [SerializeField] private Sprite _flipButtonPressed;
        [Header("Inversed")]
        [SerializeField] private Sprite _inversedFlipButtonNormal;
        [SerializeField] private Sprite _inversedFlipButtonPressed;
        [Header("Components")] 
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        
        private void Start()
        {
            Refresh();
            _button.onClick.AddListener(Game.InverseOpenAndFlag);
            _button.onClick.AddListener(Refresh);
        }

        
        private void Refresh()
        {
            _image.sprite = Game.IsOpenAndFlagInversed ? _inversedFlipButtonNormal : _flipButtonNormal;
            
            var buttonSpriteState = _button.spriteState;
            buttonSpriteState.highlightedSprite = Game.IsOpenAndFlagInversed ? _inversedFlipButtonNormal : _flipButtonNormal;
            buttonSpriteState.pressedSprite = Game.IsOpenAndFlagInversed ? _inversedFlipButtonPressed : _flipButtonPressed;
            buttonSpriteState.selectedSprite = Game.IsOpenAndFlagInversed ? _inversedFlipButtonNormal : _flipButtonNormal;
            buttonSpriteState.disabledSprite = Game.IsOpenAndFlagInversed ? _inversedFlipButtonNormal : _flipButtonNormal;
            _button.spriteState = buttonSpriteState;
        }
    }
}
