namespace Library.BiznesRadar;

public class DownloadHtml
{
    public async static Task<string> DownloadHtmlAsync(string name)
    {
        string url = $"https://www.biznesradar.pl/raporty-finansowe-rachunek-zyskow-i-strat/{name},Q";
        return await DownloadHtmlWithRedirectAsync(url);
    }

    private async static Task<string> DownloadHtmlWithRedirectAsync(string url)
    {
        HttpClientHandler handler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };

        using HttpClient client = new(handler);
        using HttpResponseMessage response = await client.GetAsync(url);
        string requestUri = response.RequestMessage.RequestUri.ToString();

        if (requestUri != url)
        {
            return await DownloadHtmlWithRedirectAsync($"{requestUri},Q");
        }

        using HttpContent content = response.Content;
        var html = await content.ReadAsStringAsync();

        return html;
    }
}
