using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_URS_MODO_OPERACION
    /// </summary>
    public class SmaUrsModoOperacionHelper : HelperBase
    {
        public SmaUrsModoOperacionHelper(): base(Consultas.SmaUrsModoOperacionSql)
        {
        }

        public SmaUrsModoOperacionDTO Create(IDataReader dr)
        {
            SmaUrsModoOperacionDTO entity = new SmaUrsModoOperacionDTO();

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iUrsnomb = dr.GetOrdinal(this.Ursnomb);
            if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

            int iUrstipo = dr.GetOrdinal(this.Urstipo);
            if (!dr.IsDBNull(iUrstipo)) entity.Urstipo = dr.GetString(iUrstipo);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iGrupocodincp = dr.GetOrdinal(this.Grupocodincp);
            if (!dr.IsDBNull(iGrupocodincp)) entity.Grupocodincp = Convert.ToInt32(dr.GetValue(iGrupocodincp)); 

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iGrupotipo = dr.GetOrdinal(this.Grupotipo);
            if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);


            int iCatecodi = dr.GetOrdinal(this.Catecodi);
            if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

            int iGrupopadre = dr.GetOrdinal(this.Grupopadre);
            if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

            int iCentral = dr.GetOrdinal(this.Central);
            if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

            return entity;
        }

        public SmaUrsModoOperacionDTO CreateList(IDataReader dr)
        {
            SmaUrsModoOperacionDTO entity = new SmaUrsModoOperacionDTO();

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iUrsnomb = dr.GetOrdinal(this.Ursnomb);
            if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

            int iUrstipo = dr.GetOrdinal(this.Urstipo);
            if (!dr.IsDBNull(iUrstipo)) entity.Urstipo = dr.GetString(iUrstipo);

            return entity;
        }


        #region Mapeo de Campos

        public string Usercode = "USERCODE"; // 17-dic-2015 

        public string Urscodi = "URSCODI";
        public string Ursnomb = "URSNOMB";
        public string Urstipo = "URSTIPO";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Grupoabrev = "GRUPOABREV";
        public string Gruponombncp = "GRUPONOMBNCP";
        public string Grupocodincp = "GRUPOCODINCP";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Grupotipo = "GRUPOTIPO";

        public string Catecodi = "CATECODI";
        public string Grupopadre = "GRUPOPADRE";
        public string Central = "CENTRAL";

        #endregion

        public string SqlListUrs
        {
            get { return GetSqlXml("ListUrs"); }
        }

        public string SqlListInUrs
        {
            get { return GetSqlXml("ListInUrs"); }
        }

        public string SqlListMO
        {
            get { return GetSqlXml("ListMO"); }
        }

    }
}
