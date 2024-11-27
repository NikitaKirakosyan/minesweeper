using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public sealed class NumbersSpriteSheet : ScriptableObject
    {
        [SerializeField] private Sprite[] _numberSprites;


        public Sprite GetSprite(int number)
        {
            return _numberSprites[number];
        }
    }
}