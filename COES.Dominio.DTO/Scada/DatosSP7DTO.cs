using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    public class DatosSP7DTO : EntityBase
    {
        public Int32 Canalcodi { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaSistema { get; set; }
        public string Path { get; set; }
        public decimal Valor { get; set; }
        public int Calidad { get; set; }
        public string CalidadTexto { get; set; }
        public string TipoPunto { get; set; }
        public int Zonacodi { get; set; }
        public string CanalDescription { get; set; }
        public string ZonaDescription { get; set; }
        public string CodigoEquipo { get; set; }

        public DatosSP7DTO()
            : base()
        {

        }

        public DatosSP7DTO(DateTime fecha, DateTime fechaSistema, string path2, decimal valor, int calidad)
            : base()
        {
            this.Fecha = fecha;
            this.FechaSistema = fechaSistema;
            this.Path = path2;
            this.Valor = valor;
            this.Calidad = calidad;
        }

        public DatosSP7DTO(int canalcodi, DateTime fecha, DateTime fechaSistema, decimal valor, int calidad)
            : base()
        {
            Canalcodi = canalcodi;
            Fecha = fecha;
            FechaSistema = fechaSistema;
            Valor = valor;
            Calidad = calidad;
        }

        public DatosSP7DTO(int canalcodi, DateTime fecha, DateTime fechaSistema, decimal valor, int calidad, string tipopunto)
            : base()
        {
            Canalcodi = canalcodi;
            Fecha = fecha;
            FechaSistema = fechaSistema;
            Valor = valor;
            Calidad = calidad;
            TipoPunto = tipopunto;
        }

    }
}
