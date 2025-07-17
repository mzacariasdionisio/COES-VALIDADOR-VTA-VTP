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
    public class TrnConsumoNoAutorizadoHelper : HelperBase
    {
        public TrnConsumoNoAutorizadoHelper() : base(Consultas.TrnConsumoNoAutorizadoSql)
        {

        }

        public TrnConsumoNoAutorizadoDTO Create(IDataReader dr)
        {
            TrnConsumoNoAutorizadoDTO entity = new TrnConsumoNoAutorizadoDTO();

            #region Campos Originales
            // CONSCODI
            int iConscodi = dr.GetOrdinal(this.Conscodi);
            if (!dr.IsDBNull(iConscodi)) entity.Conscodi = dr.GetInt32(iConscodi);

            // EMPRCODI
            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            // EMPRNOMB
            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            // VALORCNA
            int iValorcna = dr.GetOrdinal(this.Valorcna);
            if (!dr.IsDBNull(iValorcna)) entity.Valorcna = dr.GetDecimal(iValorcna);

            // FECHACNA
            int iFechacna = dr.GetOrdinal(this.Fechacna);
            if (!dr.IsDBNull(iFechacna)) entity.Fechacna = dr.GetDateTime(iFechacna);

            // LASTUSER
            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            // LASTDATE
            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            #endregion

            return entity;
        }

        #region Campos Originales
        public string Conscodi = "CONSCODI";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Valorcna = "VALORCNA";
        public string Fechacna = "FECHACNA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        #endregion
    }
}
