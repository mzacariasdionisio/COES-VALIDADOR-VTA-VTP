using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class IeodCuadroDTO : EntityBase
    {
        public int ICCODI { get; set; }
        public int EQUICODI { get; set; }
        public int SUBCAUSACODI { get; set; }
        public string SUBCAUSADESC { get; set; }
        public DateTime ICHORINI { get; set; }
        public DateTime ICHORFIN { get; set; }
        public string ICDESCRIP1 { get; set; }
        public string ICDESCRIP2 { get; set; }
        public string ICDESCRIP3 { get; set; }
        public string ICCHECK1 { get; set; }
        public decimal ICVALOR1 { get; set; }
        public string LASTUSER { get; set; }
        public DateTime LASTDATE { get; set; }
        public int NUMTRSGSUBIT { get; set; }
        public int NUMTRSGSOSTN { get; set; }
        public string ICCHECK2 { get; set; }
        public int EVENCLASECODI { get; set; }
        public DateTime ICHOR3 { get; set; }
        public string FECHA { get; set; }

        public string Icestado { get; set; }
        public string Icnombarchenvio { get; set; }
        public string Icnombarchfisico { get; set; }
        public string IcnombarchfisicoAnt { get; set; }
        public int CambioArchivo { get; set; }
        public string TAREAABREV { get; set; }
        public string AREANOMB { get; set; }
        public string FAMABREV { get; set; }
        public string EMPRENOMB { get; set; }
        public string EQUIABREV { get; set; }
        public string EQUINOMB { get; set; }
        public Nullable<short> EMPRCODI { get; set; }

        public string RUS { get; set; }
        public string HORA { get; set; }
        public string TIPO { get; set; }
        /// <summary>
        /// -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
        /// </summary>
        public int opCrud { get; set; }
    }
}

