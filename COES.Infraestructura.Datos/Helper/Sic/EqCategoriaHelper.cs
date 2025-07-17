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
    /// Clase que contiene el mapeo de la tabla EQ_CATEGORIA
    /// </summary>
    public class EqCategoriaHelper : HelperBase
    {
        public EqCategoriaHelper() : base(Consultas.EqCategoriaSql) { }

        #region Mapeo de Campos
        public string Ctgcodi = "CTGCODI";
        public string Ctgpadre = "CTGPADRE";
        public string Famcodi = "FAMCODI";
        public string Ctgnomb = "CTGNOMB";
        public string CtgFlagExcluyente = "CTGFLAGEXCLUYENTE";
        public string Ctgestado = "CTGESTADO";

        //inicio modificado
        public string UsuarioCreacion = "CTGUSUCREACION";
        public string FechaCreacion = "CTGFECCREACION";
        public string UsuarioUpdate = "CTGUSUMODIFICACION";
        public string FechaUpdate = "CTGFECMODIFICACION";
        //fin modificado

        public string Famnomb = "FAMNOMB";
        public string Ctgpadrenomb = "CTGPADRENOMB";
        public string TotalDetalle = "CANT_DET";
        //inicio agregado
        public string TotalHijo = "CANT_HIJO";
        public string TotalEquipo = "CANT_EQUIPO";
        //fin agregado
        #endregion

        public EqCategoriaDTO Create(IDataReader dr)
        {
            EqCategoriaDTO entity = new EqCategoriaDTO();

            int iCtgcodi = dr.GetOrdinal(this.Ctgcodi);
            if (!dr.IsDBNull(iCtgcodi)) entity.Ctgcodi = Convert.ToInt32(dr.GetValue(iCtgcodi));
            int iCtgpadre = dr.GetOrdinal(this.Ctgpadre);
            if (!dr.IsDBNull(iCtgpadre)) entity.Ctgpadre = Convert.ToInt32(dr.GetValue(iCtgpadre));
            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iCtgcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
            int iCtgnomb = dr.GetOrdinal(this.Ctgnomb);
            if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);
            int iCtgFlagExcluyente = dr.GetOrdinal(this.CtgFlagExcluyente);
            if (!dr.IsDBNull(iCtgFlagExcluyente)) entity.CtgFlagExcluyente = dr.GetString(iCtgFlagExcluyente);
            int iCtgestado = dr.GetOrdinal(this.Ctgestado);
            if (!dr.IsDBNull(iCtgestado)) entity.Ctgestado = dr.GetString(iCtgestado);

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

        public string SqlListPadre
        {
            get { return base.GetSqlXml("ListPadre"); }
        }

        public string SqlListByFamiliaAndEstado
        {
            get { return base.GetSqlXml("ListByFamiliaAndEstado"); }
        }

        public string SqlListCategoriaClasificacion
        {
            get { return base.GetSqlXml("ListCategoriaClasificacion"); }
        }

        public string SqlListCategoriaHijoByIdPadre
        {
            get { return base.GetSqlXml("ListCategoriaHijoByIdPadre"); }
        }

        //inicio agregado
        public string SqlListCategoriaHijoByIdPadreAndEmpresa
        {
            get { return base.GetSqlXml("ListCategoriaHijoByIdPadreAndEmpresa"); }
        }
        //fin agregado
    }
}
