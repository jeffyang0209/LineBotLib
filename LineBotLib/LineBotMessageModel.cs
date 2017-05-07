using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace LineBotMessage
{
    /// <summary>
    /// LineBotJsonModel
    /// </summary>
    public class LineBotMessage
    {
        /// <summary>
        /// 傳送的訊息(必填) 最多五則訊息
        /// </summary>
        public List<Message> messages { get; set; }

        #region 多人推送

        /// <summary>
        /// Multicast
        /// </summary>
        public class Multicast : LineBotMessage
        {
            /// <summary>
            /// 推播時使用 最多150人
            /// </summary>
            public List<string> to { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="messages">傳送的訊息(必填) 最多五則訊息</param>
            /// <param name="to">多筆傳送ID</param>
            public Multicast(List<Message> messages, List<string> to)
            {
                this.messages = messages;
                this.to = to;
            }
        }

        #endregion

        #region 單人推送

        /// <summary>
        /// Push
        /// </summary>
        public class Push : LineBotMessage
        {
            /// <summary>
            /// 推播時使用 單人
            /// </summary>
            public string to { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="messages">傳送的訊息(必填) 最多五則訊息</param>
            /// <param name="to">傳送的ID</param>
            public Push(List<Message> messages, string to)
            {
                this.messages = messages;
                this.to = to;
            }
        }

        #endregion

        #region 回覆

        /// <summary>
        /// ReplyToken
        /// </summary>
        public class ReplyToken : LineBotMessage
        {
            /// <summary>
            /// 回覆時使用
            /// </summary>
            public string replyToken { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="messages">傳送的訊息(必填) 最多五則訊息</param>
            /// <param name="replyToken">回覆ID</param>
            public ReplyToken(List<Message> messages, string replyToken)
            {
                this.messages = messages;
                this.replyToken = replyToken;
            }
        }

        #endregion

        #region Message

        /// <summary>
        /// Message
        /// </summary>
        public abstract class Message
        {
            /// <summary>
            /// 訊息種類
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public abstract MessageType type { get; }
        }

        #region 列舉-訊息種類

        /// <summary>
        /// 訊息種類
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// 按鈕
            /// </summary>
            template,
            /// <summary>
            /// 文字
            /// </summary>
            text,
            /// <summary>
            /// 圖片
            /// </summary>
            image
        }

        #endregion

        #region 按鈕模板

        /// <summary>
        /// Template
        /// </summary>
        public class Template : Message
        {
            #region Message

            /// <summary>
            /// 訊息種類
            /// </summary>
            public override MessageType type { get; }

            #endregion

            /// <summary>
            /// 訊息出現時的提示文字(一閃即逝)
            /// </summary>
            public string altText { get; set; }

            /// <summary>
            /// 按鈕模板
            /// </summary>
            public ButtonTemplate template { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="altText">訊息出現時的提示文字(一閃即逝)</param>
            /// <param name="template">按鈕模板</param>
            public Template(string altText, ButtonTemplate template)
            {
                this.type = MessageType.template;
                this.altText = altText;
                this.template = template;
            }
        }

        #endregion

        #region 文字

        /// <summary>
        /// Text
        /// </summary>
        public class Text : Message
        {
            #region Message

            /// <summary>
            /// 訊息種類
            /// </summary>
            public override MessageType type { get; }

            #endregion

            /// <summary>
            /// 傳送的文字訊息
            /// </summary>
            public string text { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="text">傳送的文字訊息</param>
            public Text(string text)
            {
                this.type = MessageType.text;
                this.text = text;
            }
        }

        #endregion

        #region 圖片

        /// <summary>
        /// Image
        /// </summary>
        public class Image : Message
        {
            #region Message

            /// <summary>
            /// 傳送的文字訊息
            /// </summary>
            public override MessageType type { get; }

            #endregion

            /// <summary>
            /// 圖片網址
            /// 圖片路徑需https開頭 最長一千個字
            /// 解析度：最大1024*1024
            /// 檔案大小：最大1MB
            /// 檔案格式：限定JPEG
            /// </summary>
            public string originalContentUrl { get; set; }

            /// <summary>
            /// 預覽圖網址
            /// 圖片路徑需https開頭 最長一千個字
            /// 解析度：最大240*240
            /// 檔案大小：最大1MB
            /// 檔案格式：限定JPEG
            /// </summary>
            public string previewImageUrl { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="originalContentUrl">原圖網址，相關限制請F12查看</param>
            /// <param name="previewImageUrl">縮圖網址，相關限制請F12查看</param>
            public Image(string originalContentUrl, string previewImageUrl)
            {
                this.type = MessageType.image;
                this.originalContentUrl = originalContentUrl;
                this.previewImageUrl = previewImageUrl;
            }
        }

        #endregion

        #endregion

        #region TemplateType

        /// <summary>
        /// TemplateType
        /// </summary>
        public abstract class ButtonTemplate
        {
            /// <summary>
            /// 按鈕類型
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public abstract TemplateType type { get; }
        }

        #region 列舉-按鈕種類

        /// <summary>
        /// 按鈕種類
        /// </summary>
        public enum TemplateType
        {
            /// <summary>
            /// 確認選單
            /// </summary>
            confirm,
            /// <summary>
            /// 多 - 選單按鈕
            /// </summary>
            carousel,
            /// <summary>
            /// 單 - 選單按鈕
            /// </summary>
            buttons
        }

        #endregion

        #region 確認

        /// <summary>
        /// 確認選單
        /// </summary>
        public class Confirm : ButtonTemplate
        {
            #region TemplateType

            /// <summary>
            /// 按鈕類型
            /// </summary>
            public override TemplateType type { get; }

            #endregion

            /// <summary>
            /// 描述
            /// </summary>
            public string text { get; set; }

            /// <summary>
            /// 按鈕，最多二個
            /// </summary>        
            public List<ButtonText> actions { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="actions">按鈕，最多二個</param>
            /// <param name="text">描述</param>
            public Confirm(List<ButtonText> actions, string text)
            {
                this.type = TemplateType.confirm;
                this.actions = actions;
                this.text = text;
            }
        }

        #endregion

        #region 多-選單按鈕

        /// <summary>
        /// 多-選單按鈕
        /// </summary>
        public class Carousel : ButtonTemplate
        {
            #region TemplateTypeModel

            /// <summary>
            /// 按鈕類型
            /// </summary>
            public override TemplateType type { get; }

            #endregion

            /// <summary>
            /// 按鈕選單，最多五個
            /// </summary>
            public List<Column> columns { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="columns">按鈕選單，最多五個</param>
            public Carousel(List<Column> columns)
            {
                this.type = TemplateType.carousel;
                this.columns = columns;
            }
        }

        #endregion

        #region 單-選單按鈕

        /// <summary>
        /// 單-選單按鈕
        /// </summary>
        public class Buttons : ButtonTemplate, IColumn
        {
            #region TemplateTypeModel

            /// <summary>
            /// 按鈕類型
            /// </summary>
            public override TemplateType type { get; }

            #endregion

            #region IColumn

            /// <summary>
            /// 最上方圖片，請帶圖片網址
            /// </summary>
            public string thumbnailImageUrl { get; set; }

            /// <summary>
            /// 粗體標題
            /// </summary>
            public string title { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string text { get; set; }

            /// <summary>
            /// 按鈕，最多四個
            /// </summary>        
            public List<Action> actions { get; set; }

            #endregion

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="column">Column</param>
            public Buttons(Column column)
            {
                this.type = TemplateType.buttons;
                this.thumbnailImageUrl = column.thumbnailImageUrl;
                this.title = column.title;
                this.text = column.text;
                this.actions = column.actions;
            }
        }

        #endregion

        #endregion

        #region Column

        /// <summary>
        /// IColumn
        /// </summary>
        public interface IColumn
        {
            /// <summary>
            /// 最上方圖片，請帶圖片網址
            /// </summary>
            string thumbnailImageUrl { get; set; }

            /// <summary>
            /// 粗體標題
            /// </summary>
            string title { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            string text { get; set; }

            /// <summary>
            /// 按鈕，最多三個
            /// </summary>        
            List<Action> actions { get; set; }
        }

        /// <summary>
        /// Column
        /// </summary>
        public class Column : IColumn
        {
            /// <summary>
            /// 最上方圖片，請帶圖片網址
            /// </summary>
            public string thumbnailImageUrl { get; set; }

            /// <summary>
            /// 粗體標題
            /// </summary>
            public string title { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string text { get; set; }

            /// <summary>
            /// 按鈕，最多三個
            /// </summary>        
            public List<Action> actions { get; set; }


            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="thumbnailImageUrl">最上方圖片，請帶圖片網址</param>
            /// <param name="title">粗體標題</param>
            /// <param name="text">描述</param>
            /// <param name="actions">按鈕，最多四個</param>
            public Column(string thumbnailImageUrl, string title, string text, List<Action> actions)
            {
                this.thumbnailImageUrl = thumbnailImageUrl;
                this.title = title;
                this.text = text;
                this.actions = actions;
            }
        }

        #endregion

        #region Action

        /// <summary>
        /// ActionModel (文字/連結/自訂API Route)
        /// </summary>
        public abstract class Action
        {
            /// <summary>
            /// 點選按鈕後的觸發屬性
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public abstract ActionType type { get; }

            /// <summary>
            /// 按鈕名稱
            /// </summary>
            public string label { get; set; }
        }

        #region 列舉-按鈕點選內容

        /// <summary>
        /// 點選按鈕後的觸發屬性
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// 使用者點選後出現文字
            /// </summary>
            message,
            /// <summary>
            /// 使用者點選後打參數回自家API
            /// </summary>
            postback,
            /// <summary>
            /// 使用者點選後導至網址
            /// </summary>
            uri
        }

        #endregion

        #region 文字

        /// <summary>
        /// Message
        /// </summary>
        public class ButtonText : Action
        {
            #region ActionModel

            /// <summary>
            /// 點選按鈕後的觸發屬性
            /// </summary>
            public override ActionType type { get; }

            #endregion

            /// <summary>
            /// 使用者回覆文字
            /// </summary>
            public string text { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="label">按鈕名稱</param>
            /// <param name="text">使用者回覆文字</param>
            public ButtonText(string label, string text)
            {
                this.type = ActionType.message;
                this.label = label;
                this.text = text;
            }
        }

        #endregion

        #region 自訂API Route

        /// <summary>
        /// Postback
        /// </summary>
        public class ButtonPostback : Action
        {
            #region ActionModel

            /// <summary>
            /// 點選按鈕後的觸發屬性
            /// </summary>
            public override ActionType type { get; }

            #endregion

            /// <summary>
            /// 送URL參數回自己的WebApi的路徑
            /// </summary>
            public string data { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="label">按鈕名稱</param>
            /// <param name="data">送URL參數回自己的WebApi的路徑</param>
            public ButtonPostback(string label, string data)
            {
                this.type = ActionType.postback;
                this.label = label;
                this.data = data;
            }
        }

        #endregion

        #region 連結

        /// <summary>
        /// Uri
        /// </summary>
        public class ButtonUri : Action
        {
            #region ActionModel

            /// <summary>
            /// 點選按鈕後的觸發屬性
            /// </summary>
            public override ActionType type { get; }

            #endregion

            /// <summary>
            /// 使用時機：type為uri
            /// -連結網址
            /// </summary>
            public string uri { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="label">按鈕名稱</param>
            /// <param name="uri">使用者點選後導至網址</param>
            public ButtonUri(string label, string uri)
            {
                this.type = ActionType.uri;
                this.label = label;
                this.uri = uri;
            }
        }

        #endregion

        #endregion
    }
}
