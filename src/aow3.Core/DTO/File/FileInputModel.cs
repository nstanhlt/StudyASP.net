using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NAWASCO.ERP.Dto.File
{
    public class FileInputModel
    {
        public IFormFile FileToUpload { get; set; }
    }
}
