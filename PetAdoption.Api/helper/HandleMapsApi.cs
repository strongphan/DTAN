using PetAdoption.Shared;
using System.Text.Json;

namespace PetAdoption.Api.helper
{
    public class HandleMapsApi
    {
        public async Task<Double> GetDistance(string addressFrom, string addressTo)
        {
            try
            {
                using var client = new HttpClient();

                var requestUrl = $"https://dev.virtualearth.net/REST/v1/Routes/Driving?wp.0={addressFrom}&wp.1={addressTo}&key={AppConstants.BingMapsApiKey}";
                var directionsRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                directionsRequest.Headers.Add("Accept", "application/json");

                var response = await client.SendAsync(directionsRequest);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var directionsJson = await response.Content.ReadAsStringAsync();

                        JsonDocument document = JsonDocument.Parse(directionsJson);
                        if (document.RootElement.TryGetProperty("resourceSets", out JsonElement resourceSets) && resourceSets.GetArrayLength() > 0)
                        {
                            JsonElement resourceSet = resourceSets[0];
                            if (resourceSet.TryGetProperty("resources", out JsonElement resources) && resources.GetArrayLength() > 0)
                            {
                                JsonElement resource = resources[0];
                                if (resource.TryGetProperty("travelDistance", out JsonElement travelDistance))
                                {
                                    return travelDistance.GetDouble();
                                    // Use the distance value as needed
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                            else
                            {
                                return 0;
                                // Handle missing resources
                            }
                        }
                        else
                        {
                            return 0;
                            // Handle missing resourceSets
                        }
                    }
                    catch (Exception ex)
                    {
                        return 0;
                        // Handle parsing errors or other exceptions

                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
                // Handle potential exceptions during API calls
            }
        }
    }
}
