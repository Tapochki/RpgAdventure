using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GoogleSheetsService
{
    private SheetsService _service;
    private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    private static readonly string ApplicationName = "RpgAdveture";

    public GoogleSheetsService()
    {
        GoogleCredential credential;
        string credentialsPath = (Application.streamingAssetsPath + "/credentials.json");
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
        }
        _service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public async Task<IList<IList<object>>> GetSheetData(string spreadsheetId, string range)
    {
        try
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = _service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = await request.ExecuteAsync();
            return response.Values;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching data: {ex.Message}");
            return null;
        }
    }
}
