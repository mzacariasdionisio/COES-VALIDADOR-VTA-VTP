using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_INFOADICIONAL
    /// </summary>
    public class TrnInfoadicionalHelper : HelperBase
    {
        public TrnInfoadicionalHelper(): base(Consultas.TrnInfoadicionalSql)
        {
        }

        public TrnInfoadicionalDTO Create(IDataReader dr)
        {
            TrnInfoadicionalDTO entity = new TrnInfoadicionalDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iInfadicodi = dr.GetOrdinal(this.Infadicodi);
            if (!dr.IsDBNull(iInfadicodi)) entity.Infadicodi = Convert.ToInt32(dr.GetValue(iInfadicodi));

            int iInfadinomb = dr.GetOrdinal(this.Infadinomb);
            if (!dr.IsDBNull(iInfadinomb)) entity.Infadinomb = dr.GetString(iInfadinomb);

            int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

            int iFechacorte = dr.GetOrdinal(this.Fechacorte);
            if (!dr.IsDBNull(iFechacorte)) entity.Fechacorte = dr.GetDateTime(iFechacorte);

            int iFechacorteDesc = dr.GetOrdinal(this.Fechacorte);
            if (!dr.IsDBNull(iFechacorte)) entity.Fechacortedesc = dr.GetDateTime(iFechacorteDesc).ToString(ConstantesBase.FormatoFecha);

            int iInfadiestado = dr.GetOrdinal(this.Infadiestado);
            if (!dr.IsDBNull(iInfadiestado)) entity.Infadiestado = dr.GetString(iInfadiestado);

            int iUsucreacion = dr.GetOrdinal(this.Usucreacion);
            if (!dr.IsDBNull(iUsucreacion)) entity.UsuCreacion = dr.GetString(iUsucreacion);

            int iInfadicodosinergmin = dr.GetOrdinal(this.Infadicodosinergmin);
            if (!dr.IsDBNull(iInfadicodosinergmin)) entity.Infadicodosinergmin = dr.GetString(iInfadicodosinergmin);

            return entity;
        }


        public string SqlListVersion
        {
            get { return GetSqlXml("ListVersiones"); }
        }

        #region Mapeo de Campos

        public string Emprcodi = "EMPRCODI";
        public string Infadicodi = "INFADICODI";
        public string Infadinomb = "INFADINOMB";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Fechacorte = "FECHACORTE";
        public string Infadiestado = "INFADIESTADO";
        public string Usucreacion = "USUCREACION";
        public string Datecreacion = "DATECREACION";
        public string Usuupdate = "USUUPDATE";
        public string Dateupdate = "DATEUPDATE";
        public string Infadicodosinergmin = "INFADICODOSINERGMIN";

        //Atributos agregados para Empresas con dos comportamientos
        public string Emprnomb = "EMPRNOMB";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Emprcodosinergmin = "EMPRCODOSINERGMIN";
        #endregion
    }
}
