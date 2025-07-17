using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndParametroProcesoDTO
    {
        public int Parmetcodi { get; set; }
        public DateTime Parmetfecproc { get; set; }
        public int Parmetdiamax { get; set; }
        public int Parmethoramax { get; set; }
        public int Parmettolerancia { get; set; }
        public string Parmetestado { get; set; }
        public string Parmetusumodificacion { get; set; }
        public DateTime Parmetfecmodificiacion { get; set; }
        public string strParmetfecproc
        
        {
            get
            {
                return String.Format("{0:dd/MM/yyy}", Parmetfecproc);
            }
        }
        public string strParmetfecmodificiacion
        {
            get
            {
                return String.Format("{0:dd/MM/yyy}", Parmetfecmodificiacion);
            }
        }
        //public int validacion { get; set; }
    }
}
