using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Firebase.Auth;

namespace Friends_Chat
{
    [Activity(Label = "Friends_Chat", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : AppCompatActivity
    {
        protected EditText userMail, userPassword;
        protected Button login;
        protected TextView createAccount;

        private FirebaseAuth mAuth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            InitializeFirebase();
            InitializeButtons();
            InitializeEditText();
            InitializeTextViews();
        }

        private void AuthStateChanged(object sender, FirebaseAuth.AuthStateEventArgs e)
        {
            var user = e.Auth.CurrentUser;
            if (user != null)
            {
                StartActivity(typeof(MainActivity));
            }

        }

        protected override void OnStart()
        {
            base.OnStart();
            mAuth.AuthState += AuthStateChanged;
        }

        protected override void OnStop()
        {
            base.OnStop();
            mAuth.AuthState -= AuthStateChanged;
        }


        /// <summary>
        ///     Méthode d'initialisation de Firebase
        /// </summary>
        private void InitializeFirebase()
        {
            mAuth = FirebaseAuth.Instance;
        }

        /// <summary>
        ///     Méthode d'initialisation des boutons
        /// </summary>
        private void InitializeButtons()
        {
            login = FindViewById<Button>(Resource.Id.button_login);
            login.Click += async delegate
            {
                try
                {
                    await mAuth.SignInWithEmailAndPasswordAsync(userMail.Text.ToString(), userPassword.Text.ToString());
                }
                catch
                {
                    Toast.MakeText(this, "Authentication failed.", ToastLength.Short).Show();
                }
            };
        }

        /// <summary>
        ///     Méthode d'initialisation des EditText
        /// </summary>
        private void InitializeEditText()
        {
            userMail = FindViewById<EditText>(Resource.Id.email);
            userPassword = FindViewById<EditText>(Resource.Id.password);
        }

        /// <summary>
        ///     Méthode d'initialisation des TextViews
        /// </summary>
        private void InitializeTextViews()
        {
            createAccount = FindViewById<TextView>(Resource.Id.create_account);
            createAccount.Click += delegate
            {
                StartActivity(typeof(RegisterActivity));
            };
        }

    }
}