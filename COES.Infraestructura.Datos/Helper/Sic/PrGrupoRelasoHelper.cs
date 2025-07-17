using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_GRUPORELASO
    /// </summary>
    public class PrGrupoRelasoHelper : HelperBase
    {
        public PrGrupoRelasoHelper()
            //: base(Consultas.PrGrupoSql)
        {
        }
        //                                                                                                                         
        public string Codrel = "COD_REL";
        public string Tiporel = "TIPO_REL";
        public string Codsddp = "COD_SDDP";
        public string descsddp = "DESC_SDDP";
        public string Codsic = "COD_SIC";
        public string Descsic = "DESC_SIC";
        public string Tag = "TAG";
        public string Secuencia = "SECUENCIA";
        public string Fechamodi = "FECHA_MODI";
        public string Usuamodi = "USUA_MODI";
    }
}
