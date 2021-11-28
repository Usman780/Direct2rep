using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Direct2Rep.Helping_Classes
{
    public class Comp_SaleRepDTOList
    {
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int Id { get; set; }
        public int? SaleRepID { get; set; }
        public string SalesRepNam { get; set; }
        public string SalesRepEmail { get; set; }
        public string SalesRepFirmName { get; set; }
        public string SalesRepEmailReceiveLeads { get; set; }
        public string SalesRepPictureUpload { get; set; }
        public string SalesRepLogoUpload { get; set; }
        public DateTime? Creadted_At { get; set; }
    }
}