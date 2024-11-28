using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NikitaKirakosyan.Minesweeper
{
    public class DifficultyButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private SpriteByDifficulty[] _spritesByDifficulties;

        private Dictionary<DifficultyType, (Sprite normalSprite, Sprite pressedSprite)> _cachedSpritesByDifficulty;


        private void OnEnable()
        {
            Game.OnGameSettingsChanged += Refresh;
            Refresh(Game.Instance.CurrentGameSettings);
            _button.onClick.AddListener(Game.Instance.ChangeGameSettings);
        }

        private void OnDisable()
        {
            Game.OnGameSettingsChanged -= Refresh;
        }


        private void Refresh(GameSettingsData gameSettings)
        {
            var sprites = GetSpriteByDifficulty(gameSettings.DifficultyType);
            
            _image.sprite = sprites.normal;
            
            var buttonSpriteState = _button.spriteState;
            buttonSpriteState.highlightedSprite = sprites.normal;
            buttonSpriteState.pressedSprite = sprites.pressed;
            buttonSpriteState.selectedSprite = sprites.normal;
            buttonSpriteState.disabledSprite = sprites.normal;
            _button.spriteState = buttonSpriteState;
        }

        private (Sprite normal, Sprite pressed) GetSpriteByDifficulty(DifficultyType difficultyType)
        {
            if(_cachedSpritesByDifficulty.IsNullOrEmpty())
            {
                _cachedSpritesByDifficulty = new ();
                foreach(var spriteByDifficulty in _spritesByDifficulties)
                    _cachedSpritesByDifficulty.Add(spriteByDifficulty.Difficulty, (spriteByDifficulty.NormalSprite, spriteByDifficulty.PressedSprite));
            }

            return _cachedSpritesByDifficulty[difficultyType];
        }


        [Serializable]
        private struct SpriteByDifficulty
        {
            public DifficultyType Difficulty;
            public Sprite NormalSprite;
            public Sprite PressedSprite;
        }
    }
}
