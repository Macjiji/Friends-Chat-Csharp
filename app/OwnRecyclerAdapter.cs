using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Firebase.Auth;

namespace Friends_Chat
{
    class OwnRecyclerAdapter : RecyclerView.Adapter
    {

        private readonly int VIEW_TYPE_SELF = 0;
        private readonly int VIEW_TYPE_OTHER = 1;

        private Context context;
        private List<ChatBubble> messageArrayList;

        /// <summary>
        ///     Méthode pour la vue d'un message de l'utilisateur courant
        /// </summary>
        public class ViewHolderSelf : RecyclerView.ViewHolder
        {
            public TextView message, timestamp;

            public ViewHolderSelf(View itemView) : base(itemView)
            {
                message = itemView.FindViewById<TextView>(Resource.Id.message);
                timestamp = itemView.FindViewById<TextView>(Resource.Id.timestamp);
            }

            public TextView Message { get; private set; }
            public TextView Timestamp { get; private set; }
        }

        /// <summary>
        ///     Classe pour la vue d'un message arrivant
        /// </summary>
        public class ViewHolderOther : RecyclerView.ViewHolder
        {
            public TextView message, timestamp;

            public ViewHolderOther(View itemView) : base(itemView)
            {
                message = itemView.FindViewById<TextView>(Resource.Id.message);
                timestamp = itemView.FindViewById<TextView>(Resource.Id.timestamp);
            }

            public TextView Message { get; private set; }
            public TextView Timestamp { get; private set; }
        }

        /// <summary>
        ///     Constructeur de l'adaptateur de RecyclerView
        /// </summary>
        /// <param name="context">Le contexte dans lequel l'utiliser</param>
        /// <param name="messageArrayList">La liste des messages</param>
        public OwnRecyclerAdapter(Context context, List<ChatBubble> messageArrayList)
        {
            this.context = context;
            this.messageArrayList = messageArrayList;
        }

        /// <summary>
        ///     Méthode héritée de ViewHolder.Adapter, permettant d'addicher les données dans chacun des ViewHolder.
        /// </summary>
        /// <param name="holder">Le ViewHolder qui contiendra les données</param>
        /// <param name="position">La position dans la liste des ChatBubble</param>
        /// <see cref="RecyclerView.Adapter"/>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            switch (GetItemViewType(position))
            {
                case 0 :
                    ChatBubble chatBubbleSelf = messageArrayList[position];
                    ((ViewHolderSelf)holder).message.Text = chatBubbleSelf.Message;
                    ((ViewHolderSelf)holder).timestamp.Text = chatBubbleSelf.GetLongToAgo();
                    break;
                case 1 :
                    ChatBubble chatBubbleOther = messageArrayList[position];
                    ((ViewHolderOther)holder).message.Text = chatBubbleOther.Message;
                    ((ViewHolderOther)holder).timestamp.Text = chatBubbleOther.GetLongToAgo() + " par " + chatBubbleOther.UserName;
                    break;
            }
        }

        /// <summary>
        ///     Méthode héritée de RecyclerView.Adapter, permettant de créer le ViewHolder adéquat à partir de son type.
        /// </summary>
        /// <param name="parent">La parent contenant le ViewHolder</param>
        /// <param name="viewType">Le type de la vue</param>
        /// <see cref="RecyclerView.Adapter"/>
        /// <returns>Le ViewHolder</returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            switch (viewType)
            {
                case 0 :
                    return new ViewHolderSelf(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ChatItemSelf, parent, false));
                case 1 :
                    return new ViewHolderOther(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ChatItemOther, parent, false));
            }
            return null;
        }

        /// <summary>
        ///     Méthode renvoyant le type de vue à afficher. On va utiliser pour cela le nom de l'utilisateur présent dans un objet ChatBubble.
        /// </summary>
        /// <param name="position">La position dans la liste des ChatBubble</param>
        /// <see cref="RecyclerView.Adapter"/>
        /// <returns>Le type de vue à afficher</returns>
        public override int GetItemViewType(int position)
        {
            ChatBubble chatBubble = messageArrayList[position];
            if (chatBubble.UserName.Equals(FirebaseAuth.Instance.CurrentUser.DisplayName))
            {
                return VIEW_TYPE_SELF;
            }
            else if (!chatBubble.UserName.Equals(FirebaseAuth.Instance.CurrentUser.DisplayName))
            {
                return VIEW_TYPE_OTHER;
            }
            return position;
        }

        public override int ItemCount
        {
            get
            {
                return messageArrayList.Count;
            }
        }

    }

}