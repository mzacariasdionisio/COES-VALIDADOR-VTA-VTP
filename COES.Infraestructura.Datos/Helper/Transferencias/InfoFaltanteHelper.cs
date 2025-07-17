using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la consulta para la informacion faltante
    /// </summary>
    public class InfoFaltanteHelper:HelperBase
    {
        public InfoFaltanteHelper() : base(Consultas.InfoFaltanteSql)
        {
        }

        public InfoFaltanteDTO Create(IDataReader dr)
        {
            InfoFaltanteDTO entity = new InfoFaltanteDTO();

            //int iCodi = dr.GetOrdinal(this.Codi);
            //if (!dr.IsDBNull(iCodi)) entity.Codi = dr.GetInt32(iCodi);

            int iEmpresa = dr.GetOrdinal(this.Empresa);
            if (!dr.IsDBNull(iEmpresa)) entity.Empresa = dr.GetString(iEmpresa);

            int iBarra = dr.GetOrdinal(this.Barra);
            if (!dr.IsDBNull(iBarra)) entity.Barra = dr.GetString(iBarra);

            int iCliente = dr.GetOrdinal(this.Cliente);
            if (!dr.IsDBNull(iCliente)) entity.Cliente = dr.GetString(iCliente);

            int iCodigo = dr.GetOrdinal(this.Codigo);
            if (!dr.IsDBNull(iCodigo)) entity.Codigo = dr.GetString(iCodigo);

            return entity;
        }

        #region Mapeo de Campos

        public string PeriCodi = "PeriCodi";
        public string Codi = "Codi";
        public string Empresa = "Empresa";
        public string Barra = "Barra";
        public string Cliente = "Cliente";
        public string Codigo = "Codigo";
        public string EmprCodi = "EmprCodi";

        public string FechaInicio = "FechaInicio";
        public string FechaFin = "FechaFin";
        public string Tipo = "Tipo";

        //Parametro
        public string Version = "Version";

        #endregion

        public string SqlGetByListaPeriodoVersion
        {
            get { return base.GetSqlXml("GetByListaPeriodoVersion"); }
        }



        

    }
}
