using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class PeriodoDeclaracionHelper : HelperBase
    {
        public PeriodoDeclaracionHelper() : base(Consultas.PeriodoDeclaracionSql)
        {
        }
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private object valorReturn(IDataReader dr, string sColumna)
        {
            object resultado = null;
            int iIndex;
            if (columnsExist(sColumna, dr))
            {
                iIndex = dr.GetOrdinal(sColumna);
                if (!dr.IsDBNull(iIndex))
                    resultado = dr.GetValue(iIndex);
            }
            return resultado?.ToString();
        }
        public static T? ConvertToNull<T>(object x) where T : struct
        {
            return x == null ? null : (T?)Convert.ChangeType(x, typeof(T));
        }

        public PeriodoDeclaracionDTO Create(IDataReader dr)
        {
            PeriodoDeclaracionDTO entity = new PeriodoDeclaracionDTO();
             
            //potencia contrato
            entity.PeridcCodi = Convert.ToInt32(valorReturn(dr, PeridcCodi) ?? 0);
            entity.PeridcNombre = (valorReturn(dr, PeridcNombre)?.ToString());
            entity.PeridcAnio = Convert.ToInt32(valorReturn(dr, PeridcAnio));
            entity.PeridcMes = Convert.ToInt32(valorReturn(dr, PeridcMes));
            entity.PeridcAnioMes = valorReturn(dr, PeridcAnioMes)?.ToString();
            entity.PeridcFecRegi = ConvertToNull<DateTime>(valorReturn(dr, PeridcFecRegi));
            entity.PeridcUsuarioRegi = valorReturn(dr, PeridcUsuarioRegi)?.ToString();
            entity.PeridcEstado = valorReturn(dr, PeridcEstado)?.ToString();
            entity.EstdAbrev = valorReturn(dr, EstdAbrev)?.ToString();
            entity.PeridcNuevo = Convert.ToInt32(valorReturn(dr, PeridcNuevo));


            return entity;
        }
        #region Mapeo de Campos


        //potencia contrato
        public string PeridcCodi = "PERIDCCODI";
        public string PeridcNombre = "PERIDCNOMBRE";
        public string PeridcAnio = "PERIDCANIO";
        public string PeridcMes = "PERIDCMES";
        public string PeridcAnioMes = "PERIDCANIOMES";
        public string PeridcFecRegi = "PERIDCFECREGI";
        public string PeridcUsuarioRegi = "PERIDCUSUARIOREGI";
        public string PeridcEstado = "PERIDCESTADO";
        public string EstdAbrev = "ESTDABREV";
        public string PeridcNuevo = "PERIDCNUEVO";
        public string Mensaje = "MENSAJE";

        #endregion Mapeo de Campos


        #region SQL
        public string SqlListaCombobox
        {
            get { return base.GetSqlXml("GetListaCombobox"); }
        }
        
        public string SqlListaPeriodoDeclaracion
        {
            get { return base.GetSqlXml("GetListaPeriodoDeclaracion"); }
        }
        public string SqlSaveUpdate
        {
            get { return GetSqlXml("SaveUpdate"); }
        }
        #endregion SQL
    }
}
