// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
#endregion

namespace Bdt.Shared.Request
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une demande d'écriture
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct WriteRequest : IGenericRequest
    {

        #region " Attributs "
        private byte[] m_data;
        private int m_cid;
        private int m_uid;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le jeton de connexion
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Cid
        {
            get
            {
                return m_cid;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le jeton utilisateur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Uid
        {
            get
            {
                return m_uid;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Les données à écrire
        /// </summary>
        /// -----------------------------------------------------------------------------
        public byte[] Data
        {
            get
            {
                return m_data;
            }
        }
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="uid">Le jeton utilisateur</param>
        /// <param name="cid">Le jeton de connexion</param>
        /// <param name="data">Les données à écrire</param>
        /// -----------------------------------------------------------------------------
        public WriteRequest(int uid, int cid, byte[] data)
        {
            this.m_uid = uid;
            this.m_cid = cid;
            this.m_data = data;
        }
        #endregion

    }

}

