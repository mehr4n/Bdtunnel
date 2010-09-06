/* BoutDuTunnel Copyright (c) 2007-2010 Sebastien LEBRETON

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

#region " Inclusions "
using System;
#endregion

namespace Bdt.Shared.Response
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une connexion au sein du tunnel
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct Connection 
    {

        #region " Attributs "
        private string m_address;
        private string m_host;
        private int m_port;
        private DateTime m_lastAccess;
        private string m_cid;
        private int m_readcount;
        private int m_writecount;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le nombre d'octets lus
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int ReadCount 
        {
            get
            {
                return m_readcount;
            }
            set
            {
                m_readcount = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le nombre d'octets �crits
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int WriteCount
        {
            get
            {
                return m_writecount;
            }
            set
            {
                m_writecount = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'adresse distante
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Address
        {
            get
            {
                return m_address;
            }
            set
            {
                m_address = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le h�te distant
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Host
        {
            get
            {
                return m_host;
            }
            set
            {
                m_host = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le port distant
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Port
        {
            get
            {
                return m_port;
            }
            set
            {
                m_port = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La date de dernier acc�s
        /// </summary>
        /// -----------------------------------------------------------------------------
        public DateTime LastAccess
        {
            get
            {
                return m_lastAccess;
            }
            set
            {
                m_lastAccess = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le jeton de connexion
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Cid
        {
            get
            {
                return m_cid;
            }
            set
            {
                m_cid = value;
            }
        }
        #endregion

    }

}
