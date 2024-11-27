using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    [CreateAssetMenu(menuName = "Configurations/GameSettingsData", fileName = nameof(GameSettingsData))]
    public class GameSettingsData : ScriptableObject
    {
        public Vector2 GameWindowSize = new (328, 412);
        public int CellsColumns = 9;
        public int CellsRows = 9;
        public int BombsCount = 10;
        [Min(2)] public int BombsRandomnicity = 100;
        [Min(2)] public int BombsRandomnDelta = 6;
    }
}