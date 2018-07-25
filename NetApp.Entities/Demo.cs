using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Entities
{
    [Table("car")]
    public class Car
    {
        [Column("carid")]
        public int CarId { get; set; }

        [Column("LicensePlate")]
        public string LicensePlate { get; set; }

        public List<RecordOfSale> SaleHistory { get; set; }
    }

    [Table("RecordOfSale")]
    public class RecordOfSale
    {
        [Column("RecordOfSaleId")]
        public int RecordOfSaleId { get; set; }

        [Column("CarLicensePlate")]
        public string CarLicensePlate { get; set; }

       public Car Car { get; set; }
    }
}
