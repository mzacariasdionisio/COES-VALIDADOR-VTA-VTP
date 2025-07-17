using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.SIOSEIN.Util
{
    public class SioExcelHoja
    {
        public string NombreHoja { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo1 { get; set; }
        public string Subtitulo2 { get; set; }
        public List<int> ListaAnchoColumna { get; set; }
        public List<SioExcelModelo>[] ListaCabeceras { get; set; }
        public SioExcelCuerpo Cuerpo { get; set; }
        public List<SioExcelModelo>[] ListaPies { get; set; }
    }

    public class SioExcelModelo
    {
        public string Nombre { get; set; }
        public int NumColumnas { get; set; }
        public int NumFilas { get; set; }
        public string AlineaHorizontal { get; set; }
    }

    public class SioExcelCuerpo
    {
        public List<string>[] ListaRegistros { get; set; }
        public List<string> ListaAlineaHorizontal { get; set; }
        public List<string> ListaTipo { get; set; }
    }

}
