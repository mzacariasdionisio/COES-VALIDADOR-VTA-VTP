using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Data;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.ReportesFrecuencia
{
    public class CargaVirtualHelper : HelperBase
    {
        public CargaVirtualHelper() : base(Consultas.CargaVirtualSql)
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

        public CargaVirtualDTO Create(IDataReader dr)
        {
            CargaVirtualDTO entity = new CargaVirtualDTO();

            entity.IdCarga = Convert.ToInt32(valorReturn(dr, IdCarga) ?? 0);
            entity.NombreEmpresa = valorReturn(dr, Emprnomb)?.ToString();
            entity.CodCentral = valorReturn(dr, Central)?.ToString();
            entity.NombreEquipo = valorReturn(dr, GPSNomb)?.ToString();
            entity.GPSCodi = Convert.ToInt32(valorReturn(dr, GPSCodi) ?? 0);
            entity.TipoCarga = valorReturn(dr, TipoCarga)?.ToString();
            entity.ArchivoCarga = valorReturn(dr, ArchivoCarga)?.ToString();
            entity.FechaCargaIni = valorReturn(dr, FechaCargaIni)?.ToString();
            entity.FechaCargaFin = valorReturn(dr, FechaCargaFin)?.ToString();
            entity.FechaCreacion = valorReturn(dr, FechaCreacion)?.ToString();
            entity.UsuCreacion = valorReturn(dr, UsuCreacion)?.ToString();
            entity.NombreUnidad = valorReturn(dr, NombreUnidad)?.ToString();
            return entity;
        }

        public LecturaVirtualDTO CreateLecturaVirtual(IDataReader dr)
        {
            LecturaVirtualDTO entity = new LecturaVirtualDTO();
            entity.IdCarga = Convert.ToInt32(valorReturn(dr, IdCarga) ?? 0);
            entity.FecHora = valorReturn(dr, FechaHora)?.ToString();
            entity.Frecuencia = Convert.ToDecimal(valorReturn(dr, Frecuencia) ?? 0);
            entity.Tension= Convert.ToDecimal(valorReturn(dr, Tension) ?? 0);
            return entity;
        }

        public SiEmpresaDTO CreateEmpresa(IDataReader dr)
        {
            SiEmpresaDTO entity = new SiEmpresaDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            return entity;
        }

        public CentralDTO CreateCentral(IDataReader dr)
        {
            CentralDTO entity = new CentralDTO();

            int iEquiNomb = dr.GetOrdinal(this.EquiNomb);
            if (!dr.IsDBNull(iEquiNomb)) entity.NombreCentral = dr.GetString(iEquiNomb);

            return entity;
        }


        public UnidadDTO CreateUnidad(IDataReader dr)
        {
            UnidadDTO entity = new UnidadDTO();

            int iUnidadCodi = dr.GetOrdinal(this.UnidadCodi);
            if (!dr.IsDBNull(iUnidadCodi)) entity.IdUnidad = Convert.ToInt32(dr.GetValue(iUnidadCodi));

            int iUnidadnomb = dr.GetOrdinal(this.UnidadNomb);
            if (!dr.IsDBNull(iUnidadnomb)) entity.NombreUnidad = dr.GetString(iUnidadnomb);

            return entity;
        }

        #region Mapeo de Campos


        //CargaVirtual
        public string IdCarga = "IDCARGA";
        public string GPSCodi = "GPSCODI";
        public string GPSNomb = "GPSNOMB";
        public string TipoCarga = "TIPOCARGA";
        public string Central = "CENTRAL";
        public string UnidadCod = "UNIDADCODI";
        public string ArchivoCarga = "ARCHIVOCARGA";
        public string FechaCargaIni = "FECHACARGAINI";
        public string FechaCargaFin = "FECHACARGAFIN";
        public string UsuCreacion = "USUCREACION";
        public string NombreUnidad = "NOMBREUNIDAD";
        public string DataCarga = "DATACARGA";
        public string Mensaje = "MENSAJE";
        public string Resultado = "RESULTADO";
        public string FechaCreacion = "FECHACREACION";

        public static string TableNameEmpresa = "SI_EMPRESA";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";

        public string EquiNomb = "EQUINOMB";

        public string CentralNomb = "CENTRALNOMB";

        public string UnidadCodi = "CODUNIDAD";
        public string UnidadNomb = "UNIDADNOMB";

        public string FechaHora = "FECHAHORA";
        public string Frecuencia = "FRECUENCIA";
        public string Tension = "TENSION";

        #endregion Mapeo de Campos


        #region SQL
        public string SqlListaCargaVirtual
        {
            get { return base.GetSqlXml("GetListaCargaVirtual"); }
        }
        public string SqlListaLecturaVirtual
        {
            get { return base.GetSqlXml("GetListaLecturaVirtual"); }
        }
        public string SqlListaEmpresasCargaVirtual
        {
            get { return base.GetSqlXml("GetListaEmpresasCargaVirtual"); }
        }

        public string SqlListaCentralPorEmpresa
        {
            get { return base.GetSqlXml("GetListaCentralPorEmpresa"); }
        }

        public string SqlListaUnidadPorCentralEmpresa
        {
            get { return base.GetSqlXml("GetListaUnidadPorCentralEmpresa"); }
        }

        public string SqlSaveUpdate
        {
            get { return GetSqlXml("SaveUpdate"); }
        }

        public string SqlSaveLecturaVirtual
        {
            get { return GetSqlXml("SaveLecturaVirtual"); }
        }
        public string SqlSaveLectura
        {
            get { return GetSqlXml("SaveLectura"); }
        }
        #endregion SQL
    }
}
