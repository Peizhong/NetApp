using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Avmt
{
    public class AccountBase
    {
        public string Id { get; set; }
        public string WorkspaceId { get; set; }
        public string Name { get; set; }

        public string ProvinceCode { get; set; }
        public string BureauCode { get; set; }

        public int ObjectType { get; set; }
        public string ClassifyId { get; set; }
        public int VoltageId { get; set; }
        public int State { get; set; }
    }
}