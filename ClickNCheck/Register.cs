using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dummy.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ClickNCheck
{
    public static class Register
    {
        [FunctionName("Register")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route ="api/DummyModels/users/register")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Received a registration request.");
            HttpResponseMessage response=null;
            try
            {
                string passwordHash;
                string salt;
                var dummyModel = await req.Content.ReadAsAsync<DummyModel>();
                log.Info("Saving to database");
                if (dummyModel!=null)
                {
                    using (var db = new DummyContext())
                    {
                        salt = Salt.Create();
                        passwordHash = Hash.Create(dummyModel.Password, salt);
                        dummyModel.Password = passwordHash;
                        dummyModel.Salt = salt;
                        db.DummyModel.Add(dummyModel);
                        await db.SaveChangesAsync();
                    }
                    response = req.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    string errorMsg = "Error to parse user";
                    log.Error(errorMsg);
                    req.CreateErrorResponse(HttpStatusCode.BadRequest, errorMsg);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                req.CreateErrorResponse(HttpStatusCode.InternalServerError,ex);
            }

            return response;
        }
    }
}
