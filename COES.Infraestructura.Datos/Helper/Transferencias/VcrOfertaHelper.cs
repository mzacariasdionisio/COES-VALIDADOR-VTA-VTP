using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_OFERTA
    /// </summary>
    public class VcrOfertaHelper : HelperBase
    {
        public VcrOfertaHelper(): base(Consultas.VcrOfertaSql)
        {
        }

        public VcrOfertaDTO Create(IDataReader dr)
        {
            VcrOfertaDTO entity = new VcrOfertaDTO();

            int iVcrofecodi = dr.GetOrdinal(this.Vcrofecodi);
            if (!dr.IsDBNull(iVcrofecodi)) entity.Vcrofecodi = Convert.ToInt32(dr.GetValue(iVcrofecodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iVcrofecodigoenv = dr.GetOrdinal(this.Vcrofecodigoenv);
            if (!dr.IsDBNull(iVcrofecodigoenv)) entity.Vcrofecodigoenv = dr.GetString(iVcrofecodigoenv);

            int iVcrofefecha = dr.GetOrdinal(this.Vcrofefecha);
            if (!dr.IsDBNull(iVcrofefecha)) entity.Vcrofefecha = dr.GetDateTime(iVcrofefecha);

            int iVcrofehorinicio = dr.GetOrdinal(this.Vcrofehorinicio);
            if (!dr.IsDBNull(iVcrofehorinicio)) entity.Vcrofehorinicio = dr.GetDateTime(iVcrofehorinicio);

            int iVcrofehorfinal = dr.GetOrdinal(this.Vcrofehorfinal);
            if (!dr.IsDBNull(iVcrofehorfinal)) entity.Vcrofehorfinal = dr.GetDateTime(iVcrofehorfinal);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrofemodoperacion = dr.GetOrdinal(this.Vcrofemodoperacion);
            if (!dr.IsDBNull(iVcrofemodoperacion)) entity.Vcrofemodoperacion = dr.GetString(iVcrofemodoperacion);

            int iVcrofepotofertada = dr.GetOrdinal(this.Vcrofepotofertada);
            if (!dr.IsDBNull(iVcrofepotofertada)) entity.Vcrofepotofertada = dr.GetDecimal(iVcrofepotofertada);

            int iVcrofeprecio = dr.GetOrdinal(this.Vcrofeprecio);
            if (!dr.IsDBNull(iVcrofeprecio)) entity.Vcrofeprecio = dr.GetDecimal(iVcrofeprecio);

            int iVcrofeusucreacion = dr.GetOrdinal(this.Vcrofeusucreacion);
            if (!dr.IsDBNull(iVcrofeusucreacion)) entity.Vcrofeusucreacion = dr.GetString(iVcrofeusucreacion);

            int iVcrofefeccreacion = dr.GetOrdinal(this.Vcrofefeccreacion);
            if (!dr.IsDBNull(iVcrofefeccreacion)) entity.Vcrofefeccreacion = dr.GetDateTime(iVcrofefeccreacion);

            int iVcrofetipocarga = dr.GetOrdinal(this.Vcrofetipocarga);
            if (!dr.IsDBNull(iVcrofetipocarga)) entity.Vcrofetipocarga = Convert.ToInt32(dr.GetValue(iVcrofetipocarga));

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrofecodi = "VCROFECODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Usercode = "USERCODE";
        public string Vcrofecodigoenv = "VCROFECODIGOENV";
        public string Vcrofefecha = "VCROFEFECHA";
        public string Vcrofehorinicio = "VCROFEHORINICIO";
        public string Vcrofehorfinal = "VCROFEHORFINAL";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrofemodoperacion = "VCROFEMODOPERACION";
        public string Vcrofepotofertada = "VCROFEPOTOFERTADA";
        public string Vcrofeprecio = "VCROFEPRECIO";
        public string Vcrofeusucreacion = "VCROFEUSUCREACION";
        public string Vcrofefeccreacion = "VCROFEFECCREACION";
        public string Vcrofetipocarga = "VCROFETIPOCARGA";

        //Variables utilizados en las consultas
        public string Username = "USERNAME";

        #endregion

        //METODO PARA LA TABLA FW_USER
        public string SqlGetByFwUserByNombre
        {
            get { return base.GetSqlXml("GetByFwUserByNombre"); }
        }

        public string SqlGetByIdMaxDia
        {
            get { return base.GetSqlXml("GetByIdMaxDia"); }
        }

        public string SqlGetByIdMaxDiaGrupoCodi
        {
            get { return base.GetSqlXml("GetByIdMaxDiaGrupoCodi"); }
        }

        public string SqlGetByIdMaxMes
        {
            get { return base.GetSqlXml("GetByIdMaxMes"); }
        }

        public string SqlGetByIdMaxDiaUrs
        {
            get { return base.GetSqlXml("GetByIdMaxDiaUrs"); }
        }

        //ASSETEC 20190115
        public string SqlListSinDuplicados
        {
            get { return base.GetSqlXml("ListSinDuplicados"); }
        }

        public string SqlGetByCriteriaVcrOferta
        {
            get { return base.GetSqlXml("GetByCriteriaVcrOferta"); }
        }
        //--------------------------------------------------------

        public string SqlGetOfertaMaxDiaGrupoCodiHiHf
        {
            get { return base.GetSqlXml("GetOfertaMaxDiaGrupoCodiHiHf"); }
        }

        public string SqlGetOfertaMaxDiaGrupoCodiHiHf2020
        {
            get { return base.GetSqlXml("GetOfertaMaxDiaGrupoCodiHiHf2020"); }
        }
    }
}
