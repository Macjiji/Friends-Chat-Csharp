using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Util;
using Java.Lang;
using Firebase.Database;
using System;
using Firebase.Auth;

namespace Friends_Chat
{
    public class FragmentChat : Fragment, IChildEventListener
    {

        protected View rootView;
        protected ListView groupChat, oneToOneChat;
        private OwnListViewAdapter groupChatAdapter, oneToOneChatAdapter;
        protected List<ChatRoom> groupChatList = new List<ChatRoom>();
        protected List<ChatRoom> oneToOneChatList = new List<ChatRoom>();
        protected List<string> oneToOneChatListCompleteName = new List<string>();
        protected IMainListener mainInterface;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (rootView == null)
            {
                rootView = inflater.Inflate(Resource.Layout.FragmentChatRooms, container, false);
                InitializeFirebase();
                InitializeListViews();
            }
            return rootView;
        }


        /// <summary>
        ///     Méthode permettant d'attacher IMainListener au fragment
        /// </summary>
        /// <param name="context">Le contexte utilisé, ici, ce sera implicitement l'activité Main</param>
        public override void OnAttach(Android.App.Activity activity)
        {
            base.OnAttach(activity);
            try
            {
                mainInterface = (IMainListener)activity;
            }
            catch (ClassCastException exception)
            {
                Log.Debug("Ex", exception.ToString());
            }
        }

        private void InitializeFirebase()
        {
            // Etape 1 : On récupère les salons de discussion de groupe
            FirebaseDatabase.Instance.Reference.Child("groupChat").AddChildEventListener(this);

            // Etape 2 : On récupère les salons de discussions en one to one. Ici, on ne va récupèrer que les salons contenant le nom de l'utilisateur
            //              à l'aide des méthode startAt et endAt de Firebase
            FirebaseDatabase.Instance.Reference.Child("oneToOneChat").AddChildEventListener(this);
        }

        private void InitializeListViews()
        {
            // Etape 1 : On récupère les références via la classe R
            groupChat = rootView.FindViewById<ListView>(Resource.Id.group_chat);
            oneToOneChat = rootView.FindViewById<ListView>(Resource.Id.one_to_one_chat);

            // Etape 2 : On crée les adaptateurs
            groupChatAdapter = new OwnListViewAdapter(Activity, groupChatList);
            groupChat.Adapter = groupChatAdapter;
            groupChat.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                mainInterface.GoIntoChatRoom(groupChatList[e.Position]); // On envoie à l'activité la chatroom à lancer
            };

            oneToOneChatAdapter = new OwnListViewAdapter(Activity, oneToOneChatList);
            oneToOneChat.Adapter = oneToOneChatAdapter;
            oneToOneChat.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                oneToOneChatList[e.Position].ChatName = oneToOneChatListCompleteName[e.Position];
                mainInterface.GoIntoChatRoom(oneToOneChatList[e.Position]); // On envoie à l'activité la chatroom à lancer
            };

        }

        public void OnCancelled(DatabaseError error) { }

        public void OnChildAdded(DataSnapshot snapshot, string previousChildName)
        {
            if (snapshot.Ref.Parent.Equals(FirebaseDatabase.Instance.Reference.Child("groupChat"))) // On teste si la référence à FirebaseDatabase est "groupChat" ...
            {
                groupChatList.Add(new ChatRoom(snapshot.Key, Convert.ToInt64(snapshot.Child("createAt").Value)));
                groupChatAdapter.NotifyDataSetChanged();
            }
            else if (snapshot.Ref.Parent.Equals(FirebaseDatabase.Instance.Reference.Child("oneToOneChat"))) // ... Puis on teste si la référence à FirebaseDatabase est "oneToOneChat"
            {
                if (snapshot.Key.Contains(FirebaseAuth.Instance.CurrentUser.DisplayName))
                {
                    string[] result = snapshot.Key.Split('~');
                    if (snapshot.Key.EndsWith("~" + FirebaseAuth.Instance.CurrentUser.DisplayName))
                    {
                        oneToOneChatList.Add(new ChatRoom(result[0], Convert.ToInt64(snapshot.Child("createAt").Value)));
                        oneToOneChatListCompleteName.Add(snapshot.Key);
                        oneToOneChatAdapter.NotifyDataSetChanged();
                    }
                    else
                    {
                        oneToOneChatList.Add(new ChatRoom(result[1], Convert.ToInt64(snapshot.Child("createAt").Value)));
                        oneToOneChatListCompleteName.Add(snapshot.Key);
                        oneToOneChatAdapter.NotifyDataSetChanged();
                    }
                }
            }
        }

        public void OnChildChanged(DataSnapshot snapshot, string previousChildName) { }

        public void OnChildMoved(DataSnapshot snapshot, string previousChildName) { }

        public void OnChildRemoved(DataSnapshot snapshot) { }


    }
}