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
using Firebase.Database;
using Firebase.Auth;
using Newtonsoft.Json;
using Java.Util;

namespace Friends_Chat
{
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : Activity, IChildEventListener
    {

        protected RecyclerView recyclerView;
        private OwnRecyclerAdapter mAdapter;
        private List<ChatBubble> chatBubbleList = new List<ChatBubble>();

        protected EditText inputMessage;
        protected ImageButton btnSend;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Chat);

            InitializeFirebase();
            InitializeRecyclerView();
            InitializeImageButton();
            InitializeEditText();
        }

        private void InitializeFirebase()
        {
            if (Intent.GetStringExtra("chatName").Contains("~"))
            {
                // Cas n°1 : On est dans un chat entre deux utilisateurs (on ira dans "oneToOneChat")
                FirebaseDatabase
                        .Instance
                        .Reference
                        .Child("oneToOneChat")
                        .Child(Intent.GetStringExtra("chatName"))
                        .Child("messages")
                        .AddChildEventListener(this);
            }
            else
            {
                // Cas n°2 : On est dans un chat de groupe (on ira dans "groupChat")
                FirebaseDatabase
                    .Instance
                    .Reference
                    .Child("groupChat")
                    .Child(Intent.GetStringExtra("chatName"))
                    .Child("messages")
                    .AddChildEventListener(this);
            }
        }

        private void InitializeRecyclerView()
        {
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view);

            mAdapter = new OwnRecyclerAdapter(this, chatBubbleList);

            LinearLayoutManager layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetItemAnimator(new DefaultItemAnimator());
            recyclerView.SetAdapter(mAdapter);
        }


        private void InitializeImageButton()
        {
            btnSend = FindViewById<ImageButton>(Resource.Id.btn_send);

            btnSend.Click += delegate
            {
                if (inputMessage.Text.ToString().Trim().Length > 0)
                { // On teste dans un premier temps si l'utilisateur a écrit un message
                    inputMessage.Error = null;

                    if (Intent.GetStringExtra("chatName").Contains("~"))
                    {
                        // Cas n°1 : On est dans un chat entre deux utilisateurs (on ira dans "oneToOneChat")
                        ChatBubble model = new ChatBubble(FirebaseAuth.Instance.CurrentUser.DisplayName, inputMessage.Text.ToString(), DateTimeOffset.Now.ToUnixTimeSeconds());
                        FirebaseDatabase
                                .Instance
                                .Reference
                                .Child("oneToOneChat")
                                .Child(Intent.GetStringExtra("chatName"))
                                .Child("messages")
                                .Child(Convert.ToString(DateTimeOffset.Now.ToUnixTimeSeconds()))
                                .SetValueAsync(ChatBubbleModelToMap(model));
                    }
                    else
                    {
                        // Cas n°2 : On est dans un chat de groupe (on ira dans "groupChat")
                        ChatBubble model = new ChatBubble(FirebaseAuth.Instance.CurrentUser.DisplayName, inputMessage.Text.ToString(), DateTimeOffset.Now.ToUnixTimeSeconds());
                        FirebaseDatabase
                                .Instance
                                .Reference
                                .Child("groupChat")
                                .Child(Intent.GetStringExtra("chatName"))
                                .Child("messages")
                                .Child(Convert.ToString(DateTimeOffset.Now.ToUnixTimeSeconds()))
                                .SetValueAsync(ChatBubbleModelToMap(model));
                    }

                    inputMessage.Text = "";

                }
                else
                {
                    inputMessage.Error = "Vous devez renseigner un message !";
                }
            };

        }


        /// <summary>
        ///     Méthode d'initialisation du champ d'édition
        /// </summary>
        private void InitializeEditText()
        {
            inputMessage = FindViewById<EditText>(Resource.Id.message);
        }

        private HashMap ChatBubbleModelToMap(ChatBubble chatBubble)
        {
            HashMap map = new HashMap();
            map.Put("userName", chatBubble.UserName);
            map.Put("dateTime", chatBubble.DateTime);
            map.Put("message", chatBubble.Message);
            return map;
        }

        public void OnCancelled(DatabaseError error) { }

        public void OnChildAdded(DataSnapshot snapshot, string previousChildName)
        {
            ChatBubble chatBubble = new ChatBubble();
            chatBubble.UserName = Convert.ToString(snapshot.Child("userName").Value);
            chatBubble.Message = Convert.ToString(snapshot.Child("message").Value);
            chatBubble.DateTime = Convert.ToInt64(snapshot.Child("dateTime").Value);

            chatBubbleList.Add(chatBubble);
            mAdapter.NotifyDataSetChanged();
        }

        public void OnChildChanged(DataSnapshot snapshot, string previousChildName) { }

        public void OnChildMoved(DataSnapshot snapshot, string previousChildName) { }

        public void OnChildRemoved(DataSnapshot snapshot) { }

    }
}