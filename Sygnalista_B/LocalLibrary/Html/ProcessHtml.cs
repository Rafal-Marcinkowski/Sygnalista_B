using Library;
using Sygnalista_B.MVVM.Models;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Sygnalista_B.LocalLibrary.Html;

public class ProcessHtml
{
    private readonly string[] captchaTexts = ["Why did this happen? Please make sure your browser supports JavaScript", "not a robous", "Are you a robot?"];

    private readonly List<string> tagsToIgnore =
    [
    "ALIOR", "BNPPPL", "CEZ", "CYFRPLSAT", "ENEA", "ENERGA",
    "ETF", "FIZ", "GMINA MIASTA", "HANDLOWY", "IIAAV", "INGBSK",
    "KRKA", "LEGATO ABSOLUTNEJ", "MBANK", "MILLENNIUM",
    "MOL", "ORANGEPL", "PGE", "PEKAO", "PKNORLEN", "PKOBP",
    "PZU", "SANPL", "SANTANDER", "SOPHARMA", "UNICREDIT"
    ];

    public async Task<Information> PrepareInformation(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        Information information = new();

        var item = doc.DocumentNode.SelectSingleNode("//li[@class='styles_item__akTBt']");

        if (item is not null)
        {
            var timeNode = item.SelectSingleNode(".//span[@class='styles_datetime__F3eFA']");
            var time = timeNode != null ? timeNode.InnerText.Trim() : string.Empty;

            var linkNode = item.SelectSingleNode(".//a");
            var link = linkNode != null ? linkNode.GetAttributeValue("href", string.Empty) : string.Empty;

            var fullText = linkNode != null ? linkNode.InnerText.Trim() : string.Empty;

            var parts = fullText.Split([':'], 2);
            var companyName = parts[0].Trim().Replace(".", "");
            var content = parts.Length > 1 ? parts[1].Trim() : string.Empty;

            information.Time = time;
            information.Link = link;
            information.Header = companyName;
            information.Content = content;

            _ = SaveTextToFile.SaveAsync("OstatniaInformacjaZHtml", $"{time} {companyName}: {content} | {link}");
        }

        return information;
    }

    public async Task<bool> IsForbiddenTags(string senderText) => tagsToIgnore.Any(q => q.Equals(senderText, StringComparison.OrdinalIgnoreCase));

    public bool IsCaptchaText(string senderText) => captchaTexts.Any(q => senderText.Contains(q));
}
