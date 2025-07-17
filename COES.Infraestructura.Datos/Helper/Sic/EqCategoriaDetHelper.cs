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
    /// Clase que contiene el mapeo de la tabla EQ_CATEGORIA_DETALLE
    /// </summary>
    public class EqCategoriaDetHelper : HelperBase
    {
        public EqCategoriaDetHelper() : base(Consultas.EqCategoriaDetSql) { }

        #region Mapeo de Campos
        public string Ctgdetcodi = "CTGDETCODI";
        public string Ctgcodi = "CTGCODI";
        public string Ctgdetnomb = "CTGDETNOMB";
        public string Ctgdetestado = "CTGDETESTADO";

        //inicio modificado
        public string UsuarioCreacion = "CTGDETUSUCREACION";
        public string FechaCreacion = "CTGDETFECCREACION";
        public string UsuarioUpdate = "CTGDETUSUMODIFICACION";
        public string FechaUpdate = "CTGDETFECMODIFICACION";
        //fin modificado

        public string Famnomb = "FAMNOMB";
        public string Ctgnomb = "CTGNOMB";
        public string Ctgpadrecodi = "CTGPADRECODI";
        public string Ctgpadrenomb = "CTGPADRENOMB";
        //inicio agregado
        public string TotalEquipo = "TOTAL_EQUIPO";
        //fin agregado
        #endregion
        
        public EqCategoriaDetDTO Create(IDataReader dr)
        {
            EqCategoriaDetDTO entity = new EqCategoriaDetDTO();

            int iCtgdetcodi = dr.GetOrdinal(this.Ctgdetcodi);
            if (!dr.IsDBNull(iCtgdetcodi)) entity.Ctgdetcodi = Convert.ToInt32(dr.GetValue(iCtgdetcodi));
            int iCtgcodi = dr.GetOrdinal(this.Ctgcodi);
            if (!dr.IsDBNull(iCtgcodi)) entity.Ctgcodi = Convert.ToInt32(dr.GetValue(iCtgcodi));
            int iCtgdetnomb = dr.GetOrdinal(this.Ctgdetnomb);
            if (!dr.IsDBNull(iCtgdetnomb)) entity.Ctgdetnomb = dr.GetString(iCtgdetnomb);
            int iCtgdetestado = dr.GetOrdinal(this.Ctgdetestado);
            if (!dr.IsDBNull(iCtgdetestado)) entity.Ctgdetestado = dr.GetString(iCtgdetestado);

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


        public string SqlListByCategoriaAndEstado
        {
            get { return base.GetSqlXml("ListByCategoriaAndEstado"); }
        }

        //inicio agregado
        public string SqlListByCategoriaAndEstadoAndEmpresa
        {
            get { return base.GetSqlXml("ListByCategoriaAndEstadoAndEmpresa"); }
        }
        //fin agregado
    }
}
