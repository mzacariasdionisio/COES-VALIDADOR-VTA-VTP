using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla DOC_DIA_ESP
    /// </summary>
    public class DocFlujoRepository: RepositoryBase, IDocFlujoRepository
    {
        public DocFlujoRepository(string strConn): base(strConn)
        {
        }

        DocFlujoHelper helper = new DocFlujoHelper();

        public List<DocFlujoDTO> ListEstad(DateTime fechainicio,DateTime fechafin, string listaTipoAtencion)
        {
            List<DocFlujoDTO> entitys = new List<DocFlujoDTO>();
            string query = string.Format(helper.SqlListEstad, fechainicio.ToString(ConstantesBase.FormatoFecha),fechafin.ToString(ConstantesBase.FormatoFecha),listaTipoAtencion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<DocFlujoDTO> GetAreasResponsables(int fljcodi, string listaTipoAtencion)
        {
            List<DocFlujoDTO> entitys = new List<DocFlujoDTO>();
            string query = string.Format(helper.SqlListAreaResponsable, fljcodi, listaTipoAtencion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateArea(dr));
                }
            }

            return entitys;
        }

        public String GetDocRespuesta(int fljcodiref)
        {
            string query = string.Format(helper.SqlGetDocRespuesta, fljcodiref);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            DocFlujoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            if (entity != null) return entity.Fljnumext;
            else return null;
        }

        #region MigracionSGOCOES-GrupoB
        /// <summary>
        /// Lista los documentos de costos variables
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<DocFlujoDTO> ListDocCV(DateTime fechaIni, DateTime fechaFin)
        {
            List<DocFlujoDTO> entitys = new List<DocFlujoDTO>();
            string query = string.Format(helper.SqlListDocCV, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DocFlujoDTO entity = new DocFlujoDTO();

                    int iFljfecharecep = dr.GetOrdinal(this.helper.Fljfecharecep);
                    if (!dr.IsDBNull(iFljfecharecep)) entity.Fljfecharecep = dr.GetDateTime(iFljfecharecep);

                    int iFljfechaorig = dr.GetOrdinal(this.helper.Fljfechaorig);
                    if (!dr.IsDBNull(iFljfechaorig)) entity.Fljfechaorig = dr.GetDateTime(iFljfechaorig);

                    int iFljnombre = dr.GetOrdinal(this.helper.Fljnombre);
                    if (!dr.IsDBNull(iFljnombre)) entity.Fljnombre = dr.GetString(iFljnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
