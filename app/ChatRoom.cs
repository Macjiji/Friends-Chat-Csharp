using System;

using Java.Text;
using Java.Util;

namespace Friends_Chat
{
    public class ChatRoom
    {

        private String chatName;
        private long createAt;

        /// <summary>
        ///     Constructeur par défaut
        /// </summary>
        public ChatRoom() { }

        /// <summary>
        ///     Constructeur prenant en paramètres tous les attributs de ChatRoom
        /// </summary>
        /// <param name="chatName">Le nom du Tchat</param>
        /// <param name="createAt">La date de création du Tchat</param>
        public ChatRoom(String chatName, long createAt)
        {
            this.chatName = chatName;
            this.createAt = createAt;
        }

        public String ChatName
        {
            get { return this.chatName; }
            set { this.chatName = value; }
        }

        public long CreatedAt
        {
            get { return this.createAt; }
            set { this.createAt = value; }
        }

           
        /// <summary>
        ///     Méthode permettant de récupérer la date au forme texte
        /// </summary>
        /// <returns>La date au format texte, formatter à partir de Locale, et des formats courts</returns>
        public String GetDate() { return DateFormat.GetDateTimeInstance(DateFormat.Short, DateFormat.Short, Locale.Default).Format(createAt * 1000L); }


    }
}