using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
namespace PCSW.Models
{
    public class DataModel
    {
        public List<ProvincialBoard> boardData { get; set; }
        public List<ProvincialCommittee> committeeData { get; set; }
        public List<ProvincialTaskForce> taskforceData { get; set; }
        public ProvincialData data { get; set; }

    }
}