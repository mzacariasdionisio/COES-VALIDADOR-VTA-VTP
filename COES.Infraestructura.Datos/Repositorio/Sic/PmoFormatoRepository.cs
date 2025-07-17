using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data.Common;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PmoFormatoRepository : RepositoryBase, IPmoFormatoRepository
    {
        public PmoFormatoRepository(string strConn)
            : base(strConn)
        {
        }

        PmoFormatoHelper helper = new PmoFormatoHelper();
        public List<PmoFormatoDTO> List()
        {
            List<PmoFormatoDTO> entitys = new List<PmoFormatoDTO>();
            string queryString = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoFormatoDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener puntos de medicion por pmftabcodi
        /// </summary>
        /// <param name="pmftabcodi"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public List<PmoFormatoDTO> GetFormatPtomedicion(int pmftabcodi)
        {
            List<PmoFormatoDTO> entitys = new List<PmoFormatoDTO>();
            string queryString = string.Format(helper.SqlGetFormatPtomedicion, pmftabcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoFormatoDTO entity = new PmoFormatoDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iosinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iosinergcodi)) entity.Osinergcodi = Convert.ToString(dr.GetValue(iosinergcodi));

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
