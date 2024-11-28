using System.Collections;
using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public class Timer : Counter
    {
        private int _passedSeconds;
        
        
        public void Launch()
        {
            if(_passedSeconds > 0)
                return;
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
                _passedSeconds++;
                yield return new WaitForSeconds(1);
            }
        }
    }
}
