using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Support.V7.App;
using Android.App;
using Firebase.Auth;
using Firebase.Database;
using Android.Util;
using Android.Content;

namespace Friends_Chat
{
    [Activity(Label = "Friends_Chat")]
    public class MainActivity : AppCompatActivity, IMainListener
    {

        protected FragmentAccount fragmentAccount;
        protected FragmentChat fragmentChat;

        protected TabLayout tabLayout;
        protected ViewPager viewPager;
        protected LinearLayout.LayoutParams layoutParamsSelected, layoutParamsDefault;
        protected View viewChat, viewAccount;

        private int[] tabIcons = {
            Resource.Drawable.custom_tab_icon_chat,
            Resource.Drawable.custom_tab_icon_account
        };



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            InitialiseLayoutParams();
            InitialiseTabLayout();

        }


        /// <summary>
        ///     Méthode issue de l'interface IMainListener, permettant de déconnecter un utilisateur.
        ///     La déconnexion se fait à partir du bouton dans FragmentAccount
        /// </summary>
        /// <see cref="FragmentAccount"/>
        /// <see cref="IMainListener"/>
        public void OnDisconnect()
        {
            FirebaseAuth.Instance.SignOut();
            StartActivity(typeof(LoginActivity));
        }

        /// <summary>
        ///     Méthode issue de l'interface IMainListener, permettant à un utilisateur de lancer l'activité de création d'un fil de discussion.
        ///     La création se fait via le bouton présent dans Fragment Account
        /// </summary>
        /// <see cref="FragmentAccount"/>
        /// <see cref="IMainListener"/>
        public void GoCreatingChat()
        {
            StartActivity(typeof(CreateChatActivity));
        }

        /// <summary>
        ///     Méthode issue de l'interface IMainListener, permettant à un utilisateur de lancer un fil de discussion.
        ///     L'appel se fait lorsqu'un utilisateur cliquera sur un élément de liste dans le FragmentChat
        /// </summary>
        /// <see cref="FragmentChat"/>
        /// <see cref="IMainListener"/>
        public void GoIntoChatRoom(ChatRoom chatRoom)
        {
            Intent chatRoomIntent = new Intent(this, typeof(ChatActivity));
            Log.Debug("Chat", "Name : " + chatRoom.ChatName);
            chatRoomIntent.PutExtra("chatName", chatRoom.ChatName);
            StartActivity(chatRoomIntent);
        }




        /// <summary>
        ///     Méthode d'initialisation de la barre
        /// </summary>
        private void InitialiseTabLayout()
        {
            tabLayout = FindViewById<TabLayout>(Resource.Id.main_tabs);
            viewPager = FindViewById<ViewPager>(Resource.Id.main_viewpager);
            viewPager.DrawingCacheEnabled = true;
            CreateViewPager(viewPager);
        }

        /// <summary>
        ///     Méthode permettant de générer le ViewPager avec l'Adaptateur
        /// </summary>
        /// <param name="viewPager">Le viewPager à générer</param>
        private void CreateViewPager(ViewPager viewPager)
        {

            var fragments = new Android.Support.V4.App.Fragment[]
            {
                new FragmentChat(),
                new FragmentAccount()
            };


            viewPager.Adapter = new OwnPagerAdapter(SupportFragmentManager, fragments);
            viewPager.CurrentItem = 0;

            tabLayout.SetupWithViewPager(viewPager);
            SetupTabIcons();
        }





        /// <summary>
        ///     Méthode d'initialisation des dimensions à attribuer aux boutons de la barre
        /// </summary>
        private void InitialiseLayoutParams()
        {
            layoutParamsSelected = new LinearLayout.LayoutParams(Resources.GetDimensionPixelSize(Resource.Dimension.value_50dp), Resources.GetDimensionPixelSize(Resource.Dimension.value_50dp));
            layoutParamsDefault = new LinearLayout.LayoutParams(Resources.GetDimensionPixelSize(Resource.Dimension.value_30dp), Resources.GetDimensionPixelSize(Resource.Dimension.value_30dp));
        }

        /// <summary>
        ///     Méthode pour créer les différentes icônes de la barre
        /// </summary>
        private void SetupTabIcons()
        {
            viewChat = LayoutInflater.Inflate(Resource.Layout.CustomTabIcon, tabLayout, false);
            viewChat.FindViewById<ImageView>(Resource.Id.icon).SetBackgroundResource(tabIcons[0]);
            viewChat.LayoutParameters = layoutParamsSelected;

            viewAccount = LayoutInflater.Inflate(Resource.Layout.CustomTabIcon, tabLayout, false);
            viewAccount.FindViewById<ImageView>(Resource.Id.icon).SetBackgroundResource(tabIcons[1]);
            viewAccount.LayoutParameters = layoutParamsDefault;


            if (tabLayout != null)
            {
                tabLayout.GetTabAt(0).SetCustomView(viewChat);
                tabLayout.GetTabAt(1).SetCustomView(viewAccount);
            }

            tabLayout.TabSelected += (object sender, TabLayout.TabSelectedEventArgs e) =>
            {
                ChangeTabSelected(e.Tab.CustomView);
            };

            tabLayout.TabUnselected += (object sender, TabLayout.TabUnselectedEventArgs e) =>
            {
                ChangeTabDefault(e.Tab.CustomView);
            };
        }




        /// <summary>
        ///     Méthode permettant de grandir un item du menu sélectionné
        /// </summary>
        /// <param name="view">La vue à agrandir</param>
        private void ChangeTabSelected(View view)
        {
            view.LayoutParameters = layoutParamsSelected;
        }

        /// <summary>
        ///     Méthode permettant de diminuer un item du menu déselecctioné
        /// </summary>
        /// <param name="view">La vue à diminuer</param>
        private void ChangeTabDefault(View view)
        {
            view.LayoutParameters = layoutParamsDefault;
        }


    }
}

