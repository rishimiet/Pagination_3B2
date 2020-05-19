using Apache.NMS;
using Apache.NMS.Util;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pagination_3B2
{
    class JMSProducer
    {
        public void sendMessageToDistiller(string txt, string msgg)
        {
            //Create the Connection factory
            try
            {
                string url = ConfigurationManager.AppSettings["queueURL"].ToString();
                string queueName = ConfigurationManager.AppSettings["queueName"].ToString();

                IConnectionFactory factory = new ConnectionFactory(url);

                using (IConnection connection = factory.CreateConnection())
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, queueName);
                    Console.WriteLine("Using destination: " + destination);

                    // Create a producer                
                    using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        // Start the connection so that messages will be processed.
                        connection.Start();
                        IMapMessage msg = producer.CreateMapMessage();
                        string fileName = txt;
                        string[] arrStr = txt.Split(',');

                        msg.Body.SetString("FILENAME", txt);
                        msg.Body.SetString("STAGE", msgg);
                        //msg.Body.SetString("MESSAGE", msgg);
                        producer.Send(msg);
                        Console.WriteLine("Message Send for " + txt);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
