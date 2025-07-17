using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_BARRA_URS
    /// </summary>
    public class TrnBarraUrsHelper : HelperBase
    {
        public TrnBarraUrsHelper(): base(Consultas.TrnBarraUrsSql)
        {
        }

        public TrnBarraursDTO Create(IDataReader dr)
        {
            TrnBarraursDTO entity = new TrnBarraursDTO();

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = dr.GetInt32(iBarrcodi);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.GrupoCodi = dr.GetInt32(iGrupocodi);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.EquiCodi = dr.GetInt32(iEquicodi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = dr.GetInt32(iEmprcodi);

            int iBarUrsUsuCreacion = dr.GetOrdinal(this.BarUrsusucreacion);
            if (!dr.IsDBNull(iBarUrsUsuCreacion)) entity.BarUrsUsuCreacion = dr.GetString(iBarUrsUsuCreacion);

            int iBarusufeccreacion = dr.GetOrdinal(this.BarUrsfeccreacion);
            if (!dr.IsDBNull(iBarusufeccreacion)) entity.BarUrsFecCreacion = dr.GetDateTime(iBarusufeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Barrcodi = "BARRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string BarUrsusucreacion = "BARURSUSUCREACION";
        public string BarUrsfeccreacion = "BARURSFECCREACION";
        //ATRIBUTOS ADICIONALES PARA CONSULTAS
        public string Gruponomb = "GRUPONOMB";
        public string Equinomb = "EQUINOMB";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        //METODOS ADICIONALES
        public string SqlListURS
        {
            get { return base.GetSqlXml("ListURS"); }
        }

        public string SqlGetEmpresas
        {
            get { return base.GetSqlXml("GetEmpresas"); }
        }

        public string SqlGetByGrupoCodiTRN
        {
            get { return base.GetSqlXml("GetByGrupoCodiTRN"); }
        }

        //Implementaciones para la tabla PR_GRUPO
        public string SqlGetByNombrePrGrupo
        {
            get { return base.GetSqlXml("GetByNombrePrGrupo"); }
        }

        public string SqlListPrGrupo
        {
            get { return base.GetSqlXml("ListPrGrupo"); }
        }


        public string SqlListURSbyEquicodi
        {
            get { return base.GetSqlXml("ListURSbyEquicodi"); }
        }

        public string SqlGetByIdGrupoCodi
        {
            get { return base.GetSqlXml("GetByIdGrupoCodi"); }
        }

        public string SqlListURSCostoMarginal
        {
            get { return base.GetSqlXml("ListURSCostoMarginal"); }
        }
    }
}
