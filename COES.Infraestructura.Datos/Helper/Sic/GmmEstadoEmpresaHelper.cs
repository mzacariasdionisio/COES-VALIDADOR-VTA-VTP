using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class GmmEstadoEmpresaHelper : HelperBase
    {
        public GmmEstadoEmpresaHelper()
            : base(Consultas.GmmEstadoEmpresaSql)
        {

        }

        #region Mapeo de Campos
        public string Estcodi = "ESTCODI";
        public string Estfecregistro="ESTFECREGISTRO";
        public string Estestado="ESTESTADO";
        public string Estusuedicion="ESTUSUEDICION";
        public string Empgcodi="EMPGCODI";
        #endregion
    }
}
