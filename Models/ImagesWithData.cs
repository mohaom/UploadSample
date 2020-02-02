using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFileSampler.Models
{
    public class ImagesWithData
    {

        public IEnumerable<System.IO.FileInfo> fileInfos;

        public string Name { get; set; }

    }
}
