using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PmoDatDbusHelper : HelperBase
    {
        public PmoDatDbusHelper()
            : base(Consultas.PmoDatDbus)
        {
        }

        public PmoDatDbusDTO Create(IDataReader dr)
        {
            PmoDatDbusDTO entity = new PmoDatDbusDTO();

            int iPmDbusCodi = dr.GetOrdinal(this.PmDbusCodi);
            if (!dr.IsDBNull(iPmDbusCodi)) entity.PmDbusCodi = dr.GetInt32(iPmDbusCodi);

            int iGrupoCodiSddp = dr.GetOrdinal(this.GrupoCodiSddp);
            if (!dr.IsDBNull(iGrupoCodiSddp)) entity.GrupoCodiSddp = dr.GetInt32(iGrupoCodiSddp);

            int iGrupoNomb = dr.GetOrdinal(this.GrupoNomb);
            if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

            int iPmDbusIdSistema = dr.GetOrdinal(this.PmDbusIdSistema);
            if (!dr.IsDBNull(iPmDbusIdSistema)) entity.PmDbusIdSistema = dr.GetString(iPmDbusIdSistema);

            int iPmDbusNroSecGen = dr.GetOrdinal(this.PmDbusNroSecGen);
            if (!dr.IsDBNull(iPmDbusNroSecGen)) entity.PmDbusNroSecGen = dr.GetInt32(iPmDbusNroSecGen);

            int iTg = dr.GetOrdinal(this.Tg);
            if (!dr.IsDBNull(iTg)) entity.Tg = dr.GetInt32(iTg);

            int iCodCentral = dr.GetOrdinal(this.CodCentral);
            if (!dr.IsDBNull(iCodCentral)) entity.CodCentral = dr.GetInt32(iCodCentral);

            int iNombCentral = dr.GetOrdinal(this.NombCentral);
            if (!dr.IsDBNull(iNombCentral)) entity.NombCentral = dr.GetString(iNombCentral);

            int iPmDbusNroArea = dr.GetOrdinal(this.PmDbusNroArea);
            if (!dr.IsDBNull(iPmDbusNroArea)) entity.PmDbusNroArea = dr.GetString(iPmDbusNroArea);

            return entity;
        }

        public string SqlGetDat
        {
            get { return base.GetSqlXml("GetDat"); }
        }
        public string SqlGetCount
        {
            get { return base.GetSqlXml("GetCount"); }
        }

        public string SqlGenerateDat
        {
            get { return base.GetSqlXml("GenerateDat"); }
        }

        public string Cant = "CANT";
        public string PmPeriCodi = "PMPERICODI";

        public string PmDbusCodi = "PMDBUSCODI";
        public string GrupoCodiSddp = "GRUPOCODISDDP";
        public string GrupoNomb = "GRUPONOMB";
        public string PmDbusIdSistema = "PMDBUSIDSISTEMA";
        public string PmDbusNroSecGen = "PMDBUSNROSECGEN";
        public string CodCentral = "CODCENTRAL";
        public string NombCentral = "NOMBCENTRAL";
        public string PmDbusNroArea = "PMDBUSNROAREA";

        public string Num = "Num";
        public string Tp = "Tp";
        public string NombreB = "NombreB";
        public string Id = "Id";
        public string Numeral = "Numeral";
        public string Tg = "Tg";
        public string Plnt = "Plnt";
        public string NombreGener = "NombreGener";
        public string Area = "Area";
        public string Per1 = "Per1";
        public string Ploa1 = "Ploa1";
        public string Pind1 = "Pind1";
        public string PerF1 = "PerF1";
        public string Per2 = "Per2";
        public string Ploa2 = "Ploa2";
        public string Pind2 = "Pind2";
        public string PerF2 = "PerF2";
        public string Per3 = "Per3";
        public string Ploa3 = "Ploa3";
        public string Pind3 = "Pind3";
        public string PerF3 = "PerF3";
        public string Per4 = "Per4";
        public string Ploa4 = "Ploa4";
        public string Pind4 = "Pind4";
        public string PerF4 = "PerF4";
        public string Per5 = "Per5";
        public string Ploa5 = "Ploa5";
        public string Pind5 = "Pind5";
        public string PerF5 = "PerF5";
        public string Icca = "Icca";
    }
}
