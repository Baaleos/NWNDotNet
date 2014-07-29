using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace AsmodeiServices
{
    public static class AsmodeiServices
    {
        //http://localhost/Services/RhunServices.aspx?service=link_acc&pin=969361&gameaccount=Baaleos
        public static string GetRequest(string sURL)
        {
            HttpWebRequest wRequest;
            HttpWebResponse response;
            Stream resStream;
            StreamReader reader;
            try
            {
                wRequest = (HttpWebRequest)WebRequest.Create(sURL);
                // execute the request
                response = (HttpWebResponse)
                    wRequest.GetResponse();
                // we will read data via the response stream
                resStream = response.GetResponseStream();
                reader = new StreamReader(resStream);
                string sText = reader.ReadToEnd();
                //MessageBox.Show(sText);
                return sText;
            }
            catch (Exception e)
            {
                return "Error:" + e.ToString();
            }
            finally
            {
                wRequest = null;
                response = null;
                resStream = null;
                reader = null;
            }

        }



        public static string KillServerClean()
        {
            string sReturn = "SUCCESS";
            System.Diagnostics.Process[] pArray = System.Diagnostics.Process.GetProcessesByName("nwserver");
            foreach (System.Diagnostics.Process process in pArray)
            {
                try
                {
                    process.Kill();
                }
                catch (Exception ee)
                {
                    sReturn += ee.ToString();
                }
            }

            return sReturn;
        }

        private static void _start()
        {
            try
            {
                
            }
            catch (Exception ee)
            {
               // MessageBox.Show(ee.ToString());
            }
        }

        public static string StartListener(string sNULL)
        {
            try
            {
                //MessageBox.Show("Starting!!");
                NWNListener listener = new NWNListener();
                return "DONE";
            }
            catch (Exception ee)
            {
                //MessageBox.Show(ee.ToString());
                return "FAILURE:" + ee.ToString();
            }
            
        }


    
    }
}