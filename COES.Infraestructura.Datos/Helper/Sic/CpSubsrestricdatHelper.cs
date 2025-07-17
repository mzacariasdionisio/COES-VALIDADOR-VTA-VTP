using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using System.Data;
using COES.Dominio.DTO.Sic;

//using COES.Infraestructura.Datos.CortoPlazo.Modelo.Sql;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_SUBSRESTRICDAT
    /// </summary>
    public class CpSubsrestricdatHelper : HelperBase
    {
        public CpSubsrestricdatHelper() : base(Consultas.CpSubsrestricdatSql)
        {
        }

        public CpSubrestricdatDTO Create(IDataReader dr)
        {
            CpSubrestricdatDTO entity = new CpSubrestricdatDTO();

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt16(dr.GetValue(iTopcodi));

            int iSrestriccodi = dr.GetOrdinal(this.Srestriccodi);
            if (!dr.IsDBNull(iSrestriccodi)) entity.Srestcodi = Convert.ToInt16(dr.GetValue(iSrestriccodi));

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iValor1 = dr.GetOrdinal(this.Srestdvalor1);
            if (!dr.IsDBNull(iValor1)) entity.Srestdvalor1 = dr.GetDecimal(iValor1);

            int iValor2 = dr.GetOrdinal(this.Srestdvalor2);
            if (!dr.IsDBNull(iValor2)) entity.Srestdvalor2 = dr.GetDecimal(iValor2);

            int iValor3 = dr.GetOrdinal(this.Srestdvalor3);
            if (!dr.IsDBNull(iValor3)) entity.Srestdvalor3 = dr.GetDecimal(iValor3);

            int iValor4 = dr.GetOrdinal(this.Srestdvalor4);
            if (!dr.IsDBNull(iValor4)) entity.Srestdvalor4 = dr.GetDecimal(iValor4);

            int iActivo = dr.GetOrdinal(this.Srestdactivo);
            if (!dr.IsDBNull(iActivo)) entity.Srestdactivo = Convert.ToInt16(dr.GetValue(iActivo));

            int iOpcion = dr.GetOrdinal(this.Srestdopcion);
            if (!dr.IsDBNull(iOpcion)) entity.Srestdopcion = Convert.ToInt16(dr.GetValue(iOpcion));

            int iSrestfecha = dr.GetOrdinal(this.Srestfecha);
            if (!dr.IsDBNull(iSrestfecha)) entity.Srestfecha = dr.GetDateTime(iSrestfecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Topcodi = "TOPCODI";
        public string Srestriccodi = "SRESTCODI";
        public string Srestdvalor1 = "SRESTDVALOR1";
        public string Srestdvalor2 = "SRESTDVALOR2";
        public string Srestdvalor3 = "SRESTDVALOR3";
        public string Srestdvalor4 = "SRESTDVALOR4";
        public string Srestdactivo = "SRESTDACTIVO";
        public string Srestdopcion = "SRESTDOPCION";
        public string Recurcodi = "RECURCODI";
        public string Srestfecha = "SRESTFECHA";
        public string Catnombre = "CATNOMBRE";
        public string Recurconsideragams = "RECURCONSIDERAGAMS";

        #endregion

        public string SqlListarDatosRestriccion
        {
            get { return base.GetSqlXml("ListarDatosRestriccion"); }
        }

        //Yupana
        public string SqlListadeSubRestriccionCategoria
        {
            get { return base.GetSqlXml("ListadeSubRestriccionCategoria"); }
        }

        //Yupana Continuo
        public string SqlListRecursoEnSubRestriccion
        {
            get { return base.GetSqlXml("ListRecursoEnSubRestriccion"); }
        }

     
        public string SqlListarDatosSubRestriccion
        {
            get { return base.GetSqlXml("ListarDatosSubRestriccion"); }
        }

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }
    }
}
