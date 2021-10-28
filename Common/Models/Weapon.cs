using System;

namespace Common.Models
{
    public class Weapon: BaseEntityModel
    {
        public string Description { get; set; }
        public decimal Calibers { get; set; }
        public decimal Weight { get; set; }
        public decimal Capacity { get; set; }
        public FireMode FireMode { get; set; }
    }

    public enum FireMode
    {
        BoltAction = 0,
        PumpAction = 1,
        DoubleAction = 2,
        SemiAutomatic = 3,
        TwoRoundBurst = 4,
        ThreeRoundBurst = 5,
        Automatic = 6
    }
}