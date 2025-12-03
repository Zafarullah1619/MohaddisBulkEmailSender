using System;
using System.Collections.Generic;
using System.Linq;
using Org.Utils;
using System.Data;
using Org.Business.Objects;
using Org.DataAccess;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Org.Business.Methods
{
    public class CommonBAL
    {
        public CommonBAL()
        {
        }

        public static async Task<bool> VerifyReCaptcha(string reCaptchaResponse, string SecretKey, string VerificationURL, string RemoteIP = null)
        {
            try
            {
                Dictionary<string, string> GoogleRequest = new Dictionary<string, string>();

                GoogleRequest.Add("secret", SecretKey);
                GoogleRequest.Add("response", reCaptchaResponse);

                if(!string.IsNullOrWhiteSpace(RemoteIP))
                {
                    GoogleRequest.Add("remoteip", RemoteIP);
                }
                
                var ResultString = await PostData(VerificationURL, GoogleRequest);

                if (ResultString != "FAILURE")
                {
                    GoogleReCaptchaResponse Response = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleReCaptchaResponse>(ResultString);

                    if(Response.Success)
                    {
                        return true;
                    }               
                }
            }
            catch(Exception)
            {
                return false;
            }

            return false;
        }

        protected static async Task<string> PostData(string IPNUrl, Dictionary<string, string> IPNValues)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    // HTTP POST
                    var content = new FormUrlEncodedContent(IPNValues);
                    HttpResponseMessage response = await client.PostAsync(new Uri(IPNUrl), content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch(Exception)
            {

            }
            return "FAILURE";
        }

                public static string RandomString(int noOfCharacter)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random r = new Random();
            string randonpassword = new string(
                Enumerable.Repeat(characters, noOfCharacter)
                          .Select(s => s[r.Next(s.Length)])
                          .ToArray());
            return randonpassword;
        }

        public static List<Country> GetCountries(string ConnectionString = "DefaultConnection")
        {
            try
            {
                using (IUnitOfWork uow = new UnitOfWork(ConnectionString))
                {
                    Filters Filter = new Filters();
                    IRepository<Country> oRepository = new Repository<Country>(uow.DataContext);
                    return oRepository.LoadSP(Filter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long GetMaxValue(string pColumnName, string pTableName)
        {
            try
            {

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    MaxValue oMax = new MaxValue();
                    Filters Filter = new Filters();
                    Filter.AddSqlParameters(() => oMax.ColumnName, pColumnName);
                    Filter.AddSqlParameters(() => oMax.TableName, pTableName);
                    IRepository<MaxValue> oRepository = new Repository<MaxValue>(uow.DataContext);

                    List<MaxValue> m = oRepository.LoadSP(Filter);
                    if (m.Count <= 0)
                    {
                        throw new Exception("Maximum Value could not be get");
                    }

                    if(m[0].MaxVal <= 0)
                    {
                        throw new Exception("Maximum Value is Zero");
                    }

                    return m[0].MaxVal;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public static async Task<string> SendEmailAsync(string FromEmail, string FromName, string ToEmail, string ToName, string Subject, string EmailText, string credentialUserName,string credentialPassword, string SMTPHost,string SMTPPort)
        {
            // Plug in your email service here to send an email.
            //return Task.FromResult(0);
            // convert IdentityMessage to a MailMessage
            // Credentials:
            //var credentialUserName = ConfigurationManager.AppSettings["SMTPCredentialsUser"];
            var sentFrom = FromName;
            var senderEmail = FromEmail;
            //var credentialPassword = ConfigurationManager.AppSettings["SMTPCredentialsPassword"];

            // Configure the client:
            SmtpClient client = new SmtpClient(SMTPHost);

             client.Port = Convert.ToInt32(SMTPPort);
             client.DeliveryMethod = SmtpDeliveryMethod.Network;
             client.UseDefaultCredentials = false;

             // Create the credentials:
             NetworkCredential credentials = new NetworkCredential(credentialUserName, credentialPassword);

             client.EnableSsl = true;
             client.Credentials = credentials;

             // Create the message:
             var mail = new MailMessage(new MailAddress(senderEmail, sentFrom), new MailAddress(ToEmail, ToName));

             mail.Subject = Subject;
             mail.Body = EmailText;
             mail.IsBodyHtml = true;

             try
             {
                 // Send:
                 await client.SendMailAsync(mail);
             }
             catch(Exception ex)
             {
                 throw ex;
             }

            return "SUCCESS";
        }

        public static async Task<HttpResponseMessage> PostFormUrlEncodedContent(string PostUrl, string PostRequestUri, List<KeyValuePair<string, string>> keyValues, string PostMethod = "POST")
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(PostUrl);

            HttpMethod httpMethod = HttpMethod.Post;

            if (PostMethod == "GET")
            {
                httpMethod = HttpMethod.Get;
            }

            var request = new HttpRequestMessage(httpMethod, PostRequestUri);

            request.Content = new FormUrlEncodedContent(keyValues);
            var response = await client.SendAsync(request);
            return response;
        }
        
        
        //GetGlobalSummaryStats
    }
}
