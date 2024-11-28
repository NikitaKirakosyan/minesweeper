using System;
using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public static class ZoomController
    {
        private const int ZoomMax = 100;
        private const int ZoomMin = -75;

        public static Action<float> OnZoomChanged;
        
        private static int ZoomStep = 25;
        private static int CurrentZoom;
        

        static ZoomController()
        {
            CurrentZoom = 0;
        }
        
        
        public static void Zoom(ZoomType zoomType)
        {
            switch(zoomType)
            {
                case ZoomType.In:
                    CurrentZoom = Mathf.Min(CurrentZoom + ZoomStep, ZoomMax);
                    break;
                
                case ZoomType.Out:
                    CurrentZoom = Mathf.Max(CurrentZoom - ZoomStep, ZoomMin);
                    break;
            }
            
            OnZoomChanged?.Invoke(CurrentZoom);
        }
    }
    
    public enum ZoomType
    {
        In,
        Out,
    }
}
