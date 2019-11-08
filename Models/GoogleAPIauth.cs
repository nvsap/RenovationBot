using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using File = Google.Apis.Drive.v3.Data.File;

namespace RenovationBot.Models
{
    public class GoogleAPIauth
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string[] scopesdr = new string[] { DriveService.Scope.Drive,
                           DriveService.Scope.DriveAppdata,
                           DriveService.Scope.DriveFile,
                           DriveService.Scope.DriveMetadataReadonly,
                           DriveService.Scope.DriveReadonly,
                           DriveService.Scope.DriveScripts };
        static string ApplicationName = "RenovationProjectkyiv";
        static string MainFolderId = "13_VFdZ7fPD4cPeyMfHxASNtdIs39gBVs";
        static string MainTableId = "1magz22zOaxTCi4qJgy9t3CyA-HkwuGKWjdLLCDEcSyQ";
        static string SheetRange = "'Sheet1'!A1:E";


        public static void UploadNewBuilding(string[] data, List<string> photoIds, TelegramBotClient botClient)
        {
            var driveCredentials = GetUserCredentialDrive();
            var sheetCredintials = GetUserCredentialSheet();

            var driveService = GetDriveService(driveCredentials);
            var sheetService = GetSheetsService(sheetCredintials);

            string foldLink = UploadFilesToDrive(driveService, MainFolderId, photoIds, botClient, driveService, sheetService);

            var sheetData = new List<object> { data[0], data[1], data[2], foldLink };

            FillSheetData(sheetService, MainTableId, sheetData);
        }

        public static UserCredential GetUserCredentialSheet()
        {
            UserCredential credential;
            using (var stream = new FileStream("Sheet.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("RenovationBot/token.json")).Result;
            }
            return credential;
        }
        public static UserCredential GetUserCredentialDrive()
        {
            UserCredential credential;
            using (var stream = new FileStream("drive.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "tokendr.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopesdr,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("RenovationBot/token1.json")).Result;
            }
            return credential;
        }
        private static SheetsService GetSheetsService(UserCredential credential)
        {
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }
        private static DriveService GetDriveService(UserCredential credential)
        {
            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }
        private static string UploadFilesToDrive(DriveService service, string folderName, List<string> filesId, TelegramBotClient botClient, DriveService driveService, SheetsService sheetsService)
        {
            var file = new File();
            file.Name = folderName;
            file.MimeType = "application/vnd.google-apps.folder";

            var request = service.Files.Create(file);
            request.Fields = "id";

            var result = request.Execute();

            foreach(var id in filesId)
            {
                UploadFileToDrive(driveService, @"application/vnd.google-apps.file", botClient, id, result.Id);
            }

            return result.WebViewLink;
        }
        private static async void UploadFileToDrive(DriveService service, string contentType, TelegramBotClient botClient, string fileId, string folderId)
        {
            var fileMetadata = new File
            {
                Name = fileId,
                Parents = new List<string> { folderId }
            };
            FilesResource.CreateMediaUpload request;


            var photo = await botClient.GetFileAsync(fileId);


            MemoryStream ms; 
            using (ms = new MemoryStream())
            {
                await botClient.DownloadFileAsync(photo.FilePath, ms);
            }

            using (var stream = ms)
            {
                request = service.Files.Create(fileMetadata, stream, contentType);
                request.Upload();
            }
            var file = request.ResponseBody;
        }
        private static void FillSheetData(SheetsService sheetsService, string spreadshedId, List<object> data)
        {
            ValueRange valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>> { data };

            var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadshedId, SheetRange);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = appendRequest.Execute();
        }
    }
}
