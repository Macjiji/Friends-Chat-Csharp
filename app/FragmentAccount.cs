using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Android.Util;

namespace Friends_Chat
{
    public class FragmentAccount : Fragment
    {

        protected View rootView;
        protected Button createNewChat, disconnectUser;
        protected IMainListener mainInterface;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if(rootView == null)
            {
                rootView = inflater.Inflate(Resource.Layout.FragmentAccount, container, false);
                InitializeButtons();
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

        private void InitializeButtons()
        {
            createNewChat = rootView.FindViewById<Button>(Resource.Id.button_create_chat);
            disconnectUser = rootView.FindViewById<Button>(Resource.Id.button_disconnect);

            createNewChat.Click += delegate
            {
                mainInterface.GoCreatingChat();
            };

            disconnectUser.Click += delegate
            {
                mainInterface.OnDisconnect();
            };
        }

    }
}