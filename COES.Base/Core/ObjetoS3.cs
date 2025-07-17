using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Core
{
    public class ObjetoS3
    {
        public string Key { get; set; }
        public string Etag { get; set; }
        public DateTime UltimaModificacion { get; set; }
        public string Propietario { get; set; }
        public long Size { get; set; }
    }
}
