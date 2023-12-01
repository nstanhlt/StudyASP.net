using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAWASCO.ERP.DTO
{
    public class InputGetAllDto
    {
        public int? limit { get; set; }
        public int? start { get; set; }
        public string filter { get; set; }
        public string q { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        public string after { get; set; }
    }
}
