using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;

namespace Friends_Chat
{
    [Activity(Label = "CreateChatActivity")]
    public class CreateChatActivity : Activity, IChildEventListener
    {

        protected TextView titleUserSelection, titleChatName;
        protected Spinner chatType, userToSelect;
        protected ArrayAdapter adapterChatType, adapterUserList;
        protected EditText chatName;
        protected Button confirm;

        private List<string> userList = new List<string>();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateChat);

            InitializeUserList();
            InitializeTextViews();
            InitializeSpinner();
            InitializeEditText();
            InitializeButton();
            UpdateUI();

        }

        /// <summary>
        ///     Méthode d'initialisation de la liste des utilisateurs de l'application
        /// </summary>
        private void InitializeUserList()
        {
            FirebaseDatabase.Instance.Reference.Child("users").AddChildEventListener(this);
        }

        /// <summary>
        ///     Méthode d'initialisation des TextViews
        /// </summary>
        private void InitializeTextViews()
        {
            titleChatName = FindViewById<TextView>(Resource.Id.titleChatName);
            titleUserSelection = FindViewById<TextView>(Resource.Id.titleUserSelection);
        }

        /// <summary>
        ///     Méthode d'initialisation des Spinner
        /// </summary>
        private void InitializeSpinner()
        {
            // Etape 1 : On récupère les références
            chatType = FindViewById<Spinner>(Resource.Id.chatType);
            userToSelect = FindViewById<Spinner>(Resource.Id.chatUser);

            // Etape 2 : On crée les adaptateurs adéquats pour les spinners
            adapterChatType = ArrayAdapter.CreateFromResource(this, Resource.Array.group_type_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapterChatType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            chatType.Adapter = adapterChatType;

            adapterUserList = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, userList);
            userToSelect.Adapter = adapterUserList;

            // Etape 3 : on crée les listeners
            chatType.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
                UpdateUI();
            };
        }

        /// <summary>
        ///     Méthode d'initialisation du champ d'édition
        /// </summary>
        private void InitializeEditText()
        {
            chatName = FindViewById<EditText>(Resource.Id.chatRoomName);
        }

        private void InitializeButton()
        {
            confirm = FindViewById<Button>(Resource.Id.button_ok);
            confirm.Click += delegate
            {
                switch (chatType.SelectedItemPosition)
                {
                    case 0:
                        Log.Debug("Chat", "Chat de groupe sélectionné");
                        FirebaseDatabase.Instance.Reference.Child("groupChat").Child(chatName.Text.ToString()).Child("createAt").SetValueAsync(DateTimeOffset.Now.ToUnixTimeSeconds());
                        StartActivity(typeof(MainActivity));
                        break;
                    case 1:
                        Log.Debug("Chat", "Chat One to One sélectionné");
                        FirebaseDatabase.Instance.Reference.Child("oneToOneChat").Child(FirebaseAuth.Instance.CurrentUser.DisplayName + "~" + userList[userToSelect.SelectedItemPosition]).Child("createAt").SetValueAsync(DateTimeOffset.Now.ToUnixTimeSeconds());
                        StartActivity(typeof(MainActivity));
                        break;
                    default:
                        break;
                }
            };
        }

        /// <summary>
        ///     Méthode permettant de mettre à jour l'interface graphique de l'activité :
        ///         -> Le champ d'édition du nom de tchat collectif disparait si Tchat pour deux est sélectionné;
        ///         -> Le spinner de sélection d'un utilisateur disparait si Tchat de groupe est sélectionné;
        ///         -> Par défaut, tous les composants sont invisibles.
        /// </summary>
        private void UpdateUI()
        {
            switch (chatType.SelectedItemPosition)
            {
                case 0:
                    titleChatName.Visibility = ViewStates.Visible;
                    chatName.Visibility = ViewStates.Visible;
                    titleUserSelection.Visibility = ViewStates.Gone;
                    userToSelect.Visibility = ViewStates.Gone;
                    break;
                case 1:
                    titleChatName.Visibility = ViewStates.Gone;
                    chatName.Visibility = ViewStates.Gone;
                    titleUserSelection.Visibility = ViewStates.Visible;
                    userToSelect.Visibility = ViewStates.Visible;
                    break;
                default:
                    titleChatName.Visibility = ViewStates.Gone;
                    chatName.Visibility = ViewStates.Gone;
                    titleUserSelection.Visibility = ViewStates.Gone;
                    userToSelect.Visibility = ViewStates.Gone;
                    break;
            }
        }

        public void OnCancelled(DatabaseError error) { }

        public void OnChildAdded(DataSnapshot snapshot, string previousChildName)
        {
            if (!snapshot.Key.Equals(FirebaseAuth.Instance.CurrentUser.DisplayName))
            {
                userList.Add(snapshot.Key);
                adapterUserList.NotifyDataSetChanged();
            }
        }

        public void OnChildChanged(DataSnapshot snapshot, string previousChildName) { }

        public void OnChildMoved(DataSnapshot snapshot, string previousChildName) { }

        public void OnChildRemoved(DataSnapshot snapshot) { }



    }
}