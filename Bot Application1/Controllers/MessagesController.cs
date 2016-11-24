using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Bot_Application1.Models;
using System.Collections.Generic;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return

                if (activity.Text.ToLower().Contains("hi") || activity.Text.ToLower().Contains("hello"))
                {
                    Activity replyToConversation = activity.CreateReply($"Welcome to Contoso Bank, how may I help?");
                    replyToConversation.Recipient = activity.From;
                    replyToConversation.Type = "message";
                    replyToConversation.Attachments = new List<Attachment>();
                    List<CardImage> cardImages = new List<CardImage>();
                    cardImages.Add(new CardImage(url: "https://pbs.twimg.com/profile_images/709620078/contosobanklogo.jpg"));
                    List<CardAction> cardButtons = new List<CardAction>();
                    ThumbnailCard plCard = new ThumbnailCard()
                    {
                        Images = cardImages,
                    };
                    Attachment plAttachment = plCard.ToAttachment();
                    replyToConversation.Attachments.Add(plAttachment);
                    await connector.Conversations.SendToConversationAsync(replyToConversation);

                }
                else if (activity.Text.ToLower().Contains("exchange rate") || activity.Text.ToLower().Contains("currency"))
                {
                    Rootobject rootObject;
                    HttpClient client = new HttpClient();
                    string currUri = await client.GetStringAsync(new Uri("http://api.fixer.io/latest?base=NZD"));

                    rootObject = JsonConvert.DeserializeObject<Rootobject>(currUri);

                    string _base = rootObject._base;
                    string date = rootObject.date;
                    float aud = rootObject.rates.AUD;
                    float cad = rootObject.rates.CAD;
                    float eur = rootObject.rates.EUR;
                    float gbp = rootObject.rates.GBP;
                    float jpy = rootObject.rates.JPY;
                    float usd = rootObject.rates.USD;

                    Activity replyToConversation = activity.CreateReply($"Here you go");
                    replyToConversation.Recipient = activity.From;
                    replyToConversation.Type = "message";
                    replyToConversation.Attachments = new List<Attachment>();

                    ReceiptItem lineItem1 = new ReceiptItem()
                    { Text = "AUD " + aud };
                    ReceiptItem lineItem2 = new ReceiptItem()
                    { Text = "CAD " + cad };
                    ReceiptItem lineItem3 = new ReceiptItem()
                    { Text = "EUR " + eur };
                    ReceiptItem lineItem4 = new ReceiptItem()
                    { Text = "GBP " + gbp };
                    ReceiptItem lineItem5 = new ReceiptItem()
                    { Text = "JPY " + jpy };
                    ReceiptItem lineItem6 = new ReceiptItem()
                    { Text = "USD " + usd };

                    List<ReceiptItem> receiptList = new List<ReceiptItem>();
                    receiptList.Add(lineItem1);
                    receiptList.Add(lineItem2);
                    receiptList.Add(lineItem3);
                    receiptList.Add(lineItem4);
                    receiptList.Add(lineItem5);
                    receiptList.Add(lineItem6);

                    ReceiptCard plCard = new ReceiptCard()
                    {
                        Title = "NZD Foreign Exchange Rates",
                        Items = receiptList,
                    };

                    Attachment plAttachment = plCard.ToAttachment();
                    replyToConversation.Attachments.Add(plAttachment);
                    await connector.Conversations.SendToConversationAsync(replyToConversation);

                }
                else if (activity.Text.ToLower().Contains("thanks") || activity.Text.ToLower().Contains("thank you"))
                {
                    Activity reply = activity.CreateReply($"You're welcome");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                else
                {
                    Activity reply = activity.CreateReply($"I'm sorry I don't understand?");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }


            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}