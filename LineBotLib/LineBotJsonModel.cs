using Newtonsoft.Json;
using System;

namespace LineBotLib
{
    public class LineBotJsonModel
    {
        public class RootObject
        {
            public Event[] Events { get; set; }

            #region EventsModel

            /// <summary>
            /// EventsModel
            /// </summary>
            public class Event
            {
                /// <summary>
                /// 回覆用Token
                /// </summary>
                [JsonProperty(PropertyName = "replyToken")]
                public string ReplyToken { get; set; }

                /// <summary>
                /// 訊息類型
                /// </summary>
                [JsonProperty(PropertyName = "type")]
                public string Type { get; set; }

                /// <summary>
                /// 時間戳
                /// </summary>
                [JsonProperty(PropertyName = "timestamp")]
                public long Timestamp { get; set; }

                /// <summary>
                /// 轉為DateTime的時間
                /// </summary>
                public DateTime TimestampFormat { get; set; }

                /// <summary>
                /// 使用者類型(Group/Room/User)
                /// </summary>
                [JsonProperty(PropertyName = "source")]
                public Source Source { get; set; }

                /// <summary>
                /// 送來的資料
                /// </summary>
                [JsonProperty(PropertyName = "message")]
                public MessageRootObject Message { get; set; }
            }


            #endregion

            #region Source

            /// <summary>
            /// SourceModel
            /// </summary>
            public class Source
            {
                /// <summary>
                /// 分類(UserID、GroupID、RoomeID)，三者只能存在其中一個
                /// </summary>
                public string Type { get; set; }

                /// <summary>
                /// 使用者ID
                /// </summary>
                public string UserID { get; set; }

                /// <summary>
                /// 群組ID
                /// </summary>
                public string GroupID { get; set; }

                /// <summary>
                /// 房間ID
                /// </summary>
                public string RoomID { get; set; }
            }

            #endregion

            #region MessageRootObject

            /// <summary>
            /// MessageRootObject
            /// </summary>
            public class MessageRootObject
            {
                #region 基本

                /// <summary>
                /// ID
                /// </summary>
                public string id { get; set; }

                /// <summary>
                /// 顯示的樣式
                /// </summary>
                public string type { get; set; }

                #endregion

                #region 文字

                /// <summary>
                /// 使用者輸入文字
                /// </summary>
                public string text { get; set; }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// UserData
        /// </summary>
        public class UserData
        {
            public string displayName { get; set; }

            public string userId { get; set; }

            public string pictureUrl { get; set; }

            public string statusMessage { get; set; }
        }
    }
}