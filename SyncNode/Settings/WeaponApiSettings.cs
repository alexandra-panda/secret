using System.Collections.Generic;

namespace SyncNode.Settings
{
    public class WeaponApiSettings: IWeaponApiSettings
    {
        public IEnumerable<string> Hosts { get; set; }
    }
}