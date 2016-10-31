using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace PizzaBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity != null)
            {
                // one of these will have an interface and process it
                if (activity.GetActivityType() == ActivityTypes.Message)
                {
                    try
                    {
                        await Conversation.SendAsync(activity, MakeRootDialog);
                    }
                    catch (FormCanceledException<PizzaOrder>)
                    {
                        ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                        Activity cancellationReply =
                            activity.CreateReply("Sorry you don't want a pizza right now. Come back and order soon!");
                        await connector.Conversations.ReplyToActivityAsync(cancellationReply);
                    }


                }
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        internal static IDialog<PizzaOrder> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(PizzaOrder.BuildForm)).Do(async (context, order) =>
            {
                var completed = await order;

                await
                    context.PostAsync(string.Format("You ordered a {0} pizza on {1} with {2} sides",
                        completed.Topping,
                        completed.Crust, completed.Sides.Count));

                await context.PostAsync("Processing your order.");
            });
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