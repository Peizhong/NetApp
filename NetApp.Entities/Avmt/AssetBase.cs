using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Avmt
{
    public class AssetBase : AccountBase
    {
        public int AssetState { get; set; }

        public int IsShareAsset { get; set; }
        public DateTime LeaveFactoryDate { get; set; }
        public string LeaveFactoryNo { get; set; }
        public DateTime PlantTransferDate { get; set; }
        public string Manufacturer { get; set; }
    }

    public class Device : AssetBase
    {
        public string DeviceName { get; set; }
        public string DeviceCode { get; set; }
    }

    public class Parts : AssetBase
    {
        public string PartsName { get; set; }
        public string PartsCode { get; set; }
        public string AssetId { get; set; }
    }
}
