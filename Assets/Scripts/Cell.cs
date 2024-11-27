using UnityEngine;
using UnityEngine.UI;

namespace NikitaKirakosyan.Minesweeper
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _bombSprite;


        public void Init(bool hasBomb)
        {
            _image.sprite = hasBomb ? _bombSprite : _defaultSprite;
        }
    }
}