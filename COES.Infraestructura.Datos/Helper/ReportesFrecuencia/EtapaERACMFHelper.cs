using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.ReportesFrecuencia
{
    public class EtapaERAHelper : HelperBase
    {
        public EtapaERAHelper() : base(Consultas.EtapaERASql)
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

        public EtapaERADTO Create(IDataReader dr)
        {
            EtapaERADTO entity = new EtapaERADTO();
            entity.EtapaCodi = Convert.ToInt32(valorReturn(dr, EtapaCodi) ?? 0);
            entity.NombreEtapa = valorReturn(dr, Nombre)?.ToString();
            entity.Umbral = Convert.ToDecimal(valorReturn(dr, Umbral) ?? 0);
            entity.EtapaEstado = valorReturn(dr, Estado)?.ToString();
            entity.FecRegi = ConvertToNull<DateTime>(valorReturn(dr, FecCreacion));
            entity.UsuarioRegi = valorReturn(dr, UsuCreacion)?.ToString();
            entity.FecAct = ConvertToNull<DateTime>(valorReturn(dr, FecModifica));
            entity.UsuarioAct = valorReturn(dr, UsuModifica)?.ToString();
            return entity;
        }
        #region Mapeo de Campos


        //EtapaERA
        public string EtapaCodi = "ETAPACODI";
        public string Nombre = "NOMBRE";
        public string Umbral = "UMBRAL";
        public string Estado = "ESTADO";
        public string FecCreacion = "FECCREACION";
        public string UsuCreacion = "GPSUSUCREACION";
        public string FecModifica = "GPSFECMODIFICA";
        public string UsuModifica = "GPSUSUMODIFICA";
        public string Mensaje = "MENSAJE";
        public string Resultado = "RESULTADO";

        #endregion Mapeo de Campos


        #region SQL
        public string SqlListaEtapas
        {
            get { return base.GetSqlXml("GetListaEtapas"); }
        }
       
        public string SqlUltimoCodigoGenerado
        {
            get { return base.GetSqlXml("GetUltimoCodigoGenerado"); }
        }

        public string SqlValidarNombreEtapa
        {
            get { return base.GetSqlXml("ValidarNombreEtapa"); }
        }

        public string SqlSaveUpdate
        {
            get { return GetSqlXml("SaveUpdate"); }
        }
        #endregion SQL
    }
}
