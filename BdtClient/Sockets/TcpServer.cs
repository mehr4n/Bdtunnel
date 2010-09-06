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
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Bdt.Shared.Logs;
using Bdt.Client.Resources;
#endregion

namespace Bdt.Client.Sockets
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Serveur TCP de base
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class TcpServer : LoggedObject
    {

        #region " Constantes "
        public const int ACCEPT_POLLING_TIME = 50;
        #endregion

        #region " Attributs "
        private TcpListener m_listener;
        private ManualResetEvent m_mre = new ManualResetEvent(false);
        private IPAddress m_ip = IPAddress.Any;
        private int m_port;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'ip d'�coute
        /// </summary>
        /// -----------------------------------------------------------------------------
        public IPAddress Ip
        {
            get
            {
                return m_ip;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le port d'�coute
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Port
        {
            get
            {
                return m_port;
            }
        }
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="port">port local c�t� client</param>
        /// <param name="shared">bind sur toutes les ip/ip locale</param>
        /// -----------------------------------------------------------------------------
        public TcpServer(int port, bool shared)
        {
            m_ip = (IPAddress)(shared ? IPAddress.Any : IPAddress.Loopback);
            m_port = port;

            m_listener = new TcpListener(m_ip, m_port);
            Thread thr = new Thread(new System.Threading.ThreadStart(ServerThread));
            thr.Start();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Callback en cas de nouvelle connexion
        /// </summary>
        /// <param name="client">le socket client</param>
        /// -----------------------------------------------------------------------------
        protected abstract void OnNewConnection(TcpClient client);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fermeture de l'�coute
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void CloseServer()
        {
            m_mre.Set();
            m_listener.Stop();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Traitement principal du thread
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected void ServerThread()
        {
            try
            {
                m_listener.Start();
                while (!m_mre.WaitOne(ACCEPT_POLLING_TIME, false))
                {
                    try
                    {
                        TcpClient client = m_listener.AcceptTcpClient();
                        OnNewConnection(client);
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode != SocketError.Interrupted)
                        {
                            Log(ex.Message, ESeverity.ERROR);
                            Log(ex.ToString(), ESeverity.DEBUG);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message, ESeverity.ERROR);
                        Log(ex.ToString(), ESeverity.DEBUG);
                    }
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    Log(String.Format(Strings.TCP_SERVER_DISABLED, m_port), ESeverity.WARN);
                }
                else
                {
                    Log(ex.Message, ESeverity.ERROR);
                }
                Log(ex.ToString(), ESeverity.DEBUG);
            }
            catch (Exception ex)
            {
                Log(ex.Message, ESeverity.ERROR);
                Log(ex.ToString(), ESeverity.DEBUG);
            }
        }
        #endregion

    }

}

