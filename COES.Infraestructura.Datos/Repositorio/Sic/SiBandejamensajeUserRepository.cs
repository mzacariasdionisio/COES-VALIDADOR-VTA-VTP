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
    /// Clase de acceso a datos de la tabla SI_BANDEJAMENSAJE_USER
    /// </summary>
    public class SiBandejamensajeUserRepository: RepositoryBase, ISiBandejamensajeUserRepository
    {
        public SiBandejamensajeUserRepository(string strConn): base(strConn)
        {
        }

        SiBandejamensajeUserHelper helper = new SiBandejamensajeUserHelper();

        public int SaveCarpeta(string nomCarpeta, string usuario, DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            string query = string.Format(helper.SqlSave, id, nomCarpeta, usuario, fecha.ToString(ConstantesBase.FormatoFechaHora));
            command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteReader(command);

            return id;
        }


        public List<SiBandejamensajeUserDTO> listaCantEnCarpetaPorModYUser(int modcodi, string usuario, string correo)
        {
            List<SiBandejamensajeUserDTO> entitys = new List<SiBandejamensajeUserDTO>();

            string query = string.Format(helper.SqllistaCantEnCarpetaPorModYUser, modcodi, usuario, correo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            /* SiMensajeDTO entity;*/

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                SiBandejamensajeUserDTO entity;
                while (dr.Read())
                {
                    entity = new SiBandejamensajeUserDTO();

                    int iBandcodi = dr.GetOrdinal(helper.Bandcodi);
                    if (!dr.IsDBNull(iBandcodi)) entity.Bandcodi = Convert.ToInt32(dr.GetValue(iBandcodi));

                    int iBandnombre = dr.GetOrdinal(helper.Bandnombre);
                    if (!dr.IsDBNull(iBandnombre)) entity.Bandnombre = dr.GetString(iBandnombre);

                    int iBandusucreacion = dr.GetOrdinal(helper.Bandusucreacion);
                    if (!dr.IsDBNull(iBandusucreacion)) entity.Bandusucreacion = dr.GetString(iBandusucreacion);

                    int iCantidad = dr.GetOrdinal(helper.Cantidad);
                    if (!dr.IsDBNull(iCantidad)) entity.Cantidad = dr.GetInt32(iCantidad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
