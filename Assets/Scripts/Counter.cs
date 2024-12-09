using System;
using UnityEngine;
using UnityEngine.UI;

namespace NikitaKirakosyan.Minesweeper
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private Image[] _images;
        [SerializeField] private NumbersSpriteSheet _numberSpriteSheet;

        private int _maxValue;

        private int MaxValue
        {
            get
            {
                if(_maxValue > 0)
                    return _maxValue;

                if(_images.Length == 0)
                    return 0;

                var topNumber = 9;
                var maxValStr = string.Empty;
                for(var i = 0; i < _images.Length; i++)
                {
                    maxValStr += topNumber;
                }

                _maxValue = int.Parse(maxValStr);
                return int.Parse(maxValStr);
            }
        }


        public void SetValue(int value)
        {
            if(value < 0 || value > MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), $"Value must be >= 0 and <= {MaxValue}");

            var valueDigits = GetValueDigits(value, _images.Length);
            for(var i = 0; i < _images.Length; i++)
            {
                var image = _images[i];
                image.sprite = _numberSpriteSheet.GetSprite(valueDigits[i]);
            }
        }

        private int[] GetValueDigits(int value, int maxDigits)
        {
            var valueDigits = new int[maxDigits];
            var valueToCharsArray = value.ToString().ToCharArray();
            var delta = _images.Length - valueToCharsArray.Length;

            var tempDelta = delta;
            while(tempDelta > 0)
                valueDigits[tempDelta--] = 0;

            for(var i = 0; i < valueToCharsArray.Length; i++)
                valueDigits[i + delta] = valueToCharsArray[i] - '0';

            return valueDigits;
        }
    }
}