using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class RdoCumplimientoHelper : HelperBase
    {
        public RdoCumplimientoHelper() : base(Consultas.MeEnvioSql)
        {
        }


        public string RdoNombreEmpresa = "EMPRNOMB";
        public string H3 = "H3";
        public string H6 = "H6";
        public string H9 = "H9";
        public string H12 = "H12";
        public string H15 = "H15";
        public string H18 = "H18";
        public string H21 = "H21";
        public string H24 = "H24";

    }
}
