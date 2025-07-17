using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_CATEGORIA_EQUIPO
    /// </summary>
    public class EqCategoriaEquipoHelper : HelperBase
    {
        public EqCategoriaEquipoHelper() : base(Consultas.EqCategoriaEquipoSql) { }

        #region Mapeo de Campos
        public string Ctgdetcodi = "CTGDETCODI";
        public string Equicodi = "EQUICODI";
        public string Ctgequiestado = "CTGEQUIESTADO";
        public string UsuarioCreacion = "CTGEQUIUSUCREACION";
        public string FechaCreacion = "CTGEQUIFECCREACION";
        public string UsuarioUpdate = "CTGEQUIUSUMODIFICACION";
        public string FechaUpdate = "CTGEQUIFECMODIFICACION";

        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Equiabrev = "EQUIABREV";
        public string Equinomb = "EQUINOMB";
        public string Ctgcodi = "CTGCODI";
        public string Ctgnomb = "CTGNOMB";
        public string Ctgpadrenomb = "CTGPADRENOMB";
        public string Ctgdetnomb = "CTGDETNOMB";
        public string CtgFlagExcluyente = "CTGFLAGEXCLUYENTE";
        public string CtgdetcodiOld = "CTGDETCODIOLD";
        public string EquicodiOld = "EQUICODIOLD";

        #region SIOSEIN
        public string Grupocodi = "GRUPOCODI";
        #endregion

        #endregion

        public EqCategoriaEquipoDTO Create(IDataReader dr)
        {
            EqCategoriaEquipoDTO entity = new EqCategoriaEquipoDTO();

            int iCtgdetcodi = dr.GetOrdinal(this.Ctgdetcodi);
            if (!dr.IsDBNull(iCtgdetcodi)) entity.Ctgdetcodi = Convert.ToInt32(dr.GetValue(iCtgdetcodi));
            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
            int iCtgequiestado = dr.GetOrdinal(this.Ctgequiestado);
            if (!dr.IsDBNull(iCtgequiestado)) entity.Ctgequiestado = dr.GetString(iCtgequiestado);

            int iUsuarioCreacion = dr.GetOrdinal(this.UsuarioCreacion);
            if (!dr.IsDBNull(iUsuarioCreacion)) entity.UsuarioCreacion = dr.GetString(iUsuarioCreacion);
            int iFechaCreacion = dr.GetOrdinal(this.FechaCreacion);
            if (!dr.IsDBNull(iFechaCreacion)) entity.FechaCreacion = Convert.ToDateTime(dr.GetValue(iFechaCreacion));
            int iUsuarioUpdate = dr.GetOrdinal(this.UsuarioUpdate);
            if (!dr.IsDBNull(iUsuarioUpdate)) entity.UsuarioUpdate = dr.GetString(iUsuarioUpdate);
            int iFechaUpdate = dr.GetOrdinal(this.FechaUpdate);
            if (!dr.IsDBNull(iFechaUpdate)) entity.FechaUpdate = Convert.ToDateTime(dr.GetValue(iFechaUpdate));

            return entity;
        }

        public string SqlListPaginadoClasificacion
        {
            get { return base.GetSqlXml("ListPaginadoClasificacion"); }
        }

        public string SqlTotalListadoClasificacion
        {
            get { return base.GetSqlXml("ListTotalPaginadoClasificacion"); }
        }

        public string SqlListClasificacionByCategoriaAndEquipo
        {
            get { return base.GetSqlXml("ListClasificacionByCategoriaAndEquipo"); }
        }

        public string SqlListClasificacionByCategoriaAndEmpresa
        {
            get { return base.GetSqlXml("ListClasificacionByCategoriaAndEmpresa"); }
        }

        public string SqlListClasificacionByCategoriaPadreAndEquipo
        {
            get { return base.GetSqlXml("ListClasificacionByCategoriaPadreAndEquipo"); }
        }

        public string SqlListClasificacionByCategoriaDetalle
        {
            get { return base.GetSqlXml("ListClasificacionByCategoriaDetalle"); }
        }
        public string SqlGetByIdEquipo
        {
            get { return base.GetSqlXml("GetByIdEquipo"); }
        }
    }
}
