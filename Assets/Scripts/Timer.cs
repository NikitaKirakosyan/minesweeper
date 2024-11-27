using System.Collections;
using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public class Timer : Counter
    {
        private int _passedSeconds;
        
        
        public void Launch()
        {
            StartCoroutine(SlowUpdate());
        }

        public void Stop()
        {
            _passedSeconds = 0;
            StopAllCoroutines();
        }


        private IEnumerator SlowUpdate()
        {
            while(true)
            {
                SetValue(_passedSeconds);
                yield return new WaitForSeconds(1);
                _passedSeconds++;
            }
        }
    }
}
