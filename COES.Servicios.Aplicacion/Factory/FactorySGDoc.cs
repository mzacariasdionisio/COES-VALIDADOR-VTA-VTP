using COES.Dominio.Interfaces.SGDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Infraestructura.Datos.Repositorio.SGDoc;

namespace COES.Servicios.Aplicacion.Factory
{
    public class FactorySGDoc
    {
        public static string StrConexion = "ContextoSIC";

        public static IConsultaRepository GetConsultaRepository()
        {
            return new ConsultaRepository(StrConexion);
        }
    }
}
