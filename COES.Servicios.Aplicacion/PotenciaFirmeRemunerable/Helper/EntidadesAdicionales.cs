using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.PotenciaFirmeRemunerable.Helper
{
    public class EntidadesAdicionales
    {
        /// <summary>
        /// Permite mapear el resultado GAMS
        /// </summary>
        public class ResultadoGams
        {
            public string Nombarra { get; set; }
            public decimal Energia { get; set; }
            public decimal Congestion { get; set; }
            public decimal Total { get; set; }
        }

        public class SalidaGams
        {
            public int Tipo { get; set; }
            public int Escenariocodi { get; set; }
            public string Id { get; set; }           
            public decimal? Valor { get; set; }
        }
    }
}
