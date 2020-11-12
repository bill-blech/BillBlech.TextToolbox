using RestSharp;
using RestSharp.Serialization.Json;

namespace BillBlech.TextToolbox.Activities.Activities
{
    //https://www.youtube.com/watch?v=S5XQwUja02Y&t=1288s

    public class textApiSentiment
    {

        public static string ReturnTextSentiment(string inputText)
        {
            //https://text-processing.com/docs/sentiment.html

            //Start the API
            RestClient restClient = new RestClient("http://text-processing.com/api/sentiment/");
            RestRequest restRequest = new RestRequest(Method.POST);

            restRequest.AddParameter("text", inputText);
            IRestResponse restResponse = restClient.Execute(restRequest);

            //Check for errors
            if (restResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return ("There was an error: " + restResponse.Content);
            }
            else
            {

                //Deserialize Json Results
                DadosRetorno dadosRetorno = new JsonDeserializer().Deserialize<DadosRetorno>(restResponse);

                //Return the Result
                return dadosRetorno.label;

            }
        }
    }

    public class DadosRetorno
    {
        public string label { get; set; }
    }

}

