using Com.Zoho.API.Authenticator;
using Com.Zoho.Officeintegrator;
using Com.Zoho.Officeintegrator.Dc;
using Com.Zoho.Officeintegrator.Logger;
using Com.Zoho.Officeintegrator.Util;
using Com.Zoho.Officeintegrator.V1;
using static Com.Zoho.Officeintegrator.Logger.Logger;


namespace PDF
{
    class EditPdfDocument
    {
        static void execute(String[] args)
        {
            try
            {
                // Initializing SDK once is enough. Calling here since code sample will be tested standalone. 
                // You can place SDK initializer code in your application and call once while your application start-up.
                initializeSdk();

                V1Operations sdkOperations = new V1Operations();
                EditPdfParameters editPdfParams = new EditPdfParameters();

                //Either use url as document source or attach the document in request body use below methods
                editPdfParams.Url = "https://demo.office-integrator.com/zdocs/BusinessIntelligence.pdf";

                //String inputFilePath = Path.Combine(System.Environment.CurrentDirectory, "../../../sample_documents/BusinessIntelligence.pdf");
                //StreamWrapper documentStreamWrapper = new StreamWrapper(inputFilePath);

                //editPdfParams.Document = documentStreamWrapper;

                DocumentInfo documentInfo = new DocumentInfo();

                documentInfo.DocumentName = "Untilted Document";
                // System time value used to generate unique document every time. You can replace based on your application.
                documentInfo.DocumentId = $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";

                editPdfParams.DocumentInfo = documentInfo;

                UserInfo userInfo = new UserInfo();

                userInfo.UserId = "1000";
                userInfo.DisplayName = "John";

                editPdfParams.UserInfo = userInfo;

                PdfEditorSettings editorSettings = new PdfEditorSettings();

                editorSettings.Unit = "in";
                editorSettings.Language = "en";
                editPdfParams.EditorSettings = editorSettings;

                PdfEditorUiOptions uiOptions = new PdfEditorUiOptions();

                uiOptions.FileMenu = "show";
                uiOptions.SaveButton = "show";

                editPdfParams.UiOptions = uiOptions;

                Dictionary<string, object> saveUrlParams = new Dictionary<string, object>();

                saveUrlParams.Add("id", 123456789);
                saveUrlParams.Add("auth_token", "oswedf32rk");

                Dictionary<string, object> saveUrlHeaders = new Dictionary<string, object>();

                saveUrlHeaders.Add("header1", "value1");
                saveUrlHeaders.Add("header2", "value2");

                CallbackSettings callbackSettings = new CallbackSettings();

                callbackSettings.Retries = 2;
                callbackSettings.Timeout = 10000;
                callbackSettings.SaveFormat = "pdf";
                callbackSettings.HttpMethodType = "post";
                callbackSettings.SaveUrlParams = saveUrlParams;
                callbackSettings.SaveUrlHeaders = saveUrlHeaders;
                callbackSettings.SaveUrl = "https://officeintegrator.zoho.com/v1/api/webhook/savecallback/601e12157123434d4e6e00cc3da2406df2b9a1d84a903c6cfccf92c8286";

                editPdfParams.CallbackSettings = callbackSettings;

                APIResponse<PdfEditorResponseHandler> response = sdkOperations.EditPdf(editPdfParams);
                int responseStatusCode = response.StatusCode;

                if (responseStatusCode >= 200 && responseStatusCode <= 299)
                {
                    CreateDocumentResponse documentResponse = (CreateDocumentResponse)response.Object;

                    Console.WriteLine("PDF Document id - {0}", documentResponse.DocumentId);
                    Console.WriteLine("PDF Document session id - {0}", documentResponse.SessionId);
                    Console.WriteLine("PDF Document session url - {0}", documentResponse.DocumentUrl);
                }
                else
                {
                    InvalidConfigurationException invalidConfiguration = (InvalidConfigurationException)response.Object;
                    string errorMessage = invalidConfiguration.Message;

                    /*long errorCode = invalidConfiguration.Code;
                    string errorKeyName = invalidConfiguration.KeyName;
                    string errorParameterName = invalidConfiguration.ParameterName;*/

                    Console.WriteLine("configuration error - {0}", errorMessage);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception in creating pdf document session url - ", e);
            }
        }

        static Boolean initializeSdk()
        {
            Boolean status = false;

            try
            {
                //Sdk application log configuration
                Logger logger = new Logger.Builder()
                        .Level(Levels.INFO)
                        //.filePath("<file absolute path where logs would be written>") //No I18N
                        .Build();

                List<IToken> tokens = new List<IToken>();
                Auth auth = new Auth.Builder()
                    .AddParam("apikey", "2ae438cf864488657cc9754a27daa480") //Update this apikey with your own apikey signed up in office inetgrator service
                    .AuthenticationSchema(new Authentication.TokenFlow())
                    .Build();

                tokens.Add(auth);

                Com.Zoho.Officeintegrator.Dc.Environment environment = new APIServer.Production("https://api.office-integrator.com"); // Refer this help page for api end point domain details -  https://www.zoho.com/officeintegrator/api/v1/getting-started.html

                new Initializer.Builder()
                    .Environment(environment)
                    .Tokens(tokens)
                    .Logger(logger)
                    .Initialize();

                status = true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception in Init SDK", e);
            }
            return status;
        }
    }
}