using System.Collections;
using System.Collections.Generic;

namespace SyncNode.Settings
{
    public interface IWeaponApiSettings
    {
        public IEnumerable<string> Hosts { get; set; }
    }
}