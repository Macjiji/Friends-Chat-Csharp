using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

namespace Friends_Chat
{
    class OwnListViewAdapter : BaseAdapter<ChatRoom>
    {

        List<ChatRoom> chatRoomList;
        Activity context;

        public OwnListViewAdapter(Activity context, List<ChatRoom> chatRoomList) : base()
        {
            this.context = context;
            this.chatRoomList = chatRoomList;
        }


        public override long GetItemId(int position)
        {
            return position;
        }


        public override ChatRoom this[int position]
        {
            get { return chatRoomList[position]; }
        }


        public override int Count
        {
            get { return chatRoomList.Count; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            // Etape 1 : On utilise le LayoutInflater pour inclure le layout ChatRoom
            if (convertView == null)
            {
                convertView = LayoutInflater.From(context).Inflate(Resource.Layout.ChatRoom, parent, false);

                // view = context.LayoutInflater.Inflate(Resource.Layout.ChatRoom, null);
            }

            // Etape 2 : On récupère la référence des champs de texte
            TextView chatRoomName = convertView.FindViewById<TextView>(Resource.Id.chatName);
            TextView chatRoomCreatedAt = convertView.FindViewById<TextView>(Resource.Id.chatCreatedAt);

            // Etape 3 : On inclut le texte à intégrer
            chatRoomName.Text = chatRoomList[position].ChatName;
            chatRoomCreatedAt.Text = chatRoomList[position].GetDate();

            // Etape 4 : On retournne la vue créée
            return convertView;
        }

    }
}