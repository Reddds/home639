using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HomeBasePlugin;

namespace GmailNotify
{
    public class GmailNotify : IHomePlugin
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static readonly string[] Scopes = { GmailService.Scope.GmailReadonly };
        static readonly string ApplicationName = "Home Brain";

        private readonly GmailService service;

        Timer _checkTimer;

        private bool _lastUnredMailState = false;

        public GmailNotify()
        {
            UserCredential credential;
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                var credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });



/*            // Define parameters of request.
            var request = service.Users.Labels.List("me");

            // List labels.
            var labels = request.Execute().Labels;
            Console.WriteLine("Labels:");
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }*/




        }

        private void CheckMail()
        {
            var requestGetUnreadedMessages = service.Users.Messages.List("me");
            requestGetUnreadedMessages.LabelIds = new[]
            {
                "UNREAD",
                "IMPORTANT",
                "INBOX"
            };

            try
            {
                var response = requestGetUnreadedMessages.Execute();
                var msgs = response.Messages;
                Console.WriteLine("\n\n\nUnreaded!!!!!!!!!!!!!:");

                var newState = msgs != null && msgs.Count > 0;

                if (_lastUnredMailState != newState)
                {
                    _lastUnredMailState = newState;
                    _onUnreadMail?.Invoke(newState ? new[] {(object) 1} : null);
                }

/*

                    foreach (var message in msgs)
                    {
                        var getMsgReq = service.Users.Threads.Get("me", message.Id);
                        var getMsgResp = getMsgReq.Execute();
                        Console.WriteLine($"Labels: {string.Join(",", getMsgResp.Messages[0].LabelIds)}");
                        Console.WriteLine($"{getMsgResp.Messages[0].Payload.Headers[5].Value} : {getMsgResp.Messages[0].Snippet}");

                    }
*/
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }

        private Action<object[]> _onUnreadMail;

        public void SetEventHendler(string eventName, Action<object[]> eventAction)
        {
            if (eventName == "OnUnreadMail")
                _onUnreadMail = eventAction;
        }

        public void Start()
        {
            _checkTimer = new Timer(state =>
            {
                CheckMail();
            }, null, 0, 60000);
        }
    }
}
