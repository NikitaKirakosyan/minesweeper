using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NikitaKirakosyan.Minesweeper
{
    public class Cell : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnOpened;

        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _openedSprite;
        [SerializeField] private Sprite _flagSprite;
        [SerializeField] private Sprite _bombSprite;
        [SerializeField] private Sprite _bombMistakeSprite;

        private bool _isInitialized;

        public Vector2Int MatrixPosition { get; private set; }
        public bool IsActive { get; private set; }
        public bool HasBomb { get; private set; }
        public bool HasFlag { get; private set; }
        public bool IsOpened => _image.sprite == _openedSprite || _image.sprite == _bombSprite || _image.sprite == _bombMistakeSprite;


        public override string ToString()
        {
            return $"position: {MatrixPosition} | isActive: {IsActive} | hasBomb: {HasBomb} | hasFlag: {HasFlag} | isOpened: {IsOpened}";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(Game.Instance.IsOpenAndFlagInversed)
            {
                if(eventData.button == PointerEventData.InputButton.Right)
                    Open(false);
                else if(eventData.button == PointerEventData.InputButton.Left)
                    SetFlag(!HasFlag);
            }
            else
            {
                if(eventData.button == PointerEventData.InputButton.Left)
                    Open(false);
                else if(eventData.button == PointerEventData.InputButton.Right)
                    SetFlag(!HasFlag);
            }
        }

        public void Init(Vector2Int matrixPosition, bool hasBomb)
        {
            _isInitialized = true;
            MatrixPosition = matrixPosition;
            HasBomb = hasBomb;
            _image.sprite = _defaultSprite;
            SetActive(true);
        }

        public void SetFlag(bool value)
        {
            if(IsOpened)
                return;
            
            HasFlag = value;
            _image.sprite = HasFlag ? _flagSprite : _defaultSprite;
        }

        public void Open(bool isAutoOpen)
        {
            if(!_isInitialized || IsOpened || HasFlag)
                return;

            if(isAutoOpen)
                _image.sprite = HasBomb ? _bombSprite : _openedSprite;
            else
                _image.sprite = HasBomb ? _bombMistakeSprite : _openedSprite;
            
            OnOpened?.Invoke();
        }

        public void SetActive(bool value)
        {
            if(IsActive == value)
                return;

            IsActive = value;
            gameObject.SetActive(IsActive);

            if(!IsActive)
                _isInitialized = false;
        }
    }
}