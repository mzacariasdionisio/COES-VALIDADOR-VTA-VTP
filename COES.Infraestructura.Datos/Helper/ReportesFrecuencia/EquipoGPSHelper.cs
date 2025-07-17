using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.ReportesFrecuencia
{
    public class EquipoGPSHelper : HelperBase
    {
        public EquipoGPSHelper() : base(Consultas.EquipoGPSSql)
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

        public EquipoGPSDTO Create(IDataReader dr)
        {
            EquipoGPSDTO entity = new EquipoGPSDTO();
             
            //potencia contrato
            entity.GPSCodi = Convert.ToInt32(valorReturn(dr, GPSCodi) ?? 0);
            entity.EmpresaCodi = Convert.ToInt32(valorReturn(dr, EmprCodi) ?? 0);
            entity.Empresa = valorReturn(dr, Empresa)?.ToString();
            entity.EquipoCodi = Convert.ToInt32(valorReturn(dr, EquiCodi) ?? 0);
            entity.NombreEquipo = valorReturn(dr, Nombre)?.ToString();
            entity.GPSOficial = valorReturn(dr, GPSOficial)?.ToString();
            entity.GPSDescOficial = valorReturn(dr, GPSDescOficial)?.ToString();
            entity.GPSOSINERG = valorReturn(dr, GPSOSINERG)?.ToString();
            entity.GPSEstado = valorReturn(dr, GPSEstado)?.ToString();
            entity.GPSDescEstado = valorReturn(dr, GPSDescEstado)?.ToString();
            entity.GPSTipo = valorReturn(dr, GPSTipo)?.ToString();
            entity.GPSGenAlarma = valorReturn(dr, GPSGenAlarma)?.ToString();
            entity.GPSDescGenAlarma = valorReturn(dr, GPSDescGenAlarma)?.ToString();
            entity.GPSFecRegi = ConvertToNull<DateTime>(valorReturn(dr, GPSFecCreacion));
            entity.GPSUsuarioRegi = valorReturn(dr, GPSUsuCreacion)?.ToString();
            entity.GPSFecAct = ConvertToNull<DateTime>(valorReturn(dr, GPSFecModifica));
            entity.GPSUsuarioAct = valorReturn(dr, GPSUsuModifica)?.ToString();
            entity.GPSRespaldo = valorReturn(dr, GPSRespaldo)?.ToString();
            entity.RutaFile = valorReturn(dr, RutaFile)?.ToString();

            return entity;
        }
        #region Mapeo de Campos


        //EquipoGPS
        public string GPSCodi = "GPSCODI";
        public string EmprCodi = "EMPRCODI";
        public string EquiCodi = "EQUICODI";
        public string Nombre = "NOMBRE";
        public string Empresa = "EMPRESA";
        public string GPSOficial = "GPSOFICIAL";
        public string GPSDescOficial = "OFICIAL";
        public string GPSOSINERG = "GPSOSINERG";
        public string GPSEstado = "GPSESTADO";
        public string GPSDescEstado = "ESTADO";
        public string GPSTipo = "GPSTIPO";
        public string GPSGenAlarma = "GPSGENALARMA";
        public string GPSDescGenAlarma = "GENALARMA";
        public string GPSFecCreacion = "GPSFECCREACION";
        public string GPSUsuCreacion = "GPSUSUCREACION";
        public string GPSFecModifica = "GPSFECMODIFICA";
        public string GPSUsuModifica = "GPSUSUMODIFICA";
        public string GPSRespaldo = "RESPALDO";
        public string Mensaje = "MENSAJE";
        public string Resultado = "RESULTADO";
        public string RutaFile = "RUTAFILE";

        #endregion Mapeo de Campos


        #region SQL
        public string SqlListaEquipoGPS
        {
            get { return base.GetSqlXml("GetListaEquipoGPS"); }
        }
        public string SqlListaEquipoGPSPorFiltro
        {
            get { return base.GetSqlXml("GetListaEquipoGPSPorFiltro"); }
        }

        public string SqlUltimoCodigoGenerado
        {
            get { return base.GetSqlXml("GetUltimoCodigoGenerado"); }
        }

        public string SqlNumRegistrosPorEquipo
        {
            get { return base.GetSqlXml("GetNumRegistrosPorEquipo"); }
        }

        public string SqlValidarNombreEquipoGPS
        {
            get { return base.GetSqlXml("ValidarNombreEquipoGPS"); }
        }

        public string SqlSaveUpdate
        {
            get { return GetSqlXml("SaveUpdate"); }
        }
        #endregion SQL
    }
}
