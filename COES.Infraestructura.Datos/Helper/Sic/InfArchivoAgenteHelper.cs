using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla INF_ARCHIVO_AGENTE
    /// </summary>
    public class InfArchivoAgenteHelper : HelperBase
    {
        public InfArchivoAgenteHelper(): base(Consultas.InfArchivoAgenteSql)
        {
        }

        public InfArchivoAgenteDTO Create(IDataReader dr)
        {
            InfArchivoAgenteDTO entity = new InfArchivoAgenteDTO();

            int iArchicodi = dr.GetOrdinal(this.Archicodi);
            if (!dr.IsDBNull(iArchicodi)) entity.Archicodi = Convert.ToInt32(dr.GetValue(iArchicodi));

            int iArchinomb = dr.GetOrdinal(this.Archinomb);
            if (!dr.IsDBNull(iArchinomb)) entity.Archinomb = dr.GetString(iArchinomb);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iArchiruta = dr.GetOrdinal(this.Archiruta);
            if (!dr.IsDBNull(iArchiruta)) entity.Archiruta = dr.GetString(iArchiruta);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = Convert.ToDateTime(dr.GetValue(iLastdate));

            return entity;
        }


        #region Mapeo de Campos

        public string Archicodi = "ARCHICODI";
        public string Archinomb = "ARCHINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Archiruta = "ARCHIRUTA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
        public string SqlListadoPorEmpresa
        {
            get { return base.GetSqlXml("ListadoPorEmpresa"); }
        }
        public string SqlListadoPorEmpresaFechas
        {
            get { return base.GetSqlXml("ListadoPorEmpresaFechas"); }
        }
        public string SqlCantidadListadoPorEmpresa
        {
            get { return base.GetSqlXml("CantidadListadoPorEmpresa"); }
        }
        public string SqlCantidadListadoPorEmpresaFechas
        {
            get { return base.GetSqlXml("CantidadListadoPorEmpresaFechas"); }
        }
        public string SqlCantidadArchivosNombre
        {
            get { return base.GetSqlXml("CantidadArchivosNombre"); }
        }
        
    }
}
