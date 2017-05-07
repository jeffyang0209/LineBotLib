using LineBotLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MakerAPI.Util.Line
{
    public class HttpRequestUtil
    {
        private string messageApiUrl { get; set; } = "https://api.line.me/v2/bot/message/";
        private string userDataApiUrl { get; set; } = "https://api.line.me/v2/bot/profile/";
        private string channelAccessToken { get; set; }

        public HttpRequestUtil(string ChannelAccessToken)
        {
            this.channelAccessToken = ChannelAccessToken;
        }

        public HttpRequestUtil(string ChannelAccessToken, string MessageApiUrl, string UserDataApiUrl)
        {
            this.channelAccessToken = ChannelAccessToken;
            this.messageApiUrl = MessageApiUrl;
            this.userDataApiUrl = UserDataApiUrl;
        }

        /// <summary>
        /// LINE Request
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="json">json字串</param>
        /// <returns>bool</returns>
        private bool Line(string path, string json)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(messageApiUrl + path);
            byte[] data = Encoding.UTF8.GetBytes(json);
            req.ContentType = "application/json;charset=UTF-8";
            req.Accept = "application/json";
            req.ContentLength = data.Length;
            req.Headers.Add("Authorization", "Bearer " + channelAccessToken);
            req.Method = "POST";

            using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                streamWriter.Write(json);

            HttpWebResponse res = null;
            string responseStr = string.Empty;
            try
            {
                using (WebResponse wr = req.GetResponse())
                {
                    res = (HttpWebResponse)req.GetResponse();
                    responseStr = new StreamReader(res.GetResponseStream()).ReadToEnd();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 取得使用者資料!
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns>string</returns>
        public LineBotJsonModel.UserData GetUserData(string userID)
        {
            LineBotJsonModel.UserData userDate = null;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(userDataApiUrl + userID);
            req.Headers.Add("Authorization", "Bearer " + channelAccessToken);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";

            string result = string.Empty;

            // 取得回應資料
            using (HttpWebResponse response = req.GetResponse() as HttpWebResponse)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }

            if (!string.IsNullOrWhiteSpace(result))
                userDate = JsonConvert.DeserializeObject<LineBotJsonModel.UserData>(result);

            return userDate;
        }

        /// <summary>
        /// 回訊息
        /// </summary>
        /// <param name="jsonMessage">json字串</param>
        /// <returns>bool</returns>
        private bool Reply(string jsonMessage)
        {
            return Line("reply", jsonMessage);
        }

        /// <summary>
        /// 單一用戶推播
        /// </summary>
        /// <param name="jsonMessage">json字串</param>
        /// <returns>bool</returns>
        private bool Push(string jsonMessage)
        {
            return Line("push", jsonMessage);
        }

        /// <summary>
        /// 多用戶推播
        /// </summary>
        /// <param name="jsonMessage">json字串</param>
        /// <returns>bool</returns>
        private bool Multicast(string jsonMessage)
        {
            return Line("multicast", jsonMessage);
        }


        #region 多人(最多150人)

        /// <summary>
        /// 文字訊息
        /// </summary>
        /// <param name="to">多筆ID</param>
        /// <param name="messages">發送的訊息</param>
        public void MulticastText(List<string> to, string[] messages)
        {
            List<LineBotMessage.LineBotMessage.Message> liMessage = new List<LineBotMessage.LineBotMessage.Message>();

            foreach (string str in messages)
                liMessage.Add(new LineBotMessage.LineBotMessage.Text(str));

            LineBotMessage.LineBotMessage.Multicast multicast = new LineBotMessage.LineBotMessage.Multicast(liMessage, to);
            Push(JsonConvert.SerializeObject(multicast, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }

        #endregion

        #region 多人(大於150人)

        #endregion

        #region 單人

        /// <summary>
        /// 單筆文字訊息
        /// </summary>
        /// <param name="to">單筆ID</param>
        /// <param name="message">發送的訊息</param>
        public void PushText(string to, string message)
        {
            List<LineBotMessage.LineBotMessage.Message> liMessage = new List<LineBotMessage.LineBotMessage.Message>();
            liMessage.Add(new LineBotMessage.LineBotMessage.Text(message));
            LineBotMessage.LineBotMessage push = new LineBotMessage.LineBotMessage.Push(liMessage, to);

            Push(JsonConvert.SerializeObject(push, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }


        /// <summary>
        /// 單筆文字訊息
        /// </summary>
        /// <param name="to">單筆ID</param>
        /// <param name="message">發送的訊息</param>
        public void ReplyText(string to, string message)
        {
            List<LineBotMessage.LineBotMessage.Message> liMessage = new List<LineBotMessage.LineBotMessage.Message>();
            liMessage.Add(new LineBotMessage.LineBotMessage.Text(message));
            LineBotMessage.LineBotMessage push = new LineBotMessage.LineBotMessage.Push(liMessage, to);

            Reply(JsonConvert.SerializeObject(push, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }

        #endregion
    }
}