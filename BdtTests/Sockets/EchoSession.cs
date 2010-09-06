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
using System.Net.Sockets;
using System.Threading;

using Bdt.Shared.Logs;
using Bdt.Shared.Service;
using Bdt.Shared.Request;
using Bdt.Shared.Response;
#endregion

namespace Bdt.Tests.Sockets
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Session de test
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class EchoSession : LoggedObject
    {

        #region " Constantes "
        // La taille du buffer d'IO
        public const int BUFFER_SIZE = 65536;
        // La dur�e minimale entre deux tests de l'�tat de connexion
        public const int STATE_POLLING_MIN_TIME = 10;
        // La dur�e maximale entre deux tests de l'�tat de connexion
        public const int STATE_POLLING_MAX_TIME = 5000;
        // Le coefficient de d�c�l�ration,
        public const double STATE_POLLING_FACTOR = 1.1;
        // Le test de la connexion effective
        public const int SOCKET_TEST_POLLING_TIME = 100;
        #endregion

        #region " Attributs "
        protected TcpClient m_client;
        protected NetworkStream m_stream;
        protected ManualResetEvent m_mre = new ManualResetEvent(false);
        #endregion

        #region " M�thodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="client">le socket client pour la communication locale</param>
        /// -----------------------------------------------------------------------------
        public EchoSession(TcpClient client)
        {
            m_client = client;
            m_stream = client.GetStream();

            Thread thr = new Thread(new System.Threading.ThreadStart(CommunicationThread));
            thr.IsBackground = true;
            thr.Start();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gestion des erreurs
        /// </summary>
        /// <param name="ex">l'exception � g�rer</param>
        /// <param name="show">affichage du message d'erreur</param>
        /// -----------------------------------------------------------------------------
        protected void HandleError(Exception ex, bool show)
        {
            HandleError(ex.Message, show);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gestion des erreurs
        /// </summary>
        /// <param name="message">le message � g�rer</param>
        /// <param name="show">affichage du message d'erreur</param>
        /// -----------------------------------------------------------------------------
        protected void HandleError(string message, bool show)
        {
            if (show)
            {
                Log(message, ESeverity.ERROR);
            }
            m_mre.Set();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Calcule le temps d'attente n�cessaire entre deux traitements
        /// </summary>
        /// <param name="polltime">l'attente entre pollings</param>
        /// <param name="adjpolltime">ajustement (dur�e du dernier aller-retour</param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        protected int WaitTime(int polltime, int adjpolltime)
        {
            if (adjpolltime > polltime)
            {
                return 0;
            }
            else
            {
                return Math.Max(polltime - adjpolltime, STATE_POLLING_MIN_TIME);
            }
        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Traitement principal du thread
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected void CommunicationThread()
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            int polltime = STATE_POLLING_MIN_TIME;
            int adjpolltime = 0;

            while (!m_mre.WaitOne(WaitTime(polltime, adjpolltime), false))
            {
                DateTime startmarker = DateTime.Now;

                // Si des donn�es sont pr�sentes sur le socket, renvoi
                bool isConnected = false;
                bool isDataAvailAble = false;

                try
                {
                    isConnected = (!(m_client.Client.Poll(SOCKET_TEST_POLLING_TIME, System.Net.Sockets.SelectMode.SelectRead) && m_client.Client.Available == 0));
                    isDataAvailAble = m_stream.DataAvailable;
                }
                catch (Exception ex)
                {
                    HandleError(ex, false);
                }

                if (isConnected)
                {
                    if (isDataAvailAble)
                    {
                        int count = 0;
                        try
                        {
                            count = m_stream.Read(buffer, 0, BUFFER_SIZE);
                        }
                        catch (Exception ex)
                        {
                            HandleError(ex, true);
                        }
                        if (count > 0)
                        {
                            try
                            {
                                m_stream.Write(buffer, 0, count);
                                m_stream.Flush();
                            }
                            catch (Exception ex)
                            {
                                HandleError(ex, true);
                            }
                            // Si des donn�es sont pr�sentes, on repasse en mode 'actif'
                            polltime = STATE_POLLING_MIN_TIME;
                        }
                    }
                    else
                    {
                        // Sinon on augmente le temps de latence
                        polltime = Convert.ToInt32(Math.Round(STATE_POLLING_FACTOR * polltime));
                        polltime = Math.Min(polltime, STATE_POLLING_MAX_TIME);
                    }
                }
                else
                {
                    // Deconnexion
                    m_mre.Set();
                }
                adjpolltime = Convert.ToInt32(DateTime.Now.Subtract(startmarker).TotalMilliseconds);
            }
            Disconnect();
        }
       

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Deconnexion
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected void Disconnect()
        {
            if (m_client != null)
            {
                m_stream.Close();
                m_client.Close();
                m_stream = null;
                m_client = null;
            }
        }
        #endregion

    }

}

