using Library;

namespace Sygnalista_B.LocalLibrary.Services;

public class CompanyInfoManager : BindableBase
{
    public string CompanyCode { get; private set; } = string.Empty;
    public bool ForceAutoScript { get; set; } = true;

    private string companyName = string.Empty;
    public string CompanyName
    {
        get => companyName;
        set => SetProperty(ref companyName, value);
    }

    private string medianaText = string.Empty;
    public string MedianaText
    {
        get => medianaText;
        set => SetProperty(ref medianaText, value);
    }

    private string capitalization = string.Empty;
    public string Capitalization
    {
        get => capitalization;
        set => SetProperty(ref capitalization, value);
    }

    public void Clear()
    {
        CompanyName = string.Empty;
        MedianaText = string.Empty;
        Capitalization = string.Empty;
        CompanyCode = string.Empty;
    }

    public async Task CreateCompanyCode(string header)
    {
        CompanyCode = await NameTranslation.ConvertHeaderToTranslation(header);
        _ = SaveTextToFile.SaveAsync("CompanyCode", CompanyCode);
    }

    public async Task CreateMedianaText()
    {
        MedianaText = await NameTranslation.GetTurnoverMedianForCompany(CompanyCode);
    }

    public bool HasValidCompanyCode() => !string.IsNullOrEmpty(CompanyCode);
}
