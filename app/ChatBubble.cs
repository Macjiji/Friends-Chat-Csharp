using System;

using Java.Text;
using Java.Util;
using Newtonsoft.Json;
using Firebase.Database;

namespace Friends_Chat
{
    class ChatBubble : Java.Lang.Object
    {

        public String userName;
        public String message;
        public long dateTime;


        /// <summary>
        ///     Constructeur par défaut
        /// </summary>
        public ChatBubble() { }

        /// <summary>
        ///     Constructeur prenant en paramètres tous les attributs de ChatBubble
        /// </summary>
        /// <param name="userName">La nom de l'utilisateur qui a envoyé le message</param>
        /// <param name="message">Le message envoyé</param>
        /// <param name="dateTime">La date au format long !</param>
        public ChatBubble(String userName, String message, long dateTime)
        {
            this.userName = userName;
            this.message = message;
            this.dateTime = dateTime;
        }

        [JsonProperty(PropertyName = "userName")]
        public String UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        [JsonProperty(PropertyName = "message")]
        public String Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        [JsonProperty(PropertyName = "dateTime")]
        public long DateTime
        {
            get { return this.dateTime; }
            set { this.dateTime = value; }
        }


        /// <summary>
        ///     Méthode permettant de transformer la valeur da la date (long) en Data au format texte. Cette méthode introduitla possibilité de créer le texte de date,
        ///     de façon dynamique ! (Il y a quelques secondes, Hier ...)
        /// </summary>
        /// <returns>La date au format appropriée</returns>
        public String GetLongToAgo()
        {
            DateFormat dateFormatter;
            long diff = DateTimeOffset.Now.ToUnixTimeSeconds() * 1000L - dateTime * 1000L;
            long diffSeconds = diff / 1000;
            long diffMinutes = diff / (60 * 1000) % 60;
            long diffHours = diff / (60 * 60 * 1000) % 24;
            long diffDays = diff / (24 * 60 * 60 * 1000);

            String time = null;
            if (diffDays > 0)
            {
                if (diffDays == 1)
                {
                    dateFormatter = DateFormat.GetTimeInstance(DateFormat.Short, Locale.Default);
                    time = "Hier" + ", " + dateFormatter.Format(new Date(dateTime * 1000L));
                }
                else
                {
                    dateFormatter = DateFormat.GetDateTimeInstance(DateFormat.Short, DateFormat.Short, Locale.Default);
                    time = dateFormatter.Format(new Date(dateTime * 1000L));
                }
            }
            else
            {
                if (diffHours > 2)
                {
                    if (diffHours == 1)
                    {
                        dateFormatter = DateFormat.GetTimeInstance(DateFormat.Short, Locale.Default);
                        time = "Aujourd'hui" + ", " + dateFormatter.Format(new Date(dateTime * 1000L));
                    }
                }
                else
                {
                    if (diffMinutes > 5)
                    {
                        time = "Il y a quelques minutes";
                    }
                    else
                    {
                        if (diffSeconds > 0)
                        {
                            time = "Il y a quelques secondes";
                        }
                    }

                }

            }

            return time;
        }


    }
}