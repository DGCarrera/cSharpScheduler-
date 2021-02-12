using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content;

namespace WGUClassScheduler
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button _loginButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            FindView();
            LinkEventHandlers();
        }

        private void LinkEventHandlers()
        {
            _loginButton.Click += _loginButton_Click;
        }

        private void _loginButton_Click(object sender, EventArgs e)
        {
            var tempUser = FindViewById<EditText>(Resource.Id.UsernameFront);
            var tempPass = FindViewById<EditText>(Resource.Id.PasswordFront);
            if (tempUser.Text == "test" && tempPass.Text == "test" )
            {
                Intent intent = new Intent(this, typeof(ActivityNav));
                StartActivity(intent);
            }
            
        }

        private void FindView()
        {
            _loginButton = FindViewById<Button>(Resource.Id.loginButton);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}